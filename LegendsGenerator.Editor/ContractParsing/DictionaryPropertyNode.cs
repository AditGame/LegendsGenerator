// -------------------------------------------------------------------------------------------------
// <copyright file="DictionaryPropertyNode.cs" company="Tom Luppi">
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
    /// A node which is a dictionary.
    /// </summary>
    public class DictionaryPropertyNode : PropertyNode
    {
        /// <summary>
        /// The type of the dictionary value.
        /// </summary>
        private Type valueType;

        /// <summary>
        /// The element info.
        /// </summary>
        private ElementInfo info;

        /// <summary>
        /// The thing.
        /// </summary>
        private object? thing;

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryPropertyNode"/> class.
        /// </summary>
        /// <param name="thing">The thing this node points to.</param>
        /// <param name="info">The property info.</param>
        /// <param name="options">The options for this node.</param>
        /// <param name="readOnly">If this instance should be read only.</param>
        public DictionaryPropertyNode(object? thing, ElementInfo info, IEnumerable<PropertyInfo> options, bool readOnly = false)
            : base(thing, info, options, readOnly)
        {
            this.valueType = info.PropertyType.GetGenericArguments().Last();
            this.info = info;
            this.thing = thing;

            this.Nodes.Clear();

            foreach (DictionaryEntry? kvp in this.AsDictionary())
            {
                if (kvp == null)
                {
                    continue;
                }

                PropertyNode? node = this.CreateNode(kvp.Value.Key);
                if (node != null)
                {
                    this.AddNode(node);
                }
            }
        }

        /// <inheritdoc/>
        public override bool CanCreate => true;

        /// <inheritdoc/>
        public override void HandleCreate(object sender, RoutedEventArgs e)
        {
            // validate that this key isn't a duplicate.
            foreach (var key in this.AsDictionary().Keys)
            {
                if (BaseDefinition.UnsetString.Equals(key))
                {
                    Console.WriteLine($"You must change the name of the {BaseDefinition.UnsetString} before adding another node.");
                    return;
                }
            }

            object? def;
            if (this.valueType == typeof(string))
            {
                def = BaseDefinition.UnsetString;
            }
            else
            {
                def = Activator.CreateInstance(this.valueType);
            }

            if (def == null)
            {
                throw new InvalidOperationException("A null instance was created.");
            }

            PropertyNode? node = this.HandleSetValue(BaseDefinition.UnsetString, def);

            // TODO: Better way to get the definition that owns this.
            Context.Instance?.SelectedDefinition?.History.AddHistoryItem(
                new ActionHistoryItem(
                    $"{this.FullName}.Items",
                    $"Item Count {this.AsDictionary().Count - 1}",
                    $"Item Count {this.AsDictionary().Count}",
                    () => this.HandleSetValue(BaseDefinition.UnsetString, null),
                    () => this.HandleSetValue(BaseDefinition.UnsetString, def, node)));
        }

        /// <summary>
        /// Creates a new node.
        /// </summary>
        /// <param name="key">The node key.</param>
        /// <returns>The node.</returns>
        private PropertyNode? CreateNode(object key)
        {
            ElementInfo kvpInfo = new ElementInfo(
                name: key as string ?? "UNKNOWN",
                description: this.info.Description,
                propertyType: this.valueType,
                nullable: true, // Set nullable to true, if the value is set to null than the element will be deleted.
                getValue: prop => this.AsDictionary()[prop.Name],
                setValue: (prop, value) => this.HandleSetValue(prop.Name, value),
                getCompiledParameters: this.info.GetCompiledParameters,
                compiled: this.info.Compiled,
                hiddenInEditorCondition: null)
            {
                ChangeName = (prop, newName) => this.ChangeName(prop.Name, newName),
                NameCreatesVariableName = true, // This is currently always true, should plumb in correctly with attribute for auto-magic.
            };

            return DefinitionParser.ToNode(this.thing, kvpInfo);
        }

        /// <summary>
        /// Gets the contents as a dictionary.
        /// </summary>
        /// <exception cref="InvalidOperationException">The contents is not a dictionary.</exception>
        /// <returns>The contents, as dictionary.</returns>
        private IDictionary AsDictionary()
        {
            IDictionary? dictionary = this.Content as IDictionary;

            if (dictionary == null)
            {
                throw new InvalidOperationException($"Content type must be Dictionary, was {this.Content?.GetType().Name ?? "Null"}");
            }

            return dictionary;
        }

        /// <summary>
        /// Handles the value being set, deleting the entry if it's set to null and creating if it's a new key.
        /// </summary>
        /// <param name="key">The dictionary key.</param>
        /// <param name="value">The new value.</param>
        /// <param name="node">the node if already created.</param>
        /// <returns>The node, if one was created.</returns>
        private PropertyNode? HandleSetValue(object key, object? value, PropertyNode? node = null)
        {
            if (value == null)
            {
                PropertyNode? matchingNode = this.Nodes.FirstOrDefault(n => n.Name.Equals(key));
                if (matchingNode != null)
                {
                    this.AsDictionary().Remove(key);
                    this.Nodes.Remove(matchingNode);
                }
            }
            else
            {
                if (this.AsDictionary().Contains(key))
                {
                    this.AsDictionary()[key] = value;
                }
                else
                {
                    this.AsDictionary()[key] = value;
                    PropertyNode? newNode = node ?? this.CreateNode(key);
                    if (newNode != null)
                    {
                        this.AddNode(newNode);
                        return newNode;
                    }
                    else
                    {
                        // The key must be in the dictionary when the property node function runs, so remove it if no property node is created.
                        this.AsDictionary().Remove(key);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Handles te change name scenario.
        /// </summary>
        /// <param name="oldName">The old name.</param>
        /// <param name="newName">The new name.</param>
        private void ChangeName(string oldName, string newName)
        {
            if (oldName == newName)
            {
                return;
            }

            IDictionary dictionary = this.AsDictionary();

            // validate that this key isn't a duplicate.
            foreach (var key in dictionary.Keys)
            {
                if (newName.Equals(key))
                {
                    Console.WriteLine($"A key with name {newName} already exists in the dictionary.");
                    return;
                }
            }

            object? entry = dictionary[oldName];
            dictionary.Remove(oldName);
            dictionary[newName] = entry;
        }
    }
}
