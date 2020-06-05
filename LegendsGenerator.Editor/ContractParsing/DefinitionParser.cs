// -------------------------------------------------------------------------------------------------
// <copyright file="DefinitionParser.cs" company="Tom Luppi">
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
    using System.Text;
    using System.Threading.Tasks;

    using LegendsGenerator.Contracts.Compiler;
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
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var options = properties.Where(p => p.Name.Contains("_")).ToLookup(p => p.Name.Split("_").First());

            List<DefinitionNode> nodes = new List<DefinitionNode>();
            foreach (var property in properties.Where(p => !p.Name.Contains("_")))
            {
                var node = ToNode(definition, property, options);
                if (node != null)
                {
                    nodes.Add(node);
                }
            }

            return nodes;
        }

        /// <summary>
        /// Converts a property into a node.
        /// </summary>
        /// <param name="thing">The object.</param>
        /// <param name="property">The property to convert.</param>
        /// <param name="options">The options.</param>
        /// <returns>The node if it can be converted, otherwise null.</returns>
        public static DefinitionNode? ToNode(object? thing, PropertyInfo property, ILookup<string, PropertyInfo>? optionsLookup = null)
        {
            MethodInfo? getParameters = thing?.GetType().GetMethod($"GetParameters{property.Name}");

            CompiledAttribute? compiled = property.GetCustomAttribute<CompiledAttribute>();
            ElementInfo info = new ElementInfo()
            {
                Name = property.Name.Split("_").Last(),
                Description = DescriptionProvider.GetDescription(property),
                PropertyType = property.PropertyType,
                GetParametersMethod = () => getParameters?.Invoke(thing, null) as IList<string> ?? new List<string>(),
                Compiled = compiled,
                Nullable = property.IsNullable(),
                GetMethod = () => property.GetValue(thing),
                SetMethod = value => property.SetValue(thing, value),
            };

            return ToNode(thing, info, optionsLookup);
        }

        /// <summary>
        /// Converts an element into a node.
        /// </summary>
        /// <param name="thing">The object.</param>
        /// <param name="info">The info to convert.</param>
        /// <param name="optionsLookup">The options.</param>
        /// <returns>The node if it can be converted, otherwise null.</returns>
        public static DefinitionNode? ToNode(object? thing, ElementInfo info, ILookup<string, PropertyInfo>? optionsLookup = null)
        {
            optionsLookup = optionsLookup ?? Array.Empty<PropertyInfo>().ToLookup(p => p.Name);
            IEnumerable<PropertyInfo> options = optionsLookup[info.Name];

            if (info.PropertyType == typeof(string))
            {
                if (info.Compiled != null)
                {
                    return new CompiledDefinitionNode(
                        info.Compiled,
                        thing,
                        info,
                        options);
                }
                else
                {
                    return new StringDefinitionNode(
                        thing,
                        info,
                        options);
                }
            }
            else if (info.PropertyType.IsSubclassOf(typeof(BaseDefinition)))
            {
                return new SectionDefinitionNode(
                    thing,
                    info,
                    options);
            }
            else if (info.PropertyType.IsEnum)
            {
                return new EnumDefinitionNode(
                    thing,
                    info,
                    options);
            }
            else if (info.PropertyType == typeof(bool))
            {
                return new BoolDefinitionNode(
                    thing,
                    info,
                    options);
            }
            else if (info.PropertyType == typeof(int))
            {
                return new IntDefinitionNode(
                    thing,
                    info,
                    options);
            }
            else if (typeof(IDictionary).IsAssignableFrom(info.PropertyType))
            {
                return new DictionaryDefinitionNode(
                    thing,
                    info,
                    options);
            }
            else if (typeof(IList).IsAssignableFrom(info.PropertyType))
            {
                return new ListDefinitionNode(
                    thing,
                    info,
                    options);
            }
            else
            {
                Console.WriteLine("asdf");
            }

            return null;
        }
    }
}