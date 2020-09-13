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
    using LegendsGenerator.Contracts.Compiler;
    using LegendsGenerator.Contracts.Things;

    /// <summary>
    /// Processes a condition into a compiled condition.
    /// </summary>
    public class ConditionCompiler : IConditionCompiler
    {
        /// <summary>
        /// The name of the Random parameter.
        /// </summary>
        public const string RandomVariableName = "Rand";

        /// <summary>
        /// The regex used to turn attribute arrows to their code equivilent.
        /// </summary>
        private static readonly Regex AttributeArrowRegex =
            new Regex("->([A-Za-z0-9_]*)");

        /// <summary>
        /// The list of variables that should be exposed to all conditions.
        /// </summary>
        private IDictionary<string, object> globalVariables = new Dictionary<string, object>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionCompiler"/> class.
        /// </summary>
        /// <param name="globalVariables">The variables that should be available to all conditions.</param>
        public ConditionCompiler(IDictionary<string, object> globalVariables)
        {
            this.globalVariables = globalVariables;
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

            return this.AsComplex<T>(condition, variables.Select(x => x.Key)).Evaluate(rdm, variables);
        }

        /// <inheritdoc/>
        public ICompiledCondition<T> AsComplex<T>(string condition, IEnumerable<string> variableNames)
        {
            return this.GenerateCompiledCondition<T>(
                ConvertAttributeArrow(condition),
                variableNames);
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

            return this.AsSimple<T>(condition, variables.Select(x => x.Key)).Evaluate(rdm, variables);
        }

        /// <inheritdoc/>
        public ICompiledCondition<T> AsSimple<T>(string condition, IEnumerable<string> variableNames)
        {
            if (!condition.EndsWith(";"))
            {
                condition += ";";
            }

            return this.GenerateCompiledCondition<T>(
                $"return {ConvertAttributeArrow(condition)}",
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

            return this.AsFormattedText(format, variables.Select(x => x.Key)).Evaluate(rdm, variables);
        }

        /// <inheritdoc/>
        public ICompiledCondition<string> AsFormattedText(string format, IEnumerable<string> variableNames)
        {
            return this.GenerateCompiledCondition<string>(
                $"return $\"{ConvertAttributeArrow(format)}\";",
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
            StringBuilder bldr = new StringBuilder();
            bldr.AppendLine("using LegendsGenerator.Contracts;");
            bldr.AppendLine("using LegendsGenerator.Contracts.Things;");
            bldr.Append($"{typeof(T).Name} EvaluateCondition(");
            bldr.AppendJoin(", ", variables.Select(v => $"{v.Value.Name} {v.Key}"));
            bldr.Append(") {");

            return bldr.ToString();
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
                return $".{nameof(BaseThing.EffectiveAttribute)}(\"{match.Groups[1]}\")";
            });
        }

        /// <summary>
        /// Generates a combiled condition method based on the inner code.
        /// </summary>
        /// <typeparam name="T">The type of output.</typeparam>
        /// <param name="inner">The inner code.</param>
        /// <param name="variableNames">The variable names.</param>
        /// <returns>The condition with the inner text as the method body.</returns>
        private CompiledCondition<T> GenerateCompiledCondition<T>(string inner, IEnumerable<string> variableNames)
        {
            IDictionary<string, Type> combinedVariables =
                this.globalVariables.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.GetType())
                .Concat(variableNames.ToDictionary(n => n, n => typeof(BaseThing)))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            combinedVariables[RandomVariableName] = typeof(Random);
            StringBuilder bldr = new StringBuilder();
            bldr.AppendLine(GenerateMethodSignature<T>(combinedVariables));
            bldr.AppendLine(inner);
            bldr.AppendLine("}");

            var compiledCondition = MethodDelegateCache<T>.Get(bldr.ToString());

            return new CompiledCondition<T>(
                compiledCondition, combinedVariables.Select(kvp => kvp.Key).ToList(), this.globalVariables);
        }
    }
}
