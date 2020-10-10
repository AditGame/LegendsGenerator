// -------------------------------------------------------------------------------------------------
// <copyright file="PathSquareView.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Viewer.Views
{
    using System.Windows.Media;

    /// <summary>
    /// A view into a pathing square.
    /// </summary>
    public class PathSquareView
    {
#pragma warning disable CA1822 // Mark members as static. Standard practice of view classes.
        /// <summary>
        /// Initializes a new instance of the <see cref="PathSquareView"/> class.
        /// </summary>
        /// <param name="x">The X coord.</param>
        /// <param name="y">The Y coord.</param>
        public PathSquareView(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Gets or sets the X coord of this square.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the Y coord of this square.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Gets the X position of this square in the viewer.
        /// </summary>
        public int ViewerX => (this.X * WorldViewer.GridSize) + (WorldViewer.GridSize / 4);

        /// <summary>
        /// Gets the Y position of this square in the viewer.
        /// </summary>
        public int ViewerY => (this.Y * WorldViewer.GridSize) + (WorldViewer.GridSize / 4);

        /// <summary>
        /// Gets the width of this square in the viewer.
        /// </summary>
        public int Width => WorldViewer.GridSize / 2;

        /// <summary>
        /// Gets the height of this square in the viewer.
        /// </summary>
        public int Height => WorldViewer.GridSize / 2;

        /// <summary>
        /// Gets the color of this square in the viewer.
        /// </summary>
        public Brush Color => Brushes.PaleVioletRed;
    }
#pragma warning restore CA1822 // Mark members as static
}
