// -------------------------------------------------------------------------------------------------
// <copyright file="PathFinderNode.cs" company="Tom Luppi">
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
    using System.Runtime.InteropServices;

    /// <summary>
    /// Holder class for information needed by A* algorithm.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PathFinderNode : System.IEquatable<PathFinderNode>
    {
        /// <summary>
        /// Gets or sets the X coord.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the Y coord.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Gets or sets the computed distance from the starting point.
        /// </summary>
        public float G { get; set; }

        /// <summary>
        /// Gets or sets the taxicab distance to the target.
        /// </summary>
        public float H { get; set; }

        /// <summary>
        /// Gets the estimated distance from the start to the end via this location.
        /// </summary>
        public float F => this.G + this.H;

        /// <summary>
        /// Gets or sets the parent's coords.
        /// </summary>
        public Point? Parent { get; set; }

        /// <summary>
        /// Gets or sets the status of this node.
        /// </summary>
        public byte Status { get; set; }

        public static bool operator ==(PathFinderNode left, PathFinderNode right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PathFinderNode left, PathFinderNode right)
        {
            return !(left == right);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"({this.X},{this.Y}):{this.G}+{this.H}={this.F}";
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (obj is PathFinderNode node)
            {
                return this.Equals(node);
            }

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(this.X, this.Y, this.G, this.H);
        }

        /// <inheritdoc/>
        public bool Equals(PathFinderNode other)
        {
            return this.X == other.X && this.Y == other.Y && this.H == other.H && this.G == other.G;
        }
    }
}
