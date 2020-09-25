// -------------------------------------------------------------------------------------------------
// <copyright file="ThingViewer.xaml.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Viewer
{
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for ThingViewer.xaml.
    /// </summary>
    public partial class ThingViewer : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThingViewer"/> class.
        /// </summary>
        public ThingViewer()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Increments to the previous thing in this square.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void PreviousThing_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Context.Instance.PreviousThingInSquare();
        }

        /// <summary>
        /// Increments to the next thing in this square.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void NextThing_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Context.Instance.NextThingInSquare();
        }
    }
}
