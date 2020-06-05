// -------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor
{
    using System.Windows;
    using LegendsGenerator.Contracts.Definitions;

    /// <summary>
    /// Interaction logic for MainWindow.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// The context with all the data in it.
        /// </summary>
        private Context context = new Context();

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();

            var defs = DefinitionSerializer.DeserializeFromDirectory(@"..\..\..\..\LegendsGenerator\Definitions");

            this.DataContext = this.context;

            foreach (var def in defs.events.Events)
            {
                this.context.Definitions.Add(new Definition(def));
            }

            foreach (var def in defs.definitions.AllDefinitions)
            {
                this.context.Definitions.Add(new Definition(def));
            }
        }
    }
}
