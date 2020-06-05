// -------------------------------------------------------------------------------------------------
// <copyright file="IntDefinitionNode.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor.ContractParsing
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// A definition node which is an integer.
    /// </summary>
    public class IntDefinitionNode : DefinitionNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntDefinitionNode"/> class.
        /// </summary>
        /// <param name="thing">The thing this node points to.</param>
        /// <param name="property">The property info.</param>
        /// <param name="options">The options for this node.</param>
        /// <param name="readOnly">If this instance should be read only.</param>
        public IntDefinitionNode(object? thing, ElementInfo property, IEnumerable<PropertyInfo> options, bool readOnly = false)
            : base(thing, property, options, readOnly)
        {
        }

        /// <summary>
        /// Gets or sets te contents as a string.
        /// </summary>
        public string AsString
        {
            get
            {
                return this.Content?.ToString() ?? "0";
            }

            set
            {
                if (!int.TryParse(value, out int val))
                {
                    throw new InvalidOperationException("Input must be a string.");
                }

                this.Content = val;
            }
        }
    }
}
