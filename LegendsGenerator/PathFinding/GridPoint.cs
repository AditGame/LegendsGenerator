// -------------------------------------------------------------------------------------------------
// <copyright file="GridPoint.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.PathFinding
{
    using System;

    /// <summary>
    /// A point on the grid.
    /// </summary>
    public struct GridPoint : IEquatable<GridPoint>
    {
        /// <summary>
        /// Gets or sets the cost of this point in the grid.
        /// </summary>
        public float Cost { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this point is water.
        /// </summary>
        public bool IsWater { get; set; }

        public static bool operator ==(GridPoint left, GridPoint right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GridPoint left, GridPoint right)
        {
            return !(left == right);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (obj is GridPoint point)
            {
                return this.Equals(point);
            }

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(this.Cost, this.IsWater);
        }

        /// <inheritdoc/>
        public bool Equals(GridPoint other)
        {
            return this.Cost == other.Cost && this.IsWater == other.IsWater;
        }

        /// <summary>
        /// Calculates the cost of this square.
        /// </summary>
        /// <param name="waterCostRatio">The ratio of land movement to water movement cost.</param>
        /// <returns>The actual cost of this node.</returns>
        public float CalcCost(float waterCostRatio)
        {
            return this.IsWater ? this.Cost * waterCostRatio : this.Cost;
        }
    }
}
