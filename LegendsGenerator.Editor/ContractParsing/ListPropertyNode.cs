// -------------------------------------------------------------------------------------------------
// <copyright file="ListPropertyNode.cs" company="Tom Luppi">
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
    using LegendsGenerator.Editor.ChangeHistory;

    /// <summary>
    /// A definition node which is a list of other stuff.
    /// </summary>
    public class ListPropertyNode : PropertyNode
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
        /// Initializes a new instance of the <see cref="ListPropertyNode"/> class.
        /// </summary>
        /// <param name="thing">The thing this node points to.</param>
        /// <param name="info">The property info.</param>
        /// <param name="options">The options for this node.</param>
        /// <param name="readOnly">If this instance should be read only.</param>
        public ListPropertyNode(object? thing, ElementInfo info, IEnumerable<PropertyInfo> options, bool readOnly = false)
            : base(thing, info, options, readOnly)
        {
            this.info = info;
            this.underlyingObject = thing;
            this.objectType = info.PropertyType.GenericTypeArguments.First();

            this.CreateNodes();
        }

        /// <inheritdoc/>
        public override bool CanCreate => true;

        /// <inheritdoc/>
        public override bool CanDelete => this.AsList().Count == 1 && this.objectType.IsSubclassOf(typeof(BaseDefinition));

        /// <inheritdoc/>
        public override void HandleCreate(object sender, RoutedEventArgs e)
        {
            object? def;
            if (this.objectType == typeof(string))
            {
                def = BaseDefinition.UnsetString;
            }
            else
            {
                def = Activator.CreateInstance(this.objectType);
            }

            if (def == null)
            {
                throw new InvalidOperationException("A null instance was created.");
            }

            int index = this.AsList().Count;
            PropertyNode? node = this.HandleSetValue(index, def);

            // TODO: Better way to get the definition that owns this.
            Context.Instance?.SelectedDefinition?.History.AddHistoryItem(
                new ActionHistoryItem(
                    $"{this.FullName}.Items",
                    $"Item Count {this.AsList().Count - 1}",
                    $"Item Count {this.AsList().Count}",
                    () => this.HandleSetValue(index, null),
                    () => this.HandleSetValue(index, def, node)));
        }

        /// <inheritdoc/>
        public override void HandleDelete(object sender, RoutedEventArgs e)
        {
            this.AsList().Clear();
            this.Nodes.Clear();
            this.OnPropertyChanged(nameof(this.CanDelete));
        }

        /// <summary>
        /// Creats all underlying nodes based on the underlying content list.
        /// </summary>
        private void CreateNodes()
        {
            this.Nodes.Clear();
            IList list = this.AsList();

            // If there's only one element and it's a definition node, don't bother showing it in list form.
           /* if (list.Count == 1 && this.objectType.IsSubclassOf(typeof(BaseDefinition)))
            {
                foreach (PropertyNode node in DefinitionParser.ParseToNodes(this.objectType, list[0]))
                {
                    this.AddNode(node);
                }

                return;
            }*/

            for (int i = 0; i < list.Count; i++)
            {
                PropertyNode? node = this.CreateNode(i);
                if (node != null)
                {
                    this.AddNode(node);
                }
            }
        }

        /// <summary>
        /// Create a new node.
        /// </summary>
        /// <param name="i">The index.</param>
        /// <returns>The return value.</returns>
        private PropertyNode? CreateNode(int i)
        {
            ElementInfo kvpInfo = new ElementInfo(
                name: $"[{i}]",
                description: this.Description,
                propertyType: this.objectType,
                nullable: true,
                getValue: prop => this.HandleGetValue(i),
                setValue: (prop, value) => this.HandleSetValue(i, value),
                getCompiledParameters: this.info.GetCompiledParameters,
                compiled: this.info.Compiled);

            return DefinitionParser.ToNode(this.underlyingObject, kvpInfo);
        }

        /// <summary>
        /// Handles getting the value, returning null if the key does not exist.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value, or null.</returns>
        private object? HandleGetValue(int key)
        {
            IList list = this.AsList();
            if (key < list.Count)
            {
                return list[key];
            }

            return null;
        }

        /// <summary>
        /// Handles te value being set, deleting the entry if it's set to null.
        /// </summary>
        /// <param name="key">The dictionary key.</param>
        /// <param name="value">The new value.</param>
        /// <param name="node">The visual node.</param>
        /// <returns>The created property node.</returns>
        private PropertyNode? HandleSetValue(int key, object? value, PropertyNode? node = null)
        {
            if (value == null)
            {
                var matchingNode = this.Nodes[this.GetNodeIndex(key)];
                if (matchingNode != null)
                {
                    this.AsList()[key] = null;
                    this.Nodes.Remove(matchingNode);
                    this.ResetNodeNames();
                    this.OnPropertyChanged(nameof(this.CanDelete));
                }
            }
            else
            {
                if (this.AsList().Count > key && this.AsList()[key] != null)
                {
                    this.AsList()[key] = value;
                }
                else
                {
                    if (this.AsList().Count < key)
                    {
                        throw new InvalidOperationException($"Tried to insert value at index {key} but list is too small at {this.AsList().Count}");
                    }
                    else if (this.AsList().Count == key)
                    {
                        this.AsList().Add(value);
                    }
                    else
                    {
                        this.AsList()[key] = value;
                    }

                    PropertyNode? newNode = node ?? this.CreateNode(key);
                    if (newNode != null)
                    {
                        this.AddNode(newNode, this.GetNodeIndex(key));
                        this.ResetNodeNames();
                        return newNode;
                    }
                    else
                    {
                        // The key must be in the dictionary when the property node function runs, so remove it if no property node is created.
                        this.AsList().Remove(key);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Resets the node names to be correct for their place in the list.
        /// </summary>
        private void ResetNodeNames()
        {
            for (int i = 0; i < this.Nodes.Count; i++)
            {
                this.Nodes[i].SetNameBypassingHistory($"[{i}]");
            }
        }

        /// <summary>
        /// Gets the node index for the given content index.
        /// </summary>
        /// <param name="contentIndex">The content index.</param>
        /// <returns>The node index.</returns>
        private int GetNodeIndex(int contentIndex)
        {
            int realIndex = 0;
            IList list = this.AsList();
            for (int i = 0; i < contentIndex; i++)
            {
                if (list[i] != null)
                {
                    realIndex++;
                }
            }

            return realIndex;
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
