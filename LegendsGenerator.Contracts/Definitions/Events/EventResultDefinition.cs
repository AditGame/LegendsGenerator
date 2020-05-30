// <copyright file="EventResultDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions.Events
{
    /// <summary>
    /// The result of an event occurring.
    /// </summary>
    public class EventResultDefinition
    {
        /// <summary>
        /// Gets or sets a value indicating whether this result will be picked if no other result is applicable.
        /// </summary>
        public bool Default { get; set; }

        /// <summary>
        /// Gets or sets a string which returns a number between 1 and 100 representing the chance of this happening.
        /// </summary>
        public string Chance { get; set; }

        /// <summary>
        /// Gets or sets the string condition of this event.
        /// </summary>
        public string Condition { get; set; }

        /// <summary>
        /// Gets or sets the effects of this result.
        /// </summary>
        public EffectDefinition[] Effects { get; set; }

        /// <summary>
        /// Gets or sets the spawns of this result.
        /// </summary>
        public SpawnDefinition[] Spawns { get; set; }
    }
}
