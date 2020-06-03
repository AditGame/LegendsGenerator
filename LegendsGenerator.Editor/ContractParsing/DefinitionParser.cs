// -------------------------------------------------------------------------------------------------
// <copyright file="DefinitionParser.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor.ContractParsing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    using LegendsGenerator.Contracts.Definitions;

    /// <summary>
    /// Parses a contract definition into nodes for display and edit.
    /// </summary>
    public static class DefinitionParser
    {
        /// <summary>
        /// Converts the given definition into nodes.
        /// </summary>
        /// <param name="type">The type of definition.</param>
        /// <param name="definition">The definition to convert.</param>
        /// <returns>A lsit of all nodes on the definition.</returns>
        public static IList<DefinitionNode> ParseToNodes(Type type, object? definition)
        {
            List<DefinitionNode> nodes = new List<DefinitionNode>();
            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.PropertyType == typeof(string))
                {
                    nodes.Add(new StringDefinitionNode(
                        DescriptionProvider.GetDescription(property),
                        property.Name,
                        property.IsNullable(),
                        () => (definition != null ? (property.GetValue(definition) as string) : null) ?? string.Empty,
                        input => property.SetValue(definition, input)));
                }
                else if (property.PropertyType.IsSubclassOf(typeof(BaseDefinition)))
                {
                    nodes.Add(new SectionDefinitionNode(
                        DescriptionProvider.GetDescription(property),
                        property.Name,
                        property.PropertyType,
                        property.GetValue(definition),
                        property.IsNullable(),
                        newThing => property.SetValue(definition, newThing),
                        () => property.SetValue(definition, null)));
                }
                else if (property.PropertyType.IsEnum)
                {
                    nodes.Add(new EnumDefinitionNode(
                        DescriptionProvider.GetDescription(property),
                        property.Name,
                        property.IsNullable(),
                        property.PropertyType,
                        () => property.GetValue(definition) as Enum ?? (Enum)Enum.ToObject(property.PropertyType, 0),
                        input => property.SetValue(definition, input)));
                }
            }

            return nodes;
        }
    }
}
