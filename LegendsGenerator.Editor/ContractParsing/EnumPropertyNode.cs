// -------------------------------------------------------------------------------------------------
// <copyright file="EnumPropertyNode.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor.ContractParsing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using LegendsGenerator.Contracts.Definitions.Validation;

    /// <summary>
    /// A definition node which is an enum.
    /// </summary>
    public class EnumPropertyNode : PropertyNode
    {
        /// <summary>
        /// The enum type.
        /// </summary>
        private readonly Type type;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumPropertyNode"/> class.
        /// </summary>
        /// <param name="thing">The thing this node points to.</param>
        /// <param name="property">The property info.</param>
        /// <param name="options">The options for this node.</param>
        /// <param name="readOnly">If this instance should be read only.</param>
        public EnumPropertyNode(
            object? thing,
            ElementInfo property,
            IEnumerable<PropertyInfo> options,
            bool readOnly = false)
            : base(thing, property, options, readOnly)
        {
            this.type = property.PropertyType;

            this.EnumValues = Enum.GetNames(this.type);
        }

        /// <summary>
        /// Gets or sets the enum value in string form.
        /// </summary>
        public string EnumValue
        {
            get
            {
                return Enum.GetName(this.type, this.Content ?? "None") ?? Enum.GetNames(this.type).First();
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

        /// <inheritdoc/>
        protected override ICollection<ValidationIssue> GetLevelIssues()
        {
            ICollection<ValidationIssue> output = base.GetLevelIssues();

            if (this.type.GetMember(this.EnumValue).FirstOrDefault()?.GetCustomAttribute<InvalidEnumValueAttribute>() != null)
            {
                output.Add(new ValidationIssue(
                    ValidationLevel.Error,
                    "Invalid value."));
            }

            return output;
        }
    }
}
