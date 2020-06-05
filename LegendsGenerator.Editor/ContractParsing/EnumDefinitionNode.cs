// -------------------------------------------------------------------------------------------------
// <copyright file="EnumDefinitionNode.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor.ContractParsing
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class EnumDefinitionNode : DefinitionNode
    {
        private Type type;

        public EnumDefinitionNode(
            object? thing,
            ElementInfo property,
            IEnumerable<PropertyInfo> options,
            bool readOnly = false)
            : base(thing, property, options, readOnly)
        {
            this.type = property.PropertyType;

            this.EnumValues = Enum.GetNames(this.type);
        }

        public string EnumValue
        {
            get
            {
                return Enum.GetName(this.type, this.Content);
            }

            set
            {
                this.Content = Enum.Parse(this.type, value);
            }
        }

        /// <summary>
        /// Gets the list of options.
        /// </summary>
        public IList<string> EnumValues { get; }
    }
}
