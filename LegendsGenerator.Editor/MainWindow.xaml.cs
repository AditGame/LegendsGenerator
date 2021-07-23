// -------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Forms;
    using Jot;
    using LegendsGenerator.Contracts.Definitions;
    using Ookii.Dialogs.Wpf;

    /// <summary>
    /// Interaction logic for MainWindow.
    /// </summary>s
    public partial class MainWindow : Window
    {
        /// <summary>
        /// The tracker for state.
        /// </summary>
        private static readonly Tracker Tracker = new Tracker();

        /// <summary>
        /// The context with all the data in it.
        /// </summary>
        private readonly Context context;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();

            Tracker.Configure<Window>()
                .Id(w => w.Name, SystemInformation.VirtualScreen.Size)
                .Properties(w => new { w.Top, w.Width, w.Height, w.Left, w.WindowState })
                .PersistOn(nameof(Window.Closing))
                .StopTrackingOn(nameof(Window.Closing));

            Tracker.Configure<Context>()
                .Properties(c => new
                {
                    c.OpenedDirectory,
                    c.DefinitionFileFilter,
                })
                .PersistOn(nameof(Window.Closing), this);

            this.context = new Context();
            this.DataContext = this.context;

            Tracker.Track(this);
            Tracker.Track(this.context);
        }

        /// <summary>
        /// Handles the Open click event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The args.</param>
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                this.context.OpenedDirectory = dialog.SelectedPath;
            }

            Console.WriteLine(dialog.SelectedPath);
        }

        /// <summary>
        /// Handles the Save click event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The args.</param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DefinitionSerializer.ReserializeToFiles(
                    new DefinitionsCollection(this.context.Definitions.Select(x => x.BaseDefinition)));
            }
#pragma warning disable CA1031 // Do not catch general exception types. Intentional to ensure data is not lost.
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
        }
    }
}
