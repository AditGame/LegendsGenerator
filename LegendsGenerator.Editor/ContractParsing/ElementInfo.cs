// -------------------------------------------------------------------------------------------------
// <copyright file="ElementInfo.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor.ContractParsing
{
    using System;
    using System.Collections.Generic;
    using LegendsGenerator.Contracts.Compiler;

    /// <summary>
    /// Information about an element of a definition.
    /// </summary>
    public class ElementInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementInfo"/> class.
        /// </summary>
        /// <param name="name">The element name.</param>
        /// <param name="description">In informative description of the element.</param>
        /// <param name="propertyType">The type of the property.</param>
        /// <param name="nullable">If this element can be set to null.</param>
        /// <param name="getValue">A method which returns the value.</param>
        /// <param name="setValue">A method which sets the value.</param>
        /// <param name="getCompiledParameters">A method which gets the parameters to pass into a compiled method.</param>
        /// <param name="compiled">The compiled attribute, if it exists.</param>
        public ElementInfo(
            string name,
            string description,
            Type propertyType,
            bool nullable,
            Func<object?> getValue,
            Action<object?> setValue,
            Func<IList<string>>? getCompiledParameters,
            CompiledAttribute? compiled)
        {
            this.Name = name;
            this.Description = description;
            this.PropertyType = propertyType;
            this.Nullable = nullable;
            this.GetValue = getValue;
            this.SetValue = setValue;
            this.GetCompiledParameters = getCompiledParameters != null ? getCompiledParameters : () => Array.Empty<string>();
            this.Compiled = compiled;
        }

        /// <summary>
        /// Gets or sets the name of the element.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a method which gets the value of this element.
        /// </summary>
        public Func<object?> GetValue { get; set; }

        /// <summary>
        /// Gets or sets a method which sets the value of this element.
        /// </summary>
        public Action<object?> SetValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this element can be set to null.
        /// </summary>
        public bool Nullable { get; set; }

        /// <summary>
        /// Gets or sets a description of the element.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the element type.
        /// </summary>
        public Type PropertyType { get; set; }

        /// <summary>
        /// Gets or sets a method which gets the compiled parameters of this element.
        /// </summary>
        public Func<IList<string>> GetCompiledParameters { get; set; }

        /// <summary>
        /// Gets or sets the compiled attribute.
        /// </summary>
        public CompiledAttribute? Compiled { get; set; }

        /// <summary>
        /// Gets or sets an optional method which changes the name of the node.
        /// </summary>
        public Action<string>? ChangeName { get; set; }
    }
}
