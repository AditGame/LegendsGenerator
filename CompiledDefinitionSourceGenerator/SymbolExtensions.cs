// <copyright file="SymbolExtensions.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace CompiledDefinitionSourceGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;

    /// <summary>
    /// Extensions for the symbol class.
    /// </summary>
    public static class SymbolExtensions
    {
        /// <summary>
        /// Gets attributes matching the type name.
        /// </summary>
        /// <param name="type">The type to get attributes from.</param>
        /// <param name="typeName">The type name.</param>
        /// <returns>The matching attributes.</returns>
        public static IEnumerable<AttributeData> GetAttributes(this ISymbol type, string typeName)
        {
            return type
                .GetAttributes()
                .Where(a => a.AttributeClass?.GetFullName() == typeName);
        }

        /// <summary>
        /// Checks if the specific symbol has the specified attribute.
        /// </summary>
        /// <param name="type">The symbol to check.</param>
        /// <param name="typeName">The name of the attribute.</param>
        /// <returns>True if the attribute exists on the symbol, false otherwise.</returns>
        public static bool HasAttribute(this ISymbol type, string typeName)
        {
            return type
                .GetAttributes(typeName)
                .Any();
        }

        /// <summary>
        /// Gets the attribute from the type if it exists.
        /// </summary>
        /// <param name="type">The type to get the attribute from.</param>
        /// <param name="typeName">The name of the type.</param>
        /// <returns>The attribute if it exists.</returns>
        public static AttributeData? GetAttribute(this ISymbol type, string typeName)
        {
            return type
                .GetAttributes(typeName)
                .FirstOrDefault();
        }

        /// <summary>
        /// Gets the full name of the symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <returns>The full name of the symbol.</returns>
        public static string GetFullName(this ITypeSymbol symbol)
        {
            return $"{symbol.GetNamespace()}.{symbol.Name}";
        }

        /// <summary>
        /// Gets the namespace of the symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <returns>The namespace of the symbol.</returns>
        public static string GetNamespace(this ISymbol symbol)
        {
            var namespaces = new List<INamespaceSymbol>();

            for (
                var currentNamespace = symbol.ContainingNamespace;
                !currentNamespace.IsGlobalNamespace;
                currentNamespace = currentNamespace.ContainingNamespace)
            {
                namespaces.Add(currentNamespace);
            }

            return string.Join(".", namespaces.Select(n => n.Name).Reverse());
        }
    }
}
