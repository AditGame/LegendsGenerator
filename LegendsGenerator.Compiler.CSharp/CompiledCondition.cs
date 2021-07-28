// <copyright file="CompiledCondition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Compiler.CSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using CSScriptLib;
    using LegendsGenerator.Compiler.CSharp.Presentation;
    using LegendsGenerator.Contracts.Compiler;
    using LegendsGenerator.Contracts.Things;

    /// <summary>
    /// Processes conditions.
    /// </summary>
    /// <typeparam name="TOut">The output type of the condition.</typeparam>
    /// <typeparam name="TGlobals">THe type of the global variables.</typeparam>
    public class CompiledCondition<TOut, TGlobals> : ICompiledCondition<TOut>
        where TGlobals : BaseGlobalVariables
    {
        /// <summary>
        /// The compiled condition.
        /// </summary>
        private readonly MethodDelegate<TOut> compiledCondition;

        /// <summary>
        /// The names of the variables.
        /// </summary>
        private readonly IList<string> variableNames;

        /// <summary>
        /// The list of variables that should be exposed to all conditions.
        /// </summary>
        private readonly TGlobals globalVariables;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompiledCondition{TOut, TGlobals}"/> class.
        /// </summary>
        /// <param name="compiledCondition">The compiled condition.</param>
        /// <param name="variableNames">The names of the variables in the condition.</param>
        /// <param name="globalVariables">The global variables.</param>
        internal CompiledCondition(
            MethodDelegate<TOut> compiledCondition,
            IList<string> variableNames,
            TGlobals globalVariables)
        {
            this.compiledCondition = compiledCondition;
            this.variableNames = variableNames;
            this.globalVariables = globalVariables;
        }

        /// <inheritdoc/>
        public TOut Evaluate(Random random, IDictionary<string, BaseThing> variables)
        {
            IList<object?> functionParameters = new List<object?>();
            var globals = this.globalVariables.ToDictionary();
            foreach (var variableName in this.variableNames)
            {
                if (variableName.Equals(Constants.RandomVariableName, StringComparison.OrdinalIgnoreCase))
                {
                    functionParameters.Add(random);
                }
                else if (globals.TryGetValue(variableName, out object? value))
                {
                    functionParameters.Add(value);
                }
                else if (variables.TryGetValue(variableName, out BaseThing? thing))
                {
                    // We need to convert things to their presentation type if needed.
                    if (PresentationConverters.TryConvertToPresentationType(thing, this.globalVariables.World ?? throw new InvalidOperationException("Global variable World can not be null."), out object? presentation))
                    {
                        functionParameters.Add(presentation);
                    }
                    else
                    {
                        functionParameters.Add(thing);
                    }
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
