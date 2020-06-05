// -------------------------------------------------------------------------------------------------
// <copyright file="DictionaryDefinitionNode.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor.ContractParsing
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    public class DictionaryDefinitionNode : DefinitionNode
    {
        public DictionaryDefinitionNode(object? thing, ElementInfo info, IEnumerable<PropertyInfo> options, bool readOnly = false)
            : base(thing, info, options, readOnly)
        {
            IDictionary dictionary = info.GetMethod() as IDictionary;

            foreach (DictionaryEntry kvp in dictionary)
            {
                ElementInfo kvpInfo = new ElementInfo()
                {
                    Name = kvp.Key as string,
                    Description = info.Description,
                    Compiled = info.Compiled,
                    Nullable = false,
                    PropertyType = kvp.Value.GetType(),
                    GetMethod = () => (info.GetMethod() as IDictionary)[kvp.Key],
                    SetMethod = value => (info.GetMethod() as IDictionary)[kvp.Key] = value,
                };

                this.Nodes.Add(DefinitionParser.ToNode(thing, kvpInfo));
            }
        }
    }
}
