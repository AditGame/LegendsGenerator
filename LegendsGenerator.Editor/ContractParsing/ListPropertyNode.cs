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

            IList? list = this.Content as IList;
            if (list == null)
            {
                throw new InvalidOperationException($"Content type must be list, was {this.Content?.GetType().Name ?? "Null"}");
            }

            list.Add(def);
            this.CreateNodes();
            this.OnPropertyChanged(nameof(this.CanDelete));
        }

        /// <inheritdoc/>
        public override void HandleDelete(object sender, RoutedEventArgs e)
        {
            this.AsList().Clear();
            this.CreateNodes();
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
            if (list.Count == 1 && this.objectType.IsSubclassOf(typeof(BaseDefinition)))
            {
                foreach (PropertyNode node in DefinitionParser.ParseToNodes(this.objectType, list[0]))
                {
                    this.AddNode(node);
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
                    nullable: true,
                    getValue: () => this.HandleGetValue(iCopy),
                    setValue: value => this.HandleSetValue(iCopy, value),
                    getCompiledParameters: this.info.GetCompiledParameters,
                    compiled: this.info.Compiled);

                PropertyNode? node = DefinitionParser.ToNode(this.underlyingObject, kvpInfo);
                if (node != null)
                {
                    this.AddNode(node);
                }
            }
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
        private void HandleSetValue(int key, object? value)
        {
            if (value == null)
            {
                this.AsList().RemoveAt(key);
                this.CreateNodes();
                this.OnPropertyChanged(nameof(this.CanDelete));
            }
            else
            {
                this.AsList()[key] = value;
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
