// -------------------------------------------------------------------------------------------------
// <copyright file="PathFinderOptions.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
//
// <remarks>
//     Whole or part copied from https://github.com/valantonini/AStar/blob/master/AStar.Core/PriorityQueueB.cs. See LICENSE.txt in this directory for details.
// </remarks>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.PathFinding
{
    /// <summary>
    /// Options for the path finder.
    /// </summary>
    public class PathFinderOptions
    {
        /// <summary>
        /// Gets or sets the heuristic for estimated-distance-left.
        /// </summary>
        public HeuristicFormula Formula { get; set; } = HeuristicFormula.Manhattan;

        /// <summary>
        /// Gets or sets a value indicating whether diagonals are allowed.
        /// </summary>
        public bool Diagonals { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether diagonals should cost 1.5 a normal move.
        /// </summary>
        public bool HeavyDiagonals { get; set; }

        /// <summary>
        /// Gets or sets the weight of the heuristic.
        /// </summary>
        public int HeuristicEstimate { get; set; } = 2;

        /// <summary>
        /// Gets or sets a value indicating whether ties should be carefully considered or randomly chosen.
        /// </summary>
        public bool TieBreaker { get; set; }

        /// <summary>
        /// Gets or sets how many squares to check before failing the search.
        /// </summary>
        public int SearchLimit { get; set; } = int.MaxValue;
    }
}
