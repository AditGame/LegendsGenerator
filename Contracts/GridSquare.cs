// -------------------------------------------------------------------------------------------------
// <copyright file="GridSquare.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using LegendsGenerator.Contracts.Definitions.Events;
    using LegendsGenerator.Contracts.Things;

    /// <summary>
    /// A single square of the grid.
    /// </summary>
    public class GridSquare
    {
        /// <summary>
        /// Gets the things in this grid square.
        /// </summary>
        private IList<BaseThing> ThingsInSquare { get; } = new List<BaseThing>();

        /// <summary>
        /// Gets or sets the terrain of this square.
        /// </summary>
        public WorldSquare? SquareDefinition { get; set; }

        /// <summary>
        /// Clones this instance without things.
        /// </summary>
        /// <returns>This, except without things.</returns>
        public GridSquare CloneWithoutThings()
        {
            return new GridSquare()
            {
            };
        }

        /// <summary>
        /// Gets all things on this square, including the square itself.
        /// </summary>
        /// <returns>All thing matching type.</returns>
        public IEnumerable<BaseThing> GetThings()
        {
            if (this.SquareDefinition != null)
            {
                yield return this.SquareDefinition;
            }

            foreach (BaseThing thing in this.ThingsInSquare)
            {
                yield return thing;
            }
        }

        /// <summary>
        /// Gets things based on type.
        /// </summary>
        /// <param name="type">The type of thing to get.</param>
        /// <returns>All thing matching type.</returns>
        public IEnumerable<BaseThing> GetThings(ThingType type)
        {
            return GetThings().Where(x => x.ThingType == type);
        }

        /// <summary>
        /// Adds a thing to this square.
        /// </summary>
        /// <param name="thing">The thing to add.</param>
        /// <exception cref="InvalidOperationException">A WorldSquare was added to the square when it already had a world square.</exception>
        public void AddThing(BaseThing thing)
        {
            if (thing.ThingType == ThingType.WorldSquare)
            {
                if (this.SquareDefinition != null)
                {
                    throw new InvalidOperationException($"Can not add World Squares to a square which already has a world square.");
                }

                this.SquareDefinition = thing as WorldSquare;
                return;
            }

            this.ThingsInSquare.Add(thing);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            // Do by type so it's sorted.
            foreach (ThingType type in Enum.GetValues(typeof(ThingType)).OfType<ThingType>())
            {
                foreach (BaseThing thing in this.GetThings(type))
                {
                    sb.AppendLine($"{thing.BaseDefinition.Name} {thing.Name}");
                }
            }

            return sb.ToString();
        }
    }
}
