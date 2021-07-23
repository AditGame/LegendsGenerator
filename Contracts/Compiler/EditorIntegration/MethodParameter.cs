// -------------------------------------------------------------------------------------------------
// <copyright file="MethodParameter.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Contracts.Compiler.EditorIntegration
{
    using System;

    /// <summary>
    /// A parameter to oa method.
    /// </summary>
    public class MethodParameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodParameter"/> class.
        /// </summary>
        /// <param name="name">The parameter name.</param>
        /// <param name="type">The parameter type.</param>
        public MethodParameter(string name, Type type)
        {
            this.Name = name;
            this.Type = type;
        }

        /// <summary>
        /// Gets the name of this parameter.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the type of this parameter.
        /// </summary>
        public Type Type { get; }
    }
}
