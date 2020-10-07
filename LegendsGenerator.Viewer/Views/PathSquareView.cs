// -------------------------------------------------------------------------------------------------
// <copyright file="PathSquareView.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Viewer.Views
{
    using System.Windows.Media;

    public class PathSquareView
    {
        public PathSquareView(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public int X { get; set; }

        public int Y { get; set; }

        public int ViewerX => (this.X * WorldViewer.GridSize) + (WorldViewer.GridSize / 4);

        public int ViewerY => (this.Y * WorldViewer.GridSize) + (WorldViewer.GridSize / 4);

        public int Width => WorldViewer.GridSize / 2;

        public int Height => WorldViewer.GridSize / 2;

        public Brush Color => Brushes.PaleVioletRed;
    }
}
