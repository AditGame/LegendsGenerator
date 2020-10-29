// -------------------------------------------------------------------------------------------------
// <copyright file="GeneratedSquare.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.WorldGen.Contracts
{
    /// <summary>
    /// A generated square of land.
    /// </summary>
    public class GeneratedSquare
    {
        /// <summary>
        /// Gets or sets the elevation of this tile.
        /// </summary>
        public int Elevation { get; set; }

        /// <summary>
        /// Gets or sets the rainfall on this tile.
        /// </summary>
        public int Rainfall { get; set; }

        /// <summary>
        /// Gets or sets the drainage of this tile.
        /// </summary>
        public int Drainage { get; set; }

        /// <summary>
        /// Gets or sets the average temperature of this tile.
        /// </summary>
        public int Temperature { get; set; }

        /// <summary>
        /// Gets or sets the Evilness of this tile.
        /// </summary>
        public int Evil { get; set; }

        /// <summary>
        /// Gets or sets the savagery of this tile.
        /// </summary>
        public int Savagery { get; set; }

        /// <summary>
        /// Gets or sets the amount of underground materials here.
        /// </summary>
        public int Materials { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this tile contains water.
        /// </summary>
        public bool Water { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this tile, if it contains water, contains salt water.
        /// </summary>
        public bool SaltWater { get; set; }

        /// <summary>
        /// Gets or sets information about any rivers which pass through this tile.
        /// </summary>
        public GeneratedRiver? River { get; set; }

        /// <summary>
        /// Gets or sets any interesting points of interest on this tile.
        /// </summary>
        public PointOfInterest PointOfInterest { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Elevation:{this.Elevation} " +
                $"Rainfall:{this.Rainfall} " +
                $"Drainage:{this.Drainage} " +
                $"Temperature:{this.Temperature} " +
                $"Evil:{this.Evil} " +
                $"Savagery:{this.Savagery} " +
                $"Water:{this.Water} " +
                $"SaltWater:{this.SaltWater} " +
                $"River:{this.River} " +
                $"POI:{this.PointOfInterest}";
        }
    }
}
