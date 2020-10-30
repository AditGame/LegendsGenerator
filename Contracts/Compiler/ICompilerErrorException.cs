// <copyright file="ICompilerErrorException.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for exceptions with compiler errors.
    /// </summary>
    public interface ICompilerErrorException
    {
        /// <summary>
        /// Gets the error of this compilation.
        /// </summary>
        public string? Error { get; }
    }
}
