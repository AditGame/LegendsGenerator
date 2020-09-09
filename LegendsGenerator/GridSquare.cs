// -------------------------------------------------------------------------------------------------
// <copyright file="GridSquare.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using LegendsGenerator.Contracts;
    using LegendsGenerator.Contracts.Definitions.Events;

    /// <summary>
    /// A single square of the grid.
    /// </summary>
    public class GridSquare
    {
        /// <summary>
        /// Gets the things in this grid square.
        /// </summary>
        public IList<BaseThing> ThingsInGrid { get; } = new List<BaseThing>();

        /// <summary>
        /// Gets or sets the terrain of this square.
        /// </summary>
        public TerrainSquare Terrain { get; set; } = new TerrainSquare();

        /// <summary>
        /// Clones this instance without things.
        /// </summary>
        /// <returns>This, except without things.</returns>
        public GridSquare CloneWithoutThings()
        {
            return new GridSquare()
            {
                Terrain = this.Terrain,
            };
        }

        /// <summary>
        /// Gets things based on type.
        /// </summary>
        /// <param name="type">The type of thing to get.</param>
        /// <returns>All thing matching type.</returns>
        public IEnumerable<BaseThing> GetThings(ThingType type)
        {
            return this.ThingsInGrid.Where(x => x.ThingType == type);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (ThingType type in Enum.GetValues(typeof(ThingType)).OfType<ThingType>())
            {
                sb.AppendLine($"{type}:");

                foreach (BaseThing thing in this.GetThings(type))
                {
                    sb.AppendLine($"   {thing}");
                }
            }

            return sb.ToString();
        }
    }
}
