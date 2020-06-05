// -------------------------------------------------------------------------------------------------
// <copyright file="ListDefinitionNode.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor.ContractParsing
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using LegendsGenerator.Contracts.Definitions;

    /// <summary>
    /// A definition node which is a list of other stuff.
    /// </summary>
    public class ListDefinitionNode : DefinitionNode, ICreatable
    {
        /// <summary>
        /// The underlying element info.
        /// </summary>
        private ElementInfo info;

        /// <summary>
        /// The underlying object.
        /// </summary>
        private object? underlyingObject;

        /// <summary>
        /// The object type represented in this list.
        /// </summary>
        private Type objectType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListDefinitionNode"/> class.
        /// </summary>
        /// <param name="thing">The thing this node points to.</param>
        /// <param name="info">The property info.</param>
        /// <param name="options">The options for this node.</param>
        /// <param name="readOnly">If this instance should be read only.</param>
        public ListDefinitionNode(object? thing, ElementInfo info, IEnumerable<PropertyInfo> options, bool readOnly = false)
            : base(thing, info, options, readOnly)
        {
            this.info = info;
            this.underlyingObject = thing;
            this.objectType = info.PropertyType.GenericTypeArguments.First();

            this.CreateNodes();
        }

        /// <inheritdoc/>
        public void HandleCreate(object sender, RoutedEventArgs e)
        {
            object? def;
            if (this.objectType == typeof(string))
            {
                def = string.Empty;
            }
            else
            {
                def = Activator.CreateInstance(this.objectType);
            }

            if (def == null)
            {
                throw new InvalidOperationException("A null instance was created.");
            }

            IList? list = this.Content as IList;
            if (list == null)
            {
                throw new InvalidOperationException($"Content type must be list, was {this.Content?.GetType().Name ?? "Null"}");
            }

            list.Add(def);
            this.CreateNodes();
        }

        /// <summary>
        /// Creats all underlying nodes based on the underlying content list.
        /// </summary>
        private void CreateNodes()
        {
            this.Nodes.Clear();
            IList list = this.AsList();

            // If there's only one element and it's a definition node, don't bother showing it in list form.
            if (list.Count == 1 && this.objectType.IsSubclassOf(typeof(BaseDefinition)))
            {
                foreach (DefinitionNode node in DefinitionParser.ParseToNodes(this.objectType, list[0]))
                {
                    this.Nodes.Add(node);
                }

                return;
            }

            for (int i = 0; i < list.Count; i++)
            {
                int iCopy = i;
                object? value = list[i];

                ElementInfo kvpInfo = new ElementInfo(
                    name: $"[{i}]",
                    description: this.Description,
                    propertyType: this.objectType,
                    nullable: false,
                    getValue: () => this.AsList()[iCopy],
                    setValue: value => this.AsList()[iCopy] = value,
                    getCompiledParameters: this.info.GetCompiledParameters,
                    compiled: this.info.Compiled);

                DefinitionNode? node = DefinitionParser.ToNode(this.underlyingObject, kvpInfo);
                if (node != null)
                {
                    this.Nodes.Add(node);
                }
            }
        }

        /// <summary>
        /// Gets the content as a IList.
        /// </summary>
        /// <returns>The IList.</returns>
        private IList AsList()
        {
            IList? list = this.Content as IList;
            if (list == null)
            {
                throw new InvalidOperationException($"Content type must be list, was {this.Content?.GetType().Name ?? "Null"}");
            }

            return list;
        }
    }
}
