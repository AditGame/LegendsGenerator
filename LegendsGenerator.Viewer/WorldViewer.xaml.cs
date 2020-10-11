// -------------------------------------------------------------------------------------------------
// <copyright file="WorldViewer.xaml.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Viewer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;

    using LegendsGenerator.Viewer.Views;

    /// <summary>
    /// Interaction logic for WorldViewer.xaml.
    /// </summary>
    public partial class WorldViewer : UserControl
    {
        /// <summary>
        /// The size of the grid.
        /// </summary>
        public const int GridSize = 20;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldViewer"/> class.
        /// </summary>
        public WorldViewer()
        {
            this.InitializeComponent();

            Context.Instance.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName?.Equals(nameof(Context.Squares)) == true)
                {
                    this.InvalidateVisual();
                }
            };
        }

        /// <inheritdoc/>
        protected override void OnRender(DrawingContext drawingContext)
        {
            foreach (SquareView entry in Context.Instance.Squares)
            {
                drawingContext.DrawRectangle(entry.BackgroundColor, null, new Rect(entry.ViewerX, entry.ViewerY, GridSize, GridSize));

                if (entry.Foreground != null)
                {
                    drawingContext.DrawRectangle(entry.Foreground, null, new Rect(entry.ViewerX, entry.ViewerY, GridSize, GridSize));
                }
            }

            base.OnRender(drawingContext);
        }

        /// <summary>
        /// Handles when a grid object is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The args.</param>
        private void TheWorld_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is not Canvas canvas)
            {
                MessageBox.Show($"Click sender was not a Canvas, was a {sender.GetType()}");
                return;
            }

            Point clicked = Mouse.GetPosition(canvas);

            int gridX = (int)(clicked.X / GridSize);
            int gridY = (int)(clicked.Y / GridSize);

            Context.Instance.Squares.ForEach(s =>
            {
                if (s.X == gridX && s.Y == gridY)
                {
                    s.Selected = true;
                }
                else
                {
                    s.Selected = false;
                }
            });

            Context.Instance.SelectedSquare = Context.Instance.CurrentWorld.Grid.GetSquare(gridX, gridY);
        }
    }
}
