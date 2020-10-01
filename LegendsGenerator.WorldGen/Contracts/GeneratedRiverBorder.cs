// -------------------------------------------------------------------------------------------------
// <copyright file="GeneratedRiverBorder.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.WorldGen.Contracts
{
    /// <summary>
    /// Information about the river entering or leaving a tile.
    /// </summary>
    public class GeneratedRiverBorder
    {
        /// <summary>
        /// Gets or sets a value indicating whether the flow of water is into this tile or out of this tile.
        /// </summary>
        public bool FlowingIn { get; set; }

        /// <summary>
        /// Gets or sets the strength of this river at the border of the tile.
        /// </summary>
        public int Strength { get; set; }
    }
}
