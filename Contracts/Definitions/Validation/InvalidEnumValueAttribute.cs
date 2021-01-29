// <copyright file="InvalidEnumValueAttribute.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions.Validation
{
    using System;

    /// <summary>
    /// Represents an enum value that can't actually be used.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class InvalidEnumValueAttribute : Attribute
    {
    }
}
