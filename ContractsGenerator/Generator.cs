// <copyright file="Generator.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace CompiledDefinitionSourceGenerator
{
    using System;
    using System.IO;
    using System.Text;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Text;

    /// <summary>
    /// Generates the source code.
    /// </summary>
    [Generator]
    public class Generator : ISourceGenerator
    {
        /// <inheritdoc/>
        public void Initialize(InitializationContext context)
        {
            // Debugger.Launch();
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        /// <inheritdoc/>
        public void Execute(SourceGeneratorContext context)
        {
            SyntaxReceiver receiver =
                context.SyntaxReceiver as SyntaxReceiver ??
                throw new ApplicationException("Unexpected syntax receiver registered.");
            var compilation = context.Compilation;

            foreach (var classSyntax in receiver.Classes)
            {
                SemanticModel? semanticModel = compilation.GetSemanticModel(classSyntax.SyntaxTree);
                ISymbol? type = semanticModel.GetDeclaredSymbol(classSyntax);

                if (type is INamedTypeSymbol symbol && symbol.Derives("BaseDefinition"))
                {
                    ClassInfo classInfo = new ClassInfo(symbol);
                    string code = CompiledClassFactory.Generate(classInfo);
                    context.AddSource($"{type.Name}.Compiled.Generated.cs", SourceText.From(code, Encoding.UTF8));
                    File.WriteAllText(Path.Combine(Path.GetTempPath(), $"{type.Name}.Compiled.Generated.cs"), code);
                }
            }
        }
    }
}
