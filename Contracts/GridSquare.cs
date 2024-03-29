﻿// -------------------------------------------------------------------------------------------------
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
        /// A cache of the movement cost to mvoe through this square.
        /// </summary>
        private float? movementCost;

        /// <summary>
        /// Initializes a new instance of the <see cref="GridSquare"/> class.
        /// </summary>
        /// <param name="x">The X coord.</param>
        /// <param name="y">The Y coord.</param>
        public GridSquare(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Gets or sets the terrain of this square.
        /// </summary>
        public WorldSquare? SquareDefinition { get; set; }

        /// <summary>
        /// Gets the X coord of this square.
        /// </summary>
        public int X { get; }

        /// <summary>
        /// Gets the Y coord of this square.
        /// </summary>
        public int Y { get; }

        /// <summary>
        /// Gets the things in this grid square.
        /// </summary>
        private IList<BaseThing> ThingsInSquare { get; } = new List<BaseThing>();

        /// <summary>
        /// Gets the random number generator for this square.
        /// </summary>
        /// <param name="random">The random number generator.</param>
        /// <returns>The movement cost.</returns>
        public float GetMovementCost(Random random)
        {
            if (this.movementCost.HasValue)
            {
                return this.movementCost.Value;
            }

            this.movementCost = this.SquareDefinition?.Definition.EvalMovementCost(random, this.SquareDefinition) ?? 1;
            return this.movementCost.Value;
        }

        /// <summary>
        /// Clones this instance without things.
        /// </summary>
        /// <returns>This, except without things.</returns>
        public GridSquare CloneWithoutThings()
        {
            return new GridSquare(this.X, this.Y)
            {
            };
        }

        /// <summary>
        /// Gets all things on this square, including the square itself.
        /// </summary>
        /// <param name="excludeSquareDef">Exclude the definition of the square itself.</param>
        /// <returns>All thing matching type.</returns>
        public IEnumerable<BaseThing> GetThings(bool excludeSquareDef = false)
        {
            if (this.SquareDefinition != null && !excludeSquareDef)
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
            return this.GetThings().Where(x => x.ThingType == type);
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
