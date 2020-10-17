// -------------------------------------------------------------------------------------------------
// <copyright file="PointPres.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Compiler.CSharp.Presentation
{
    /// <summary>
    /// The presentation of a point.
    /// </summary>
    public class PointPres
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PointPres"/> class.
        /// </summary>
        /// <param name="point">The coord.</param>
        public PointPres((int X, int Y) point)
            : this(point.X, point.Y)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointPres"/> class.
        /// </summary>
        /// <param name="x">The X coord.</param>
        /// <param name="y">The Y coord.</param>
        public PointPres(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Gets the X coord.
        /// </summary>
        public int X { get; }

        /// <summary>
        /// Gets the Y coord.
        /// </summary>
        public int Y { get; }
    }
}
