// <copyright file="CompiledVariableAttribute.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Compiler
{
    using System;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class CompiledVariableAttribute : Attribute
    {
        public CompiledVariableAttribute(string name, Type type)
        {
            this.Name = name;
            this.Type = type;
        }

        public string Name { get; }

        public Type Type { get; }
    }
}
