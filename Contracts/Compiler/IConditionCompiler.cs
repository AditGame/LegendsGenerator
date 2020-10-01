// <copyright file="IConditionCompiler.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Compiler
{
    using System.Collections.Generic;

    /// <summary>
    /// Compiles a condition into something the system can understand.
    /// </summary>
    public interface IConditionCompiler
    {
        /// <summary>
        /// Update the global variables with new values.
        /// </summary>
        /// <param name="variables">The variables.</param>
        void UpdateGlobalVariables(IDictionary<string, object> variables);

        /// <summary>
        /// Compiles a complex (multi-line) condition.
        /// </summary>
        /// <typeparam name="T">The output type of the condition.</typeparam>
        /// <param name="condition">The condition.</param>
        /// <param name="variableNames">All variables used within.</param>
        /// <returns>The compiled condition.</returns>
        ICompiledCondition<T> AsComplex<T>(string condition, IEnumerable<string> variableNames);

        /// <summary>
        /// Compiles a simple text formatter.
        /// </summary>
        /// <param name="format">The format of the text.</param>
        /// <param name="variableNames">The variables used in the format.</param>
        /// <returns>The compiled condition.</returns>
        ICompiledCondition<string> AsFormattedText(string format, IEnumerable<string> variableNames);

        /// <summary>
        /// Compiles a simple (single line) condition.
        /// </summary>
        /// <typeparam name="T">The output type of the condition.</typeparam>
        /// <param name="condition">The condition.</param>
        /// <param name="variableNames">All variables used within.</param>
        /// <returns>The compiled condition.</returns>
        ICompiledCondition<T> AsSimple<T>(string condition, IEnumerable<string> variableNames);
    }
}