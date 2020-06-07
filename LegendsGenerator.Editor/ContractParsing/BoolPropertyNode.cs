// -------------------------------------------------------------------------------------------------
// <copyright file="BoolPropertyNode.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor.ContractParsing
{
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// A node which represents a bool value.
    /// </summary>
    public class BoolPropertyNode : PropertyNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BoolPropertyNode"/> class.
        /// </summary>
        /// <param name="thing">The thing this node points to.</param>
        /// <param name="property">The property info.</param>
        /// <param name="options">The options for this node.</param>
        /// <param name="readOnly">If this instance should be read only.</param>
        public BoolPropertyNode(object? thing, ElementInfo property, IEnumerable<PropertyInfo> options, bool readOnly = false)
            : base(thing, property, options, readOnly)
        {
        }
    }
}
