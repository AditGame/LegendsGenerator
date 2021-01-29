// -------------------------------------------------------------------------------------------------
// <copyright file="DefinitionPropertyNode.cs" company="Tom Luppi">
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
    public class DefinitionPropertyNode : PropertyNode
    {
        /// <summary>
        /// The type to create when added.
        /// </summary>
        private readonly Type type;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefinitionPropertyNode"/> class.
        /// </summary>
        /// <param name="thing">The thing this node points to.</param>
        /// <param name="property">The property info.</param>
        /// <param name="options">The options for this node.</param>
        /// <param name="readOnly">If this instance should be read only.</param>
        public DefinitionPropertyNode(object? thing, ElementInfo property, IEnumerable<PropertyInfo> options, bool readOnly = false)
            : base(thing, property, options, readOnly)
        {
            this.type = property.PropertyType;

            if (thing != null)
            {
                this.AddInnerDefinition(this.type, property.GetValue(this));
            }

            this.PropertyChanged += this.HandlePropertyChanged;
        }

        /// <summary>
        /// Handles property changed events.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void HandlePropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName?.Equals(nameof(this.Content), StringComparison.Ordinal) == true)
            {
                this.AddInnerDefinition(this.ContentsType, this.Content);
            }
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
                    this.AddNode(element);
                }
            }
        }
    }
}
