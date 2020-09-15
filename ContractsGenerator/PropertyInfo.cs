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
    /// Information about a property.
    /// </summary>
    public class PropertyInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyInfo"/> class.
        /// </summary>
        /// <param name="property">The property.</param>
        public PropertyInfo(IPropertySymbol property)
        {
            this.Name = property.Name;
            var attributeConstructorArguments = property
                .GetAttributes("CompiledAttribute", "CompiledDictionaryAttribute")
                .SelectMany(a => a.ConstructorArguments);

            this.ReturnType = attributeConstructorArguments
                .First(x => x.Type?.Name == "Type").Value?.ToString() ?? "void";

            this.Variables = attributeConstructorArguments
                .First(x => x.Type?.ToString().Equals("String[]", StringComparison.OrdinalIgnoreCase) == true).Values
                .Select(x => x.Value?.ToString() ?? "void")
                .ToList();

            this.AsFormattedText = property
                .GetAttributes("CompiledAttribute")
                .SelectMany(a => a.NamedArguments)
                .FirstOrDefault(a => a.Key.Equals("AsFormattedText"))
                .Value.Value?.ToString().Equals(bool.TrueString) ?? false;
        }

        /// <summary>
        /// Gets or sets the name of this property.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the return type of the compiled attribute.
        /// </summary>
        public string ReturnType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this should be compiled as formatted text.
        /// </summary>
        public bool AsFormattedText { get; set; }

        /// <summary>
        /// Gets or sets the variable names.
        /// </summary>
        public IReadOnlyCollection<string> Variables { get; set; }
    }
}
