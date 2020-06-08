// -------------------------------------------------------------------------------------------------
// <copyright file="DefinitionInheritanceNode.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor.DefinitionSelector
{
    using System;
    using System.Linq;
    using System.Windows;

    /// <summary>
    /// An inheritance node which represents a definition.
    /// </summary>
    public class DefinitionInheritanceNode : InheritanceNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefinitionInheritanceNode"/> class.
        /// </summary>
        /// <param name="name">The name of this node.</param>
        /// <param name="definition">The definition of this node.</param>
        /// <param name="inheritanceList">The inheritance list.</param>
        public DefinitionInheritanceNode(string name, Definition? definition, ILookup<string?, Definition>? inheritanceList)
            : base(name, definition, inheritanceList)
        {
        }

        /// <summary>
        /// Gets a value indicating whether this node can be deleted.
        /// </summary>
        public override bool CanDelete => true;

        /// <inheritdoc/>
        public override void HandleDelete(object sender, RoutedEventArgs e)
        {
            if (this.Definition == null)
            {
                throw new InvalidOperationException("Definition is null; can't be deleted.");
            }

            // Remove itself from the parents node.
            this.Upstream?.Nodes.Remove(this);

            // Remove itself from the list of definitions
            Context.Instance?.RemoveDefinition(this.Definition);
        }
    }
}
