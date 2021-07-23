// <copyright file="CompiledVariableAttribute.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Compiler
{
    using System;

    /// <summary>
    /// An additional variable on a condition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class CompiledVariableAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompiledVariableAttribute"/> class.
        /// </summary>
        /// <param name="name">The variable name.</param>
        /// <param name="type">The variable type.</param>
        public CompiledVariableAttribute(string name, Type type)
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
    }
}
