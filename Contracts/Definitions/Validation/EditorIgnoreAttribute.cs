// <copyright file="EditorIgnoreAttribute.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions.Validation
{
    using System;

    /// <summary>
    /// If on a property, the property will not be dispalyed on the object in the editor.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class EditorIgnoreAttribute : Attribute
    {
    }
}
