// <copyright file="PropertyInfo.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace CompiledDefinitionSourceGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
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
            var attributeNamedArguments = property
                .GetAttributes("CompiledAttribute", "CompiledDictionaryAttribute")
                .SelectMany(a => a.NamedArguments);

            this.ReturnType = attributeConstructorArguments
                .First(x => x.Type?.Name == "Type").Value?.ToString() ?? "void";

            Trace.WriteLine(string.Join(", ", property.GetAttributes("CompiledVariableAttribute").Select(a => a.ConstructorArguments.Select(x => x.Type))));

            this.Variables = property.GetAttributes("CompiledVariableAttribute")
                .Select(a => (
                    a.ConstructorArguments.First(x => x.Type?.Name.Equals("String", StringComparison.OrdinalIgnoreCase) == true).Value?.ToString() ?? "void",
                    a.ConstructorArguments.First(x => x.Type?.Name.Equals("Type", StringComparison.OrdinalIgnoreCase) == true).Value?.ToString() ?? "void")).ToList();

            this.AsFormattedText = attributeNamedArguments
                .FirstOrDefault(a => a.Key.Equals("AsFormattedText"))
                .Value.Value?.ToString().Equals(bool.TrueString) ?? false;

            this.Protected = attributeNamedArguments
                .FirstOrDefault(a => a.Key.Equals("Protected"))
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
        /// Gets or sets a value indicating whether this should be generated as Protected, not Public.
        /// </summary>
        public bool Protected { get; set; }

        /// <summary>
        /// Gets or sets the variable names.
        /// </summary>
        public IReadOnlyCollection<(string Name, string Type)> Variables { get; set; }
    }
}
