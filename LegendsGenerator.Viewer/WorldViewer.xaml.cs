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
        /// Initializes a new instance of the <see cref="WorldViewer"/> class.
        /// </summary>
        public WorldViewer()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Handles when a grid object is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The args.</param>
        private void TextBlock_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is not Grid grid || grid.DataContext is not SquareView square)
            {
                return;
            }

            Context.Instance.Squares.ForEach(s => s.Selected = false);
            square.Selected = true;
            Context.Instance.SelectedSquare = Context.Instance.CurrentWorld.Grid.GetSquare(square.X, square.Y);
        }
    }
}
