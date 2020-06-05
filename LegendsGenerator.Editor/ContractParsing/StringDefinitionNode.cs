// -------------------------------------------------------------------------------------------------
// <copyright file="StringDefinitionNode.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor.ContractParsing
{
    using System.Collections.Generic;
    using System.Reflection;
    using LegendsGenerator.Contracts.Definitions;
    using LegendsGenerator.Contracts.Definitions.Validation;

    /// <summary>
    /// A contract node which is a string.
    /// </summary>
    public class StringDefinitionNode : DefinitionNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringDefinitionNode"/> class.
        /// </summary>
        /// <param name="thing">The thing this node points to.</param>
        /// <param name="property">The property info.</param>
        /// <param name="options">The options for this node.</param>
        /// <param name="readOnly">If this instance should be read only.</param>
        public StringDefinitionNode(object? thing, ElementInfo property, IEnumerable<PropertyInfo> options, bool readOnly = false)
            : base(thing, property, options, readOnly)
        {
        }

        /// <inheritdoc/>
        protected override List<ValidationIssue> GetLevelIssues()
        {
            List<ValidationIssue> output = base.GetLevelIssues();

            if (string.IsNullOrWhiteSpace(this.Content as string))
            {
                output.Add(new ValidationIssue(
                    ValidationLevel.Error,
                    "Can not be null, empty, or whitespace."));
            }
            else if (this.Content?.ToString()?.Equals(BaseDefinition.UnsetString) == true)
            {
                output.Add(new ValidationIssue(
                    ValidationLevel.Error,
                    "String can not be the default string."));
            }

            return output;
        }
    }
}
