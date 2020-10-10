// -------------------------------------------------------------------------------------------------
// <copyright file="PointComparer.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
//
// <remarks>
//     Whole or part copied from https://github.com/valantonini/AStar/blob/master/AStar.Core/PriorityQueueB.cs. See LICENSE.txt in this directory for details.
// </remarks>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.PathFinding
{
    using System.Collections.Generic;

    /// <summary>
    /// The square comparer.
    /// </summary>
    internal class PointComparer : IComparer<Point>
    {
        /// <summary>
        /// The world grid.
        /// </summary>
        private readonly PathFinderNode[,] grid;

        /// <summary>
        /// Initializes a new instance of the <see cref="PointComparer"/> class.
        /// </summary>
        /// <param name="grid">The world grid.</param>
        public PointComparer(PathFinderNode[,] grid)
        {
            this.grid = grid;
        }

        /// <inheritdoc/>
        public int Compare(Point a, Point b)
        {
            if (a == null || b == null)
            {
                return 0;
            }

            if (this.grid[a.X, a.Y].F > this.grid[b.X, b.Y].F)
            {
                return 1;
            }

            if (this.grid[a.X, a.Y].F < this.grid[b.X, b.Y].F)
            {
                return -1;
            }

            return 0;
        }
    }
}
