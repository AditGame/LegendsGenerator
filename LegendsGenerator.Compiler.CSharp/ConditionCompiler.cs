// <copyright file="ConditionCompiler.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Compiler.CSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using LegendsGenerator.Compiler.CSharp.Presentation;
    using LegendsGenerator.Contracts.Compiler;
    using LegendsGenerator.Contracts.Things;

    /// <summary>
    /// Processes a condition into a compiled condition.
    /// </summary>
    /// <typeparam name="TGlobals">The object which contains all the global variables.</typeparam>
    public class ConditionCompiler<TGlobals> : IConditionCompiler<TGlobals>
        where TGlobals : BaseGlobalVariables
    {
        /// <summary>
        /// The regex used to turn attribute arrows to their code equivilent.
        /// </summary>
        private static readonly Regex AttributeArrowRegex =
            new Regex("->([A-Za-z0-9_]*)(:[0-9])?");

        /// <summary>
        /// The regex used to turn aspect arrows to their code equivilent.
        /// </summary>
        private static readonly Regex AspectArrowRegex =
            new Regex("-<([A-Za-z0-9_]*)>([A-Za-z0-9_]*)(:[0-9])?");

        /// <summary>
        /// The list of variables that should be exposed to all conditions.
        /// </summary>
        private readonly TGlobals globalVariables;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionCompiler{TGlobals}"/> class.
        /// </summary>
        /// <param name="globalVariables">The variables that should be available to all conditions.</param>
        public ConditionCompiler(TGlobals globalVariables)
        {
            this.globalVariables = globalVariables;
        }

        /// <inheritdoc/>
        public IEditorIntegration EditorIntegration => new EditorIntegration();

        /// <inheritdoc/>
        public void UpdateGlobalVariables(Action<TGlobals> updateFunc)
        {
            updateFunc(this.globalVariables);
        }

        /// <summary>
        /// Compiles a complex (multi-line) condition and executes it.
        /// </summary>
        /// <typeparam name="T">The output type of the condition.</typeparam>
        /// <param name="rdm">The random number generator.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="variables">All variables used within.</param>
        /// <returns>The result of computation.</returns>
        public T EvalComplex<T>(Random rdm, string condition, IDictionary<string, BaseThing>? variables = null)
        {
            if (variables == null)
            {
                variables = new Dictionary<string, BaseThing>();
            }

            return this.AsComplex<T>(condition, variables.Select(x => new CompiledVariable(x.Key, x.Value.GetType()))).Evaluate(rdm, variables);
        }

        /// <inheritdoc/>
        public ICompiledCondition<T> AsComplex<T>(string condition, IEnumerable<CompiledVariable> variables)
        {
            return this.GenerateCompiledCondition<T>(
                ConvertArrows(condition),
                variables);
        }

        /// <summary>
        /// Compiles a simple (single line) condition and executes it.
        /// </summary>
        /// <typeparam name="T">The output type of the condition.</typeparam>
        /// <param name="rdm">The random number generator.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="variables">All variables used within.</param>
        /// <returns>The result of computation.</returns>
        public T EvalSimple<T>(Random rdm, string condition, IDictionary<string, BaseThing>? variables = null)
        {
            if (variables == null)
            {
                variables = new Dictionary<string, BaseThing>();
            }

            return this.AsSimple<T>(condition, variables.Select(x => new CompiledVariable(x.Key, x.Value.GetType()))).Evaluate(rdm, variables);
        }

        /// <inheritdoc/>
        public ICompiledCondition<T> AsSimple<T>(string condition, IEnumerable<CompiledVariable> variableNames)
        {
            if (!condition.EndsWith(";"))
            {
                condition += ";";
            }

            return this.GenerateCompiledCondition<T>(
                $"return {ConvertArrows(condition)}",
                variableNames);
        }

        /// <summary>
        /// Compiles a simple text formatter and executes it.
        /// </summary>
        /// <param name="rdm">The random number generator.</param>
        /// <param name="format">The format.</param>
        /// <param name="variables">All variables used within.</param>
        /// <returns>The result of computation.</returns>
        public string EvalFormattedText(Random rdm, string format, IDictionary<string, BaseThing>? variables = null)
        {
            if (variables == null)
            {
                variables = new Dictionary<string, BaseThing>();
            }

            return this.AsFormattedText(format, variables.Select(x => new CompiledVariable(x.Key, x.Value.GetType()))).Evaluate(rdm, variables);
        }

        /// <inheritdoc/>
        public ICompiledCondition<string> AsFormattedText(string format, IEnumerable<CompiledVariable> variableNames)
        {
            return this.GenerateCompiledCondition<string>(
                $"return $\"{ConvertArrows(format)}\";",
                variableNames);
        }

        /// <summary>
        /// Generates the method signature with the specified variables.
        /// </summary>
        /// <typeparam name="T">The return type.</typeparam>
        /// <param name="variables">The variable names.</param>
        /// <returns>The generated method signature.</returns>
        private static string GenerateMethodSignature<T>(IDictionary<string, Type> variables)
        {
            // Convert types to presentation types if needed.
            var presentationVariables = new Dictionary<string, Type>();
            foreach (var variable in variables)
            {
                if (PresentationConverters.TryGetPresentationType(variable.Value, out Type? presentation))
                {
                    presentationVariables[variable.Key] = presentation;
                }
                else
                {
                    presentationVariables[variable.Key] = variable.Value;
                }
            }

            StringBuilder bldr = new StringBuilder();
            bldr.AppendLine("using LegendsGenerator.Contracts;");
            bldr.AppendLine("using LegendsGenerator.Contracts.Things;");
            bldr.AppendLine("using LegendsGenerator.Compiler.CSharp.Presentation;");
            bldr.Append($"{typeof(T).Name} EvaluateCondition(");
            bldr.AppendJoin(", ", presentationVariables.Select(v => $"{v.Value.Name} {v.Key}"));
            bldr.Append(") {");

            return bldr.ToString();
        }

        /// <summary>
        /// Converts a string with out special shorthand to normal C#.
        /// </summary>
        /// <param name="condition">The condition to convert.</param>
        /// <returns>The pure C#.</returns>
        private static string ConvertArrows(string condition)
        {
            return ConvertAspectArrow(ConvertAttributeArrow(condition));
        }

        /// <summary>
        /// Converts an arrow to a call to get an effective attribute.
        /// </summary>
        /// <param name="condition">The condition to modify.</param>
        /// <returns>The condition with attribute arrows fixed.</returns>
        private static string ConvertAttributeArrow(string condition)
        {
            return AttributeArrowRegex.Replace(condition, match =>
            {
                string defaultVal = "0";
                if (!string.IsNullOrEmpty(match.Groups[2].ToString()))
                {
                    defaultVal = new string(match.Groups[2].ToString().Skip(1).ToArray());
                }

                return $".{nameof(BaseThing.EffectiveAttribute)}(\"{match.Groups[1]}\", {defaultVal})";
            });
        }

        /// <summary>
        /// Converts an arrow to a call to get an effective aspect.
        /// </summary>
        /// <param name="condition">The condition to modify.</param>
        /// <returns>The condition with aspect arrows fixed.</returns>
        private static string ConvertAspectArrow(string condition)
        {
            return AspectArrowRegex.Replace(condition, match =>
            {
                string? typeparam = "string";
                if (!string.IsNullOrEmpty(match.Groups[1].ToString()))
                {
                    typeparam = match.Groups[1].ToString();
                }

                string? defaultVal = null;
                if (!string.IsNullOrEmpty(match.Groups[3].ToString()))
                {
                    defaultVal = new string(match.Groups[3].ToString().Skip(1).ToArray());
                }

                if (defaultVal != null)
                {
                    return $".{nameof(BaseThing.EffectiveAspect)}<{typeparam}>(\"{match.Groups[2]}\", {defaultVal})";
                }
                else
                {
                    return $".{nameof(BaseThing.EffectiveAspect)}<{typeparam}>(\"{match.Groups[2]}\")";
                }
            });
        }

        /// <summary>
        /// Generates a compiled condition method based on the inner code.
        /// </summary>
        /// <typeparam name="T">The type of output.</typeparam>
        /// <param name="inner">The inner code.</param>
        /// <param name="variables">The variables.</param>
        /// <returns>The condition with the inner text as the method body.</returns>
        private CompiledCondition<T, TGlobals> GenerateCompiledCondition<T>(string inner, IEnumerable<CompiledVariable> variables)
        {
            IDictionary<string, Type> combinedVariables =
                this.globalVariables.ToDictionary().ToDictionary(kvp => kvp.Key, kvp => kvp.Value.GetType())
                .Concat(variables.ToDictionary(n => n.Name, n => n.Type))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            combinedVariables[Constants.RandomVariableName] = typeof(Random);
            StringBuilder bldr = new StringBuilder();
            bldr.AppendLine(GenerateMethodSignature<T>(combinedVariables));
            bldr.AppendLine(inner);
            bldr.AppendLine("}");

            var compiledCondition = MethodDelegateCache<T>.Get(bldr.ToString());

            return new CompiledCondition<T, TGlobals>(
                compiledCondition, combinedVariables.Select(kvp => kvp.Key).ToList(), this.globalVariables);
        }
    }
}
