// <copyright file="IConditionCompiler.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Compiler
{
    using System.Collections.Generic;
    using LegendsGenerator.Compiler.CSharp;

    /// <summary>
    /// Compiles a condition into something the system can understand.
    /// </summary>
    public interface IConditionCompiler
    {
        /// <summary>
        /// Gets a class which assists with the editor integrating with this compiler.
        /// </summary>
        IEditorIntegration EditorIntegration { get; }

        /// <summary>
        /// Compiles a complex (multi-line) condition.
        /// </summary>
        /// <typeparam name="T">The output type of the condition.</typeparam>
        /// <param name="condition">The condition.</param>
        /// <param name="variables">All variables used within.</param>
        /// <returns>The compiled condition.</returns>
        ICompiledCondition<T> AsComplex<T>(string condition, IEnumerable<CompiledVariable> variables);

        /// <summary>
        /// Compiles a simple text formatter.
        /// </summary>
        /// <param name="format">The format of the text.</param>
        /// <param name="variables">All variables used within.</param>
        /// <returns>The compiled condition.</returns>
        ICompiledCondition<string> AsFormattedText(string format, IEnumerable<CompiledVariable> variables);

        /// <summary>
        /// Compiles a simple (single line) condition.
        /// </summary>
        /// <typeparam name="T">The output type of the condition.</typeparam>
        /// <param name="condition">The condition.</param>
        /// <param name="variables">All variables used within.</param>
        /// <returns>The compiled condition.</returns>
        ICompiledCondition<T> AsSimple<T>(string condition, IEnumerable<CompiledVariable> variables);
    }
}