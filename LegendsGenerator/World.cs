// <copyright file="World.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator
{
    using System;
    using System.Collections.Generic;

    using LegendsGenerator.Contracts.Definitions.Events;

    /// <summary>
    /// The contents of everything in the world.
    /// </summary>
    public record World
    {
        /// <summary>
        /// Gets the worldseed. This should be randomly picked at the start and not changed.
        /// </summary>
        public int WorldSeed { get; init; } = new Random().Next();

        /// <summary>
        /// Gets the step which this world is currently in. Starts at 1.
        /// </summary>
        public int StepCount { get; init; } = 1;

        /// <summary>
        /// Gets the grid of this world.
        /// </summary>
        public Grid Grid { get; init; } = new Grid(0, 0);

        /// <summary>
        /// Gets or sets the events available in this world.
        /// </summary>
        public IList<EventDefinition> Events { get; set; } = new List<EventDefinition>();
    }
}
