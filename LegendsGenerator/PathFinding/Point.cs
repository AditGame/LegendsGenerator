// -------------------------------------------------------------------------------------------------
// <copyright file="Point.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
//
// <remarks>
//     Whole or part copied from https://github.com/valantonini/AStar/blob/master/AStar.Core/PriorityQueueB.cs. See LICENSE.txt in this directory for details.
// </remarks>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.PathFinding
{
    using System;

    /// <summary>
    /// A point in a matrix. Pxy where X is the row and Y is the column.
    /// </summary>
    public struct Point : IEquatable<Point>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Point"/> struct.
        /// </summary>
        /// <param name="x">The X Coord.</param>
        /// <param name="y">The Y Coord.</param>
        public Point(int x = 0, int y = 0)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Gets the X Coord.
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Gets the Y coord.
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// Overload for equality.
        /// </summary>
        /// <param name="a">The left point.</param>
        /// <param name="b">The right point.</param>
        /// <returns>True if the two points are the equal.</returns>
        public static bool operator ==(Point a, Point b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// Overload for inequality.
        /// </summary>
        /// <param name="a">The left point.</param>
        /// <param name="b">The right point.</param>
        /// <returns>True if the two points are not equal.</returns>
        public static bool operator !=(Point a, Point b)
        {
            return !a.Equals(b);
        }

        /// <inheritdoc/>
        public bool Equals(Point other)
        {
            return this.X == other.X && this.Y == other.Y;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (obj is Point point)
            {
                return this.Equals(point);
            }

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(this.X, this.Y);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"[{this.X}.{this.Y}]";
        }
    }
}