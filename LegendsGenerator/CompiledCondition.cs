// <copyright file="CompiledCondition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using CSScriptLib;
    using LegendsGenerator.Contracts;
    using LegendsGenerator.Contracts.Compiler;

    /// <summary>
    /// Processes conditions.
    /// </summary>
    /// <typeparam name="T">The output type of the condition.</typeparam>
    public class CompiledCondition<T> : ICompiledCondition<T>
    {
        /// <summary>
        /// The compiled condition.
        /// </summary>
        private MethodDelegate<T> compiledCondition;

        /// <summary>
        /// The names of the variables.
        /// </summary>
        private IList<string> variableNames;

        /// <summary>
        /// The list of variables that should be exposed to all conditions.
        /// </summary>
        private IDictionary<string, object> globalVariables = new Dictionary<string, object>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CompiledCondition{T}"/> class.
        /// </summary>
        /// <param name="compiledCondition">The compiled condition.</param>
        /// <param name="variableNames">The names of the variables in the condition.</param>
        /// <param name="globalVariables">The global variables.</param>
        internal CompiledCondition(
            MethodDelegate<T> compiledCondition,
            IList<string> variableNames,
            IDictionary<string, object> globalVariables)
        {
            this.compiledCondition = compiledCondition;
            this.variableNames = variableNames;
            this.globalVariables = globalVariables;
        }

        /// <inheritdoc/>
        public T Evaluate(Random random, IDictionary<string, BaseThing> variables)
        {
            IList<object?> functionParameters = new List<object?>();
            foreach (var variableName in this.variableNames)
            {
                if (variableName.Equals(ConditionCompiler.RandomVariableName, StringComparison.OrdinalIgnoreCase))
                {
                    functionParameters.Add(random);
                }
                else if (this.globalVariables.TryGetValue(variableName, out object? value))
                {
                    functionParameters.Add(value);
                }
                else if (variables.TryGetValue(variableName, out BaseThing? thing))
                {
                    functionParameters.Add(thing);
                }
                else
                {
                    functionParameters.Add(null);
                }
            }

            return this.compiledCondition(functionParameters.ToArray());
        }
    }
}
