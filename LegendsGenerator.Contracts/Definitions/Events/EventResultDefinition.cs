// <copyright file="EventResultDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>


namespace LegendsGenerator.Contracts.Definitions.Events
{
    using System;

    using LegendsGenerator.Contracts.Compiler;

    /// <summary>
    /// The result of an event occurring.
    /// </summary>
    [UsesAdditionalParametersForHoldingClass]
    public partial class EventResultDefinition : BaseDefinition
    {
        /// <summary>
        /// Gets or sets a value indicating whether this result will be picked if no other result is applicable.
        /// </summary>
        public bool Default { get; set; }

        /// <summary>
        /// Gets or sets a string which returns a number between 1 and 100 representing the chance of this happening.
        /// </summary>
        [Compiled(typeof(int), "Subject")]
        public string Chance { get; set; }

        /// <summary>
        /// Gets or sets the string condition of this event.
        /// </summary>
        [Compiled(typeof(bool), "Subject")]
        public string Condition { get; set; }

        /// <summary>
        /// Gets or sets the effects of this result.
        /// </summary>
        public EffectDefinition[] Effects { get; set; } = Array.Empty<EffectDefinition>();

        /// <summary>
        /// Gets or sets the spawns of this result.
        /// </summary>
        public SpawnDefinition[] Spawns { get; set; } = Array.Empty<SpawnDefinition>();
    }
}
