// <copyright file="ICompiledCondition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Compiler
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Interface for compiled conditions.
    /// </summary>
    /// <typeparam name="T">The return type of the compiled condition.</typeparam>
    public interface ICompiledCondition<T>
    {
        /// <summary>
        /// Evaolulates the condition based on the passed in variables.
        /// </summary>
        /// <param name="random">The random number generator.</param>
        /// <param name="variables">The variables.</param>
        /// <returns>The result of evalulation.</returns>
        T Evaluate(Random random, IDictionary<string, BaseThing> variables);
    }
}