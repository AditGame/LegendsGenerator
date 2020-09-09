// <copyright file="PositionType.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions.Events
{
    using System;
    using System.Text.Json.Serialization;

    /// <summary>
    /// The type a position is.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PositionType
    {
        /// <summary>
        /// Unknown type.
        /// </summary>
        [Validation.InvalidEnumValue]
        None = 0,

        /// <summary>
        /// Absolute to the world stage.
        /// </summary>
        Absolute,

        /// <summary>
        /// Relative to the subject, moving to ensure the position is in a valid terrain type for the object.
        /// </summary>
        [Obsolete("Not set up correctly yet.")]
        RelativeSafe,

        /// <summary>
        /// Relative to the subject, reguardless of terrain type.
        /// </summary>
        RelativeAbsolute,
    }
}
