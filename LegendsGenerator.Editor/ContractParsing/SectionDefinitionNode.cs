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
    using System.Windows.Controls;

    using LegendsGenerator.Contracts.Definitions;

    /// <summary>
    /// A node which is jsut the start of a new section.
    /// </summary>
    public class SectionDefinitionNode : DefinitionNode, ICreatable, IDeletable
    {
        /// <summary>
        /// The type to create when added.
        /// </summary>
        private Type type;

        public SectionDefinitionNode(object? thing, ElementInfo property, IEnumerable<PropertyInfo> options, bool readOnly = false)
            : base(thing, property, options, readOnly)
        {
            this.type = property.PropertyType;

            if (thing != null)
            {
                this.AddInnerDefinition(this.type, property.GetMethod());
            }
        }

        public Visibility CanCreate => !this.Nodes.Any() ? Visibility.Visible : Visibility.Collapsed;

        public Visibility CanDelete => this.Nodes.Any() && this.Nullable ? Visibility.Visible : Visibility.Collapsed;

        public void HandleCreate(object sender, RoutedEventArgs e)
        {
            object def = Activator.CreateInstance(this.type);
            this.Content = def;
            this.AddInnerDefinition(this.type, def);
            this.OnPropertyChanged(nameof(this.CanCreate));
            this.OnPropertyChanged(nameof(this.CanDelete));
        }

        public void HandleDelete(object sender, RoutedEventArgs e)
        {
            this.Content = null;
            this.AddInnerDefinition(this.type, null);
            this.OnPropertyChanged(nameof(this.CanCreate));
            this.OnPropertyChanged(nameof(this.CanDelete));
        }

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
