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
        /// Initializes a new instance of the <see cref="DictionaryDefinitionNode"/> class.
        /// </summary>
        /// <param name="thing">The thing this node points to.</param>
        /// <param name="info">The property info.</param>
        /// <param name="options">The options for this node.</param>
        /// <param name="readOnly">If this instance should be read only.</param>
        public DictionaryDefinitionNode(object? thing, ElementInfo info, IEnumerable<PropertyInfo> options, bool readOnly = false)
            : base(thing, info, options, readOnly)
        {
            this.Nodes.Clear();

            this.valueType = info.PropertyType.GetGenericArguments().Last();

            IDictionary? dictionary = info.GetValue() as IDictionary;

            if (dictionary == null)
            {
                return;
            }

            foreach (DictionaryEntry kvp in dictionary.OfType<DictionaryEntry>())
            {
                ElementInfo kvpInfo = new ElementInfo(
                    name: kvp.Key as string ?? "UNKNOWN",
                    description: info.Description,
                    propertyType: this.valueType,
                    nullable: false,
                    getValue: () => (info.GetValue() as IDictionary)![kvp.Key],
                    setValue: value => (info.GetValue() as IDictionary)![kvp.Key] = value,
                    getCompiledParameters: info.GetCompiledParameters,
                    compiled: info.Compiled);

                DefinitionNode? node = DefinitionParser.ToNode(thing, kvpInfo);
                if (node != null)
                {
                    this.Nodes.Add(node);
                }
            }
        }
    }
}
