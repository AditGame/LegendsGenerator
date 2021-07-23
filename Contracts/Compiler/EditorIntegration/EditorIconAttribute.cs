// -------------------------------------------------------------------------------------------------
// <copyright file="EditorIconAttribute.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Contracts.Compiler.EditorIntegration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// An icon to add to this in the Editor.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class EditorIconAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditorIconAttribute"/> class.
        /// </summary>
        /// <param name="icon">The icon to show.</param>
        public EditorIconAttribute(EditorIcon icon)
        {
            this.Icon = icon;
        }

        /// <summary>
        /// Gets the icon to display with this property.
        /// </summary>
        public EditorIcon Icon { get; }
    }
}
