// <copyright file="World.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator
{
    using System;
    using System.Collections.Generic;

    using LegendsGenerator.Contracts;
    using LegendsGenerator.Contracts.Definitions.Events;

    /// <summary>
    /// The contents of everything in the world.
    /// </summary>
    public class World
    {
        /// <summary>
        /// Gets or sets the worldseed. This should be randomly picked at the start and not changed.
        /// </summary>
        public int WorldSeed { get; set; } = new Random().Next();

        /// <summary>
        /// Gets or sets the step which this world is currently in. Starts at 1.
        /// </summary>
        public int StepCount { get; set; } = 1;

        /// <summary>
        /// Gets or sets the sites in this world.
        /// </summary>
        public IList<Site> Sites { get; set; } = new List<Site>();

        /// <summary>
        /// Gets or sets the events available in this world.
        /// </summary>
        public IList<EventDefinition> Events { get; set; } = new List<EventDefinition>();

        /// <summary>
        /// Clones this isntance.
        /// </summary>
        /// <returns>A clone of this instance.</returns>
        public World Clone()
        {
            return this;
        }

        /// <summary>
        /// Gets things based on type.
        /// </summary>
        /// <param name="type">The type of thing to get.</param>
        /// <returns>All thing matching type.</returns>
        public IEnumerable<BaseThing> GetThings(ThingType type)
        {
            switch (type)
            {
                case ThingType.Site:
                    return this.Sites;
                default:
                    throw new ApplicationException($"Thing type {type} not wired in here yet.");
            }
        }
    }
}
