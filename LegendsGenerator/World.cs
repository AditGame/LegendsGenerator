// <copyright file="World.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator
{
    using System;
    using System.Collections.Generic;
    using LegendsGenerator.Contracts.Definitions.Events;
    using LegendsGenerator.Contracts.Things;

    /// <summary>
    /// The contents of everything in the world.
    /// </summary>
    public record World
    {
        /// <summary>
        /// A cache of the results of seearching for things by guid.
        /// </summary>
        private IDictionary<Guid, BaseThing> searchByGuidHash = new Dictionary<Guid, BaseThing>();

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
        /// Gets a list of all events which occurred in the previoud step.
        /// </summary>
        public IList<OccurredEvent> OccurredEvents { get; init; } = new List<OccurredEvent>()
        {
        };

        /// <summary>
        /// Gets or sets the events available in this world.
        /// </summary>
        public IList<EventDefinition> Events { get; set; } = new List<EventDefinition>();

        /// <summary>
        /// Gets a thing in the world.
        /// </summary>
        /// <param name="thingId">The thing ID.</param>
        /// <returns>The thing.</returns>
        /// <exception cref="KeyNotFoundException">The specified thing id does not exist in the world.</exception>
        public BaseThing FindThing(Guid thingId)
        {
            if (this.searchByGuidHash.TryGetValue(thingId, out BaseThing? result))
            {
                return result;
            }

            // Re-search the grid for things. This is good in case things got added to the grid in the meantime somehow.
            this.searchByGuidHash.Clear();
            foreach (var (_, _, square) in this.Grid.GetAllGridEntries())
            {
                foreach (BaseThing thing in square.ThingsInSquare)
                {
                    this.searchByGuidHash[thing.ThingId] = thing;
                }
            }

            if (this.searchByGuidHash.TryGetValue(thingId, out result))
            {
                return result;
            }
            else
            {
                throw new KeyNotFoundException($"Thing with ID {thingId} does not exist in this world.");
            }
        }
    }
}
