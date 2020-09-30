// -------------------------------------------------------------------------------------------------
// <copyright file="LineView.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Viewer.Views
{
    using System.Drawing;

    /// <summary>
    /// The view into a line which is displayed on the map.
    /// </summary>
    public class LineView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LineView"/> class.
        /// </summary>
        /// <param name="startX">The start of the line's X coord.</param>
        /// <param name="startY">The start of the line's Y coord.</param>
        /// <param name="endX">The end of the line's X coord.</param>
        /// <param name="endY">The end of the line's Y coord.</param>
        public LineView(int startX, int startY, int endX, int endY)
            : this(new Point(startX, startY), new Point(endX, endY))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineView"/> class.
        /// </summary>
        /// <param name="start">The starting point.</param>
        /// <param name="end">The ending point.</param>
        public LineView(Point start, Point end)
        {
            this.Start = start;
            this.End = end;
        }

        /// <summary>
        /// Gets the start point of the line.
        /// </summary>
        public Point Start { get; }

        /// <summary>
        /// Gets the end point of the line.
        /// </summary>
        public Point End { get; }

        /// <summary>
        /// Gets the starting point of the line in viewer coords.
        /// </summary>
        public Point ViewerStart => new Point((this.Start.X * 20) + 10, (this.Start.Y * 20) + 10);

        /// <summary>
        /// Gets the endpoint of the line in viewer coords.
        /// </summary>
        public Point ViewerEnd => new Point((this.End.X * 20) + 10, (this.End.Y * 20) + 10);
    }
}
