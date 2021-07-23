// <copyright file="ThingTypes.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions.Events
{
    using System;
    using System.Text.Json.Serialization;
    using LegendsGenerator.Contracts.Things;

    /// <summary>
    /// The types of things.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ThingType
    {
        /// <summary>
        /// Unknown type.
        /// </summary>
        [Validation.InvalidEnumValue]
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

        /// <summary>
        /// A square on the world map.
        /// </summary>
        WorldSquare,

        /// <summary>
        /// A quest a Thing is on.
        /// </summary>
        Quest,

        /// <summary>
        /// The world!.
        /// </summary>
        World,
    }

    /// <summary>
    /// Extensions for the ThingType enum.
    /// </summary>
#pragma warning disable SA1649 // File name should match first type name. This preferred.
    public static class ThingTypeExtensions
#pragma warning restore SA1649 // File name should match first type name
    {
        /// <summary>
        /// Gets the C# type associated with this thing type.
        /// </summary>
        /// <param name="thing">The thing type.</param>
        /// <returns>The associated type.</returns>
        public static Type AssociatedType(this ThingType thing) => thing switch
        {
            ThingType.Site => typeof(Site),
            ThingType.NotablePerson => typeof(NotablePerson),
            ThingType.Unit => typeof(Unit),
            ThingType.WorldSquare => typeof(WorldSquare),
            ThingType.Quest => typeof(Quest),
            ThingType.World => typeof(World),
            _ => typeof(BaseThing),
        };
    }
}
