// -------------------------------------------------------------------------------------------------
// <copyright file="SectionDefinitionNode.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor.ContractParsing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    using LegendsGenerator.Contracts.Definitions;

    /// <summary>
    /// A node which is jsut the start of a new section.
    /// </summary>
    public class SectionDefinitionNode : DefinitionNode
    {
        /// <summary>
        /// The type to create when added.
        /// </summary>
        private Type type;

        private Action<object>? onCreate;

        private Action? onDelete;

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionDefinitionNode"/> class.
        /// </summary>
        /// <param name="description">The description of this node.</param>
        /// <param name="name">The name of this node.</param>
        public SectionDefinitionNode(
            string description,
            string name,
            Type definitionType,
            object? definition,
            bool nullable = false,
            Action<object>? onCreate = null,
            Action? onDelete = null)
            : base(description, nullable)
        {
            this.Name = name;
            this.type = definitionType;
            this.onCreate = onCreate;
            this.onDelete = onDelete;

            if (definition != null)
            {
                this.AddInnerDefinition(definitionType, definition);
            }
        }

        /// <inheritdoc/>
        public override string Name { get; }

        /// <inheritdoc/>
        public override UIElement GetContentElement()
        {
            if (this.SubNodes.Any())
            {
                if (this.Nullable)
                {
                    Button button = new Button()
                    {
                        Content = "-",
                    };

                    button.Click += HandleCreate;

                    return button;
                }
            }
            else
            {
                Button button = new Button()
                {
                    Content = "+",
                };

                button.Click += HandleCreate;

                return button;
            }

            return new TextBlock();
        }

        private void HandleCreate(object sender, RoutedEventArgs e)
        {
            object def = Activator.CreateInstance(this.type);
            this.onCreate?.Invoke(def);
            this.AddInnerDefinition(this.type, def);
        }

        private void HandleDelete(object sender, RoutedEventArgs e)
        {
            this.onDelete?.Invoke();
            this.AddInnerDefinition(this.type, null);
        }

        private void AddInnerDefinition(Type type, object? definition)
        {
            this.SubNodes.Clear();

            if (definition != null)
            {
                this.SubNodes.AddRange(DefinitionParser.ParseToNodes(type, definition));
            }
        }
    }
}
