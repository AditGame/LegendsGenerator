// <copyright file="HideInEditorAttribute.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions.Validation
{
    using System;

    /// <summary>
    /// An attribute which allows dynamically hiding properties in the editor.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class HideInEditorAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition">The condition to process. 'value' is the only supported variable, and it is the object holding this property.</param>
        public HideInEditorAttribute(string condition)
        {
            this.Condition = condition;
        }

        /// <summary>
        /// Gets or sets the string C# condition to hide attributes.
        /// </summary>
        /// <remarks>One property is passed in, value, which is the holding property.</remarks>
        public string Condition { get; }
    }
}
