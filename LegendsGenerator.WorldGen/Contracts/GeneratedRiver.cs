// -------------------------------------------------------------------------------------------------
// <copyright file="GeneratedRiver.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.WorldGen.Contracts
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Information about a river generated on this tile.
    /// </summary>
    public class GeneratedRiver
    {
        /// <summary>
        /// Gets or sets information about rivers coming in from the north.
        /// </summary>
        public GeneratedRiverBorder? North { get; set; }

        /// <summary>
        /// Gets or sets information about rivers coming in from the south.
        /// </summary>
        public GeneratedRiverBorder? South { get; set; }

        /// <summary>
        /// Gets or sets information about rivers coming in from the west.
        /// </summary>
        public GeneratedRiverBorder? West { get; set; }

        /// <summary>
        /// Gets or sets information about rivers coming in from the east.
        /// </summary>
        public GeneratedRiverBorder? East { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            List<(string Direction, GeneratedRiverBorder Border)> borders = new List<(string Direction, GeneratedRiverBorder? Border)>()
            {
                ("N", this.North),
                ("S", this.South),
                ("E", this.East),
                ("W", this.West),
            }.Where(d => d.Border != null).Select(d => (d.Direction, d.Border!)).ToList();

            if (!borders.Any())
            {
                return "None";
            }

            var into = borders.Where(d => d.Border.FlowingIn);
            var outOf = borders.Where(d => !d.Border.FlowingIn);

            StringBuilder sb = new StringBuilder();
            sb.Append(string.Join(string.Empty, into.Select(d => d.Direction)));
            sb.Append("->");
            sb.Append(string.Join(string.Empty, outOf.Select(d => d.Direction)));

            return sb.ToString();
        }
    }
}
