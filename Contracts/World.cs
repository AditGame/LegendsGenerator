// <copyright file="World.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using LegendsGenerator.Contracts.Things;

    /// <summary>
    /// The contents of everything in the world.
    /// </summary>
    public record World
    {
        /// <summary>
        /// Backing friend for Step Count.
        /// </summary>
        private int step;

        /// <summary>
        /// A cache of the results of searching for things by GUID.
        /// </summary>
        private IDictionary<Guid, BaseThing> searchByGuidHash = new Dictionary<Guid, BaseThing>();

        /// <summary>
        /// Gets the world seed. This should be randomly picked at the start and not changed.
        /// </summary>
        public int WorldSeed { get; init; } = new Random().Next();

        /// <summary>
        /// Gets the step which this world is currently in. Starts at 1.
        /// </summary>
        public int StepCount
        {
            get => this.step;
            set
            {
                this.step = value;

                // The search by GUID hash should be cleared with each copy, but there's no way to inject custom logic into the copy step.
                // Clearing when we change the step is good enough as we need to do that on each copy anyways.
                // Once Records are no longer a preview feature we should revisit this.
                this.searchByGuidHash = new Dictionary<Guid, BaseThing>();
            }
        }

        /// <summary>
        /// Gets the grid of this world.
        /// </summary>
        public WorldGrid Grid { get; init; } = new WorldGrid(0, 0);

        /// <summary>
        /// Gets a list of all events which occurred in the previous step.
        /// </summary>
        public IList<OccurredEvent> OccurredEvents { get; init; } = new List<OccurredEvent>();

        /// <summary>
        /// Gets a thing in the world.
        /// </summary>
        /// <param name="thingId">The thing ID.</param>
        /// <param name="result">The thing, if found.</param>
        /// <returns>True if the thing exists in the world, false otherwise.</returns>
        public bool TryFindThing(Guid thingId, [NotNullWhen(true)] out BaseThing? result)
        {
            if (this.searchByGuidHash.TryGetValue(thingId, out result))
            {
                return true;
            }

            // Re-search the grid for things. This is good in case things got added to the grid in the meantime somehow.
            this.searchByGuidHash.Clear();
            foreach (var (_, _, square) in this.Grid.GetAllGridEntries())
            {
                foreach (BaseThing thing in square.GetThings())
                {
                    this.searchByGuidHash[thing.ThingId] = thing;
                }
            }

            if (this.searchByGuidHash.TryGetValue(thingId, out result))
            {
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }

        /// <summary>
        /// Gets a thing in the world.
        /// </summary>
        /// <param name="thingId">The thing ID.</param>
        /// <returns>The thing.</returns>
        /// <exception cref="KeyNotFoundException">The specified thing id does not exist in the world.</exception>
        public BaseThing FindThing(Guid thingId)
        {
            if (this.TryFindThing(thingId, out BaseThing? result))
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
