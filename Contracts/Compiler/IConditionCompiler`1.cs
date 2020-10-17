// <copyright file="IConditionCompiler`1.cs" company="Tom Luppi">
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
    /// Interface for condition compilers.
    /// </summary>
    /// <typeparam name="TGlobals">The type of global variables.</typeparam>
    public interface IConditionCompiler<TGlobals> : IConditionCompiler
    {
        /// <summary>
        /// Update the global variables with new values.
        /// </summary>
        /// <param name="updateFunc">The function to update the existing variables.</param>
        void UpdateGlobalVariables(Action<TGlobals> updateFunc);
    }
}
