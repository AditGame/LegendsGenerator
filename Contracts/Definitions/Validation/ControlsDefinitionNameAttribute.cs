// <copyright file="ControlsDefinitionNameAttribute.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions.Validation
{
    using System;

    /// <summary>
    /// If put on a property, this informs the editor that the property modifies the definition name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ControlsDefinitionNameAttribute : Attribute
    {
    }
}
