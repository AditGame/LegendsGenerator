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
    using LegendsGenerator.Contracts.Definitions;
    using Ookii.Dialogs.Wpf;

    /// <summary>
    /// Interaction logic for MainWindow.
    /// </summary>
    public partial class MainWindow : Window
    {
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

            this.context = new Context();
            this.DataContext = this.context;
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
                this.context.SetDefinitions(dialog.SelectedPath);
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
                    new DefinitionCollection(this.context.Definitions.Select(x => x.BaseDefinition)));
            }
#pragma warning disable CA1031 // Do not catch general exception types. Inteional to ensure data is not lost.
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
