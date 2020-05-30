// <copyright file="Generator.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace CompiledDefinitionSourceGenerator
{
    using System;
    using System.Text;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Text;

    /// <summary>
    /// Generates the source code.
    /// </summary>
    [Generator]
    public class Generator : ISourceGenerator
    {
        /// <summary>
        /// The attribute code to be injected in the project.
        /// </summary>
        private const string Attributes = @"
namespace CompiledDefinitionSourceGenerator {
    using System;

    /// <summary>
    /// Indicates the proeprty should be compiled.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CompiledAttribute : Attribute
    {
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

    /// <summary>
    /// Attribute which indicating the class needs compiling.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class NeedsCompilingAttribute : Attribute
    {
    }
}";

        /// <inheritdoc/>
        public void Initialize(InitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        /// <inheritdoc/>
        public void Execute(SourceGeneratorContext context)
        {
            SyntaxReceiver receiver =
                context.SyntaxReceiver as SyntaxReceiver ??
                throw new ApplicationException("Unexpected syntax receiver registered.");
            var compilation = context.Compilation;

            context.AddSource($"CompiledAttributes.Generated.cs", SourceText.From(Attributes, Encoding.UTF8));

            foreach (var classSyntax in receiver.Classes)
            {
                SemanticModel? semanticModel = compilation.GetSemanticModel(classSyntax.SyntaxTree);
                ISymbol? type = semanticModel.GetDeclaredSymbol(classSyntax);

                if (type.HasAttribute(typeof(NeedsCompilingAttribute).FullName!))
                {
                    ClassInfo classInfo = new ClassInfo(type.ContainingType);
                    string code = CompiledClassFactory.Generate(classInfo);
                    context.AddSource($"{type.Name}Compiled.Generated.cs", SourceText.From(code, Encoding.UTF8));
                }
            }
        }
    }
}
