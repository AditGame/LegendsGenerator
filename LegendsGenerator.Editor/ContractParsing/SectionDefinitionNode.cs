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
    using System.Reflection;
    using System.Windows;

    /// <summary>
    /// A node which is jsut the start of a new section.
    /// </summary>
    public class SectionDefinitionNode : DefinitionNode, ICreatable, IDeletable
    {
        /// <summary>
        /// The type to create when added.
        /// </summary>
        private Type type;

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionDefinitionNode"/> class.
        /// </summary>
        /// <param name="thing">The thing this node points to.</param>
        /// <param name="property">The property info.</param>
        /// <param name="options">The options for this node.</param>
        /// <param name="readOnly">If this instance should be read only.</param>
        public SectionDefinitionNode(object? thing, ElementInfo property, IEnumerable<PropertyInfo> options, bool readOnly = false)
            : base(thing, property, options, readOnly)
        {
            this.type = property.PropertyType;

            if (thing != null)
            {
                this.AddInnerDefinition(this.type, property.GetValue());
            }
        }

        /// <summary>
        /// Gets the visibility of the create button.
        /// </summary>
        public Visibility CanCreate => !this.Nodes.Any() ? Visibility.Visible : Visibility.Collapsed;

        /// <summary>
        /// Gets the visibility of the delete button.
        /// </summary>
        public Visibility CanDelete => this.Nodes.Any() && this.Nullable ? Visibility.Visible : Visibility.Collapsed;

        /// <inheritdoc/>
        public void HandleCreate(object sender, RoutedEventArgs e)
        {
            object? def = Activator.CreateInstance(this.type);
            this.Content = def;
            this.AddInnerDefinition(this.type, def);
            this.OnPropertyChanged(nameof(this.CanCreate));
            this.OnPropertyChanged(nameof(this.CanDelete));
        }

        /// <inheritdoc/>
        public void HandleDelete(object sender, RoutedEventArgs e)
        {
            this.Content = null;
            this.AddInnerDefinition(this.type, null);
            this.OnPropertyChanged(nameof(this.CanCreate));
            this.OnPropertyChanged(nameof(this.CanDelete));
        }

        /// <summary>
        /// Adds the underlying nodes.
        /// </summary>
        /// <param name="type">The type to add.</param>
        /// <param name="definition">The object.</param>
        private void AddInnerDefinition(Type type, object? definition)
        {
            this.Nodes.Clear();

            if (definition != null)
            {
                foreach (var element in DefinitionParser.ParseToNodes(type, definition))
                {
                    this.Nodes.Add(element);
                }
            }
        }
    }
}
