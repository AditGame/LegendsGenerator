// -------------------------------------------------------------------------------------------------
// <copyright file="CompiledDefinitionNode.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor.ContractParsing
{
    using LegendsGenerator.Contracts.Compiler;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// A node of compiled text.
    /// </summary>
    public class CompiledDefinitionNode : DefinitionNode
    {
        /// <summary>
        /// Parameters which are available to all functions.
        /// </summary>
        private static readonly List<string> StaticParameters = new List<string>
        {
            "Rand",
        };

        /// <summary>
        /// If this compiled func is formatted text.
        /// </summary>
        private bool asFormattedText;

        /// <summary>
        /// A function which gets if this is compiled text.
        /// </summary>
        private Func<bool> isComplexFunc;

        /// <summary>
        /// A function which gets the parameters for the compiled conditions.
        /// </summary>
        private Func<IList<string>> getParametersFunc;

        public CompiledDefinitionNode(
            CompiledAttribute attribute,
            object? thing,
            ElementInfo property,
            IEnumerable<PropertyInfo> options,
            bool readOnly = false)
            : base(thing, property, options, readOnly)
        {
            this.asFormattedText = attribute.AsFormattedText;
            this.ReturnType = attribute.ReturnType;

            this.getParametersFunc = property.GetParametersMethod;

            DefinitionNode? isComplexNode = this.Options.FirstOrDefault(o => o.Name.Equals("IsComplex"));
            if (isComplexNode != null)
            {
                this.isComplexFunc = () => (bool)(isComplexNode.Content ?? false);

                // If the isComplex option changes, we need to notify that dependant properties may have changed.
                isComplexNode.PropertyChanged += (source, name) =>
                {
                    this.OnPropertyChanged(nameof(this.IsComplex));
                    this.OnPropertyChanged(nameof(this.InlineEnabled));
                    this.OnPropertyChanged(nameof(this.Language));
                };
            }
            else
            {
                this.isComplexFunc = () => false;
            }
        }

        /// <summary>
        /// Gets the function return type.
        /// </summary>
        public Type ReturnType { get; }

        /// <summary>
        /// Gets the parameters of this method.
        /// </summary>
        public IList<string> Parameters
        {
            get
            {
                List<string> param = new List<string>();
                param.AddRange(StaticParameters);
                param.AddRange(this.getParametersFunc());
                return param;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this func is complex.
        /// </summary>
        public bool IsComplex => this.isComplexFunc();

        /// <summary>
        /// Gets a value indicating whether inlining should be enabled.
        /// </summary>
        public bool InlineEnabled => !this.IsComplex;

        /// <summary>
        /// Gets the language of this code.
        /// </summary>
        public string Language => (this.asFormattedText && !this.IsComplex) ? "csharp-quotes" : "csharp";
    }
}
