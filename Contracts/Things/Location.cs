// <copyright file="Location.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Things
{
    /// <summary>
    /// A location on the grid.
    /// </summary>
    public record Location
    {
        /// <summary>
        /// Gets or sets the X coord.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the Y coord.
        /// </summary>
        public int Y { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{this.X},{this.Y}";
        }
    }
}
