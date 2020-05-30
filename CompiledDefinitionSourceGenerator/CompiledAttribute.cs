// <copyright file="CompiledAttribute.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace CompiledDefinitionSourceGenerator
{
    using System;

    /// <summary>
    /// Indicates the proeprty should be compiled.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CompiledAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompiledAttribute"/> class.
        /// </summary>
        /// <param name="returnType">The return type of the compiled statement.</param>
        /// <param name="parameterNames">The names of the parameters expected in this compiled statement.</param>
        public CompiledAttribute(Type returnType, params string[] parameterNames)
        {
            this.ReturnType = returnType;
            this.ParameterNames = parameterNames;
        }

        /// <summary>
        /// Gets the type of the return for the compiled condition.
        /// </summary>
        public Type ReturnType { get; }

        /// <summary>
        /// Gets the names of the parameters in this compiled statement.
        /// </summary>
        public string[] ParameterNames { get; }
    }
}
