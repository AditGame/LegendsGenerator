// <copyright file="BaseDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text.Json.Serialization;

    using LegendsGenerator.Contracts.Compiler;

    /// <summary>
    /// Base class of all definitions.
    /// </summary>
    public class BaseDefinition
    {
        /// <summary>
        /// A value which indicates something is unset.
        /// </summary>
        public static readonly string UnsetString = "UNSET";

        /// <summary>
        /// Gets the condition compiler
        /// </summary>
        protected IConditionCompiler? Compiler { get; private set; }

        /// <summary>
        /// Gets the definition which holds this definition.
        /// </summary>
        protected BaseDefinition? UpsteamDefinition { get; private set; }

        /// <summary>
        /// Reattaches all the definitions, in case new definitions were added as downstream dependencies.
        /// </summary>
        public void Reattach()
        {
            if (this.Compiler == null)
            {
                throw new InvalidOperationException("Can not reattach before attaching.");
            }

            this.Attach(this.Compiler, this.UpsteamDefinition);
        }

        /// <summary>
        /// Attaches the compiler to this definition.
        /// </summary>
        /// <param name="compiler">The compiler.</param>
        /// <param name="upsteamDefinition">The upstream definition.</param>
        public virtual void Attach(IConditionCompiler compiler, BaseDefinition? upsteamDefinition = null)
        {
            this.Compiler = compiler;
            this.UpsteamDefinition = upsteamDefinition;
        }

        /// <summary>
        /// Compiles all conditions to prepare for faster future execution.
        /// </summary>
        public virtual void Compile()
        {
            if (this.Compiler == null)
            {
                throw new InvalidOperationException("You must attach the compiler via AttachCompiler before calling Eval methods.");
            }
        }

        /// <summary>
        /// Combines all of the type reassignments.
        /// </summary>
        /// <returns>A dictionary of variable name to the new type.</returns>
        protected virtual IDictionary<string, Type> CombinedVariableTypeReassignments()
        {
            Dictionary<string, Type> param = new Dictionary<string, Type>();

            foreach (var entry in this.UpsteamDefinition?.CombinedVariableTypeReassignments() ?? new Dictionary<string, Type>())
            {
                param[entry.Key] = entry.Value;
            }

            return param;
        }

        /// <summary>
        /// Returns all additional class-wide parameters, cluding upstream parameters.
        /// </summary>
        /// <returns>The list of upstream parameters.</returns>
        protected virtual List<CompiledVariable> CombinedAdditionalParametersForClass()
        {
            List<CompiledVariable> param = new List<CompiledVariable>();

            if (this.GetType().GetCustomAttribute<UsesAdditionalParametersForHoldingClassAttribute>() != null)
            {
                param.AddRange(this.UpsteamDefinition?.CombinedAdditionalParametersForClass() ?? new List<CompiledVariable>());
            }

            return param;
        }

        /// <summary>
        /// Creates a compiled condition.
        /// </summary>
        /// <typeparam name="T">The return type of the condition.</typeparam>
        /// <param name="condition">The condition to process.</param>
        /// <param name="formattedText">True to represent it as formatted text (complex overrules)</param>
        /// <param name="complex">True to represent as a complex condition.</param>
        /// <param name="parameters">The parameters list of the strings.</param>
        /// <returns></returns>
        protected ICompiledCondition<T> CreateCondition<T>(string condition, bool formattedText, bool complex, IList<CompiledVariable> parameters)
        {
            if (this.Compiler == null)
            {
                throw new ApplicationException("Compiler must be attached before this method is called.");
            }

            if (complex)
            {
                return this.Compiler.AsComplex<T>(condition, parameters);
            }
            else if (formattedText && typeof(T) == typeof(string))
            {
                return (this.Compiler.AsFormattedText(condition, parameters) as ICompiledCondition<T>)!;
            }
            else
            {
                return this.Compiler.AsSimple<T>(condition, parameters);
            }
        }
    }
}
