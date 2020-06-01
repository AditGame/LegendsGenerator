// <copyright file="BaseDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions
{
    using System;
    using System.Collections.Generic;

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

        /// <summary>
        /// Creates a compiled condition.
        /// </summary>
        /// <typeparam name="T">The return type of the condition.</typeparam>
        /// <param name="condition">The condition to process.</param>
        /// <param name="formattedText">True to represent it as formatted text (complex overrules)</param>
        /// <param name="complex">True to represent as a complex condition.</param>
        /// <param name="parameters">The parameters list of the strings.</param>
        /// <returns></returns>
        protected ICompiledCondition<T> CreateCondition<T>(string condition, bool formattedText, bool complex, IList<string> parameters)
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
