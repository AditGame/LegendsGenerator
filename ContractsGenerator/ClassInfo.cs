// <copyright file="ClassInfo.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace CompiledDefinitionSourceGenerator
{
    using System.Collections.Generic;
    using System.Linq;

    using LegendsGenerator.ContractsGenerator;

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
        /// The start of method names which provide additional parameters for compiled conditions.
        /// </summary>
        public const string AdditionalParamtersForClassMethod = "AdditionalParametersForClass";

        /// <summary>
        /// The start of method names which provide reassignments of type for parameters.
        /// </summary>
        public const string TypeOfMethodPrefix = "TypeOf";

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassInfo"/> class.
        /// </summary>
        /// <param name="type">The type of the class.</param>
        public ClassInfo(INamedTypeSymbol type)
        {
            this.Accesibility = type.DeclaredAccessibility.ToString().ToLower();
            this.Namespace = type.GetNamespace();
            this.TypeName = type.Name;

            this.UsesAdditionalParametersForHoldingClass = type
                .GetAttributes("UsesAdditionalParametersForHoldingClassAttribute")
                .Any();

            this.CompiledProps = type
                .GetMembers()
                .OfType<IPropertySymbol>()
                .Where(IsAutoProperty)
                .Where(x => x.HasAttribute("CompiledAttribute"))
                .Select(field => new PropertyInfo(field))
                .ToArray();

            this.CompiledDictionaryProps = type
                .GetMembers()
                .OfType<IPropertySymbol>()
                .Where(IsAutoProperty)
                .Where(x => x.HasAttribute("CompiledDictionaryAttribute"))
                .Select(field => new PropertyInfo(field))
                .ToArray();

            this.DefinitionProps = type
                .GetMembers()
                .OfType<IPropertySymbol>()
                .Where(x => x.Type.Derives("BaseDefinition"))
                .Select(x => new DefinitionPropertyInfo(x))
                .ToArray();

            this.DefinitionArrayProps = type
                .GetMembers()
                .OfType<IPropertySymbol>()
                .Where(x =>
                {
                    var args = x.Type.GetTypeArguments();
                    return args.Count() == 1 && args.First().Derives("BaseDefinition");
                })
                .Select(x => new DefinitionPropertyInfo(x))
                .ToArray();

            this.DefinitionDictionaryProps = type
                .GetMembers()
                .OfType<IPropertySymbol>()
                .Where(x =>
                {
                    var args = x.Type.GetTypeArguments();
                    return args.Count() == 2 && args.Last().Derives("BaseDefinition");
                })
                .Select(x => new DefinitionPropertyInfo(x))
                .ToArray();

            this.AdditionalParametersForMethods = type
                .GetMembersRecursive()
                .OfType<IMethodSymbol>()
                .Where(x => x.Name.StartsWith(AdditionalParamtersMethodPrefix, System.StringComparison.Ordinal))
                .Select(x => x.Name)
                .ToArray();

            this.TypeOfMethods = type
                .GetMembersRecursive()
                .OfType<IMethodSymbol>()
                .Where(x => x.Name.StartsWith(TypeOfMethodPrefix, System.StringComparison.Ordinal))
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
        /// Gets a value indicating whether the compiled members of this class should use additional
        /// parameters from the holding class.
        /// </summary>
        public bool UsesAdditionalParametersForHoldingClass { get; }

        /// <summary>
        /// Gets the fields on this object to compile.
        /// </summary>
        public IReadOnlyCollection<PropertyInfo> CompiledProps { get; }

        /// <summary>
        /// Gets the fields on this object to compile as dictionaries.
        /// </summary>
        public IReadOnlyCollection<PropertyInfo> CompiledDictionaryProps { get; }

        /// <summary>
        /// Gets properties which are also definitions.
        /// </summary>
        public IReadOnlyCollection<DefinitionPropertyInfo> DefinitionProps { get; }

        /// <summary>
        /// Gets properties which are also definitions arrays.
        /// </summary>
        public IReadOnlyCollection<DefinitionPropertyInfo> DefinitionArrayProps { get; }

        /// <summary>
        /// Gets properties which are also definitions dictionaries.
        /// </summary>
        public IReadOnlyCollection<DefinitionPropertyInfo> DefinitionDictionaryProps { get; }

        /// <summary>
        /// Gets the methods which modify the property list before compiling.
        /// </summary>
        public IReadOnlyCollection<string> AdditionalParametersForMethods { get; }

        /// <summary>
        /// Gets the methods which modify the types of parameters.
        /// </summary>
        public IReadOnlyCollection<string> TypeOfMethods { get; }

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
