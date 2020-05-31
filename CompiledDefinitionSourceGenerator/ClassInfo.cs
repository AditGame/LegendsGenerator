// <copyright file="ClassInfo.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace CompiledDefinitionSourceGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Information about a class.
    /// </summary>
    internal class ClassInfo
    {
        /// <summary>
        /// The start of method names which provide additional parameters for compiled conditions.
        /// </summary>
        public const string AdditionalParamtersMethodPrefix = "AdditionalParametersFor";

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassInfo"/> class.
        /// </summary>
        /// <param name="type">The type of the class.</param>
        public ClassInfo(INamedTypeSymbol type)
        {
            this.Accesibility = type.DeclaredAccessibility.ToString().ToLower();
            this.Namespace = type.GetNamespace();
            this.TypeName = type.Name;

            this.CompiledProps = type
                .GetMembers()
                .OfType<IPropertySymbol>()
                .Where(IsAutoProperty)
                .Where(x => x.HasAttribute("CompiledAttribute"))
                .Select(field => new PropertyInfo(field))
                .ToArray();

            this.AllProperties = type
                .GetMembers()
                .OfType<IPropertySymbol>()
                .Select(x => x.Name)
                .ToArray();

            this.AdditionalParametersForMethods = type
                .GetMembersRecursive()
                .OfType<IMethodSymbol>()
                .Where(x => x.Name.StartsWith(AdditionalParamtersMethodPrefix))
                .Select(x => x.Name)
                .ToArray();
        }

        /// <summary>
        /// Gets or sets the accesibility of this class.
        /// </summary>
        public string Accesibility { get; set; }

        /// <summary>
        /// Gets the namepspace of this class.
        /// </summary>
        public string Namespace { get; }

        /// <summary>
        /// Gets the typename.
        /// </summary>
        public string TypeName { get; }

        /// <summary>
        /// Gets the fields on this object to compile.
        /// </summary>
        public IReadOnlyCollection<PropertyInfo> CompiledProps { get; }

        /// <summary>
        /// Gets all properties on this object.
        /// </summary>
        public IReadOnlyCollection<string> AllProperties { get; }

        /// <summary>
        /// Gets the methods which modify the property list before compiling.
        /// </summary>
        public IReadOnlyCollection<string> AdditionalParametersForMethods { get; }

        /// <summary>
        /// Checks if the provided property is an auto property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>True if the property is auto, false otherwise.</returns>
        private static bool IsAutoProperty(IPropertySymbol property)
        {
            return property.DeclaringSyntaxReferences.Select(r => r.GetSyntax()).OfType<PropertyDeclarationSyntax>().Any(p => p.AccessorList?.Accessors.Any(ancestor => ancestor.IsKind(SyntaxKind.GetAccessorDeclaration) && ancestor.Body == null) == true);
        }
    }
}
