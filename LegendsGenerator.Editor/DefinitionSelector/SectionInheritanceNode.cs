// -------------------------------------------------------------------------------------------------
// <copyright file="SectionInheritanceNode.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor.DefinitionSelector
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using LegendsGenerator.Contracts.Definitions;

    /// <summary>
    /// An inheritance node which represents a section.
    /// </summary>
    public class SectionInheritanceNode : InheritanceNode
    {
        /// <summary>
        /// The type to create under this node.
        /// </summary>
        private readonly Type? type;

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionInheritanceNode"/> class.
        /// </summary>
        /// <param name="type">The type to create when "Create" is called.</param>
        /// <param name="name">The name of this node.</param>
        /// <param name="nodes">The underlying nodes.</param>
        public SectionInheritanceNode(Type? type, string name, IEnumerable<InheritanceNode> nodes)
            : base(name, nodes)
        {
            this.type = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionInheritanceNode"/> class.
        /// </summary>
        /// <param name="type">The type to create when "Create" is called.</param>
        /// <param name="name">The name of this node.</param>
        /// <param name="definitions">The underlying definitions.</param>
        /// <param name="inheritanceList">The inheritance list.</param>
        public SectionInheritanceNode(Type? type, string name, IEnumerable<Definition> definitions, ILookup<string?, Definition>? inheritanceList)
            : base(name, definitions, inheritanceList)
        {
            this.type = type;
        }

        /// <summary>
        /// Gets a value indicating whether this node can be created.
        /// </summary>
        public override bool CanCreate => this.type != null;

        /// <inheritdoc/>
        public override void HandleCreate(object sender, RoutedEventArgs e)
        {
            if (this.type == null)
            {
                throw new InvalidOperationException("Cannot create on this instance as Type was not specified.");
            }

            if (Activator.CreateInstance(this.type) is not BaseDefinition newObj)
            {
                throw new InvalidOperationException("Activator returned a null instance.");
            }

            if (Context.Instance?.SelectedDefinition?.BaseDefinition != null)
            {
                if (newObj is ITopLevelDefinition topLevel &&
                    Context.Instance.SelectedDefinition.BaseDefinition is ITopLevelDefinition currentToplevel)
                {
                    topLevel.SourceFile = currentToplevel.SourceFile;
                }
            }

            Definition newDef = new Definition(newObj);
            Context.Instance?.AddDefinition(newDef);
            DefinitionInheritanceNode node = new DefinitionInheritanceNode(GetHeader(newObj), newDef, null);
            this.AddNode(node);
            Context.FixInheritanceNode(node);

            if (Context.Instance != null)
            {
                Context.Instance.SelectedDefinition = newDef;
                Context.Instance.SelectedNode = null;
            }
        }
    }
}
