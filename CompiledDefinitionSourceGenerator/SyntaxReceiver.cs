// <copyright file="SyntaxReceiver.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace CompiledDefinitionSourceGenerator
{
    using System;
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Text;

    /// <summary>
    /// Recieves and sorts the syntax in the assembly.
    /// </summary>
    internal class SyntaxReceiver : ISyntaxReceiver
    {
        /// <summary>
        /// Gets the list of classes in the assembly.
        /// </summary>
        public List<ClassDeclarationSyntax> Classes { get; } = new List<ClassDeclarationSyntax>();

        /// <inheritdoc/>
        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax classSyntax && classSyntax.AttributeLists.Count > 0)
            {
                this.Classes.Add(classSyntax);
            }
        }
    }
}
