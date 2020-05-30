// <copyright file="ThingTypes.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions.Events
{
    using System;
    using System.Text.Json.Serialization;

    /// <summary>
    /// The types of things.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ThingType
    {
        /// <summary>
        /// Unknown type.
        /// </summary>
        None = 0,

        /// <summary>
        /// A site (static location).
        /// </summary>
        Site,

        /// <summary>
        /// A notable person.
        /// </summary>
        NotablePerson,

        /// <summary>
        /// A unit (grouping of things).
        /// </summary>
        Unit,
    }
}
