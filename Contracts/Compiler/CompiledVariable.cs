// <copyright file="CompiledVariable.cs" company="Tom Luppi">
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
    /// A variable definition for the compiler.
    /// </summary>
    public class CompiledVariable
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <param name="type">The type of the variable.</param>
        public CompiledVariable(string name, Type type)
        {
            this.Name = name;
            this.Type = type;
        }

        /// <summary>
        /// Gets the name of the variable.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the type of the variable.
        /// </summary>
        public Type Type { get; }

        public override string ToString()
        {
            return $"{this.Name} ({this.Type.Name})";
        }
    }
}
