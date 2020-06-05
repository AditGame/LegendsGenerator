// -------------------------------------------------------------------------------------------------
// <copyright file="DictionaryDefinitionNode.cs" company="Tom Luppi">
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

    /// <summary>
    /// A node which is a dictionary.
    /// </summary>
    public class DictionaryDefinitionNode : DefinitionNode
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
        /// Initializes a new instance of the <see cref="DictionaryDefinitionNode"/> class.
        /// </summary>
        /// <param name="thing">The thing this node points to.</param>
        /// <param name="info">The property info.</param>
        /// <param name="options">The options for this node.</param>
        /// <param name="readOnly">If this instance should be read only.</param>
        public DictionaryDefinitionNode(object? thing, ElementInfo info, IEnumerable<PropertyInfo> options, bool readOnly = false)
            : base(thing, info, options, readOnly)
        {
            this.valueType = info.PropertyType.GetGenericArguments().Last();
            this.info = info;
            this.thing = thing;

            this.GenerateNodes();
        }

        /// <summary>
        /// Generates all lower nodes based on content dictionary.
        /// </summary>
        private void GenerateNodes()
        {
            this.Nodes.Clear();

            IDictionary dictionary = this.AsDictionary();

            foreach (DictionaryEntry? kvp in dictionary)
            {
                if (kvp == null)
                {
                    continue;
                }

                ElementInfo kvpInfo = new ElementInfo(
                    name: kvp.Value.Key as string ?? "UNKNOWN",
                    description: this.info.Description,
                    propertyType: this.valueType,
                    nullable: false,
                    getValue: () => this.AsDictionary()[kvp.Value.Key],
                    setValue: value => this.AsDictionary()[kvp.Value.Key] = value,
                    getCompiledParameters: this.info.GetCompiledParameters,
                    compiled: this.info.Compiled)
                {
                    ChangeName = newName => this.ChangeName((kvp.Value.Key as string)!, newName),
                };

                DefinitionNode? node = DefinitionParser.ToNode(this.thing, kvpInfo);
                if (node != null)
                {
                    this.Nodes.Add(node);
                }
            }
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
            object? entry = dictionary[oldName];
            dictionary.Remove(oldName);
            dictionary[newName] = entry;

            this.GenerateNodes();
        }
    }
}
