// <copyright file="BaseDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions
{
    using System;
    using LegendsGenerator.Contracts.Compiler;

    /// <summary>
    /// Base class of all definitoons.
    /// </summary>
    public class BaseDefinition
    {
        /// <summary>
        /// Gets the condition compiler
        /// </summary>
        protected IConditionCompiler? Compiler { get; private set; } = null;

        /// <summary>
        /// Attaches the compiler to this definition.
        /// </summary>
        /// <param name="compiler">The compiler.</param>
        public virtual void Attach(IConditionCompiler compiler)
        {
            this.Compiler = compiler;
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
    }
}
