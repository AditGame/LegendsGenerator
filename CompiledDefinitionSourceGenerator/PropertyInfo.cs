// <copyright file="PropertyInfo.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace CompiledDefinitionSourceGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;

    /// <summary>
    /// Information about a property
    /// </summary>
    public class PropertyInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyInfo"/> class.
        /// </summary>
        /// <param name="property">The property</param>
        public PropertyInfo(IPropertySymbol property)
        {
            this.Name = property.Name;
            var attributeConstructorArguments = property
                .GetAttributes(typeof(CompiledAttribute).FullName!)
                .SelectMany(a => a.ConstructorArguments);

            this.ReturnType = attributeConstructorArguments
                .Select(x => x.Value)
                .OfType<Type>()
                .First();

            this.Variables = attributeConstructorArguments
                .Select(x => x.Value)
                .OfType<string>()
                .ToList();
        }

        /// <summary>
        /// Gets or sets the name of this property.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the return type of the compiled attribute.
        /// </summary>
        public Type ReturnType { get; set; }

        /// <summary>
        /// Gets or sets the variable names.
        /// </summary>
        public IReadOnlyCollection<string> Variables { get; set; }
    }
}
