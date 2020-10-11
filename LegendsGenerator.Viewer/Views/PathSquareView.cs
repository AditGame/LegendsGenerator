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
        /// <summary>
        /// Initializes a new instance of the <see cref="PathSquareView"/> class.
        /// </summary>
        /// <param name="x">The X coord.</param>
        /// <param name="y">The Y coord.</param>
        /// <param name="size">The size of the brush, with 1 being the full size of the square.</param>
        /// <param name="color">The color of the square.</param>
        /// <param name="opacity">The opacity of this square.</param>
        public PathSquareView(int x, int y, float size, Brush color, float opacity = 1f)
        {
            this.X = x;
            this.Y = y;
            this.Size = size;
            this.Color = color;
            this.Opacity = opacity;
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
        /// Gets or sets the size of the square as a function of the total size of the square.
        /// </summary>
        public float Size { get; set; }

        /// <summary>
        /// Gets or sets the color of this square in the viewer.
        /// </summary>
        public Brush Color { get; set; }

        /// <summary>
        /// Gets or sets the opacitty of this square.
        /// </summary>
        public float Opacity { get; set; }

        /// <summary>
        /// Gets the X position of this square in the viewer.
        /// </summary>
        public int ViewerX => (this.X * WorldViewer.GridSize) + (int)(WorldViewer.GridSize * (1f - this.Size) / 2);

        /// <summary>
        /// Gets the Y position of this square in the viewer.
        /// </summary>
        public int ViewerY => (this.Y * WorldViewer.GridSize) + (int)(WorldViewer.GridSize * (1f - this.Size) / 2);

        /// <summary>
        /// Gets the width of this square in the viewer.
        /// </summary>
        public int Width => (int)(WorldViewer.GridSize * this.Size);

        /// <summary>
        /// Gets the height of this square in the viewer.
        /// </summary>
        public int Height => (int)(WorldViewer.GridSize * this.Size);
    }
}
