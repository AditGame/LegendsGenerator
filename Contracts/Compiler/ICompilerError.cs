// <copyright file="ICompilerErrorException.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Compiler
{
    /// <summary>
    /// Interface for exceptions with compiler errors.
    /// </summary>
    public interface ICompilerError
    {
        /// <summary>
        /// Gets the error of this compilation.
        /// </summary>
        public string? ErrorMessage { get; }
    }
}
