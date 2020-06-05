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

            var (defs, events) = DefinitionSerializer.DeserializeFromDirectory(@"..\..\..\..\LegendsGenerator\Definitions");
            defs.Attach(this.context.Compiler);
            events.Attach(this.context.Compiler);

            this.DataContext = this.context;

            foreach (var def in events.Events)
            {
                this.context.Definitions.Add(new Definition(def));
            }

            foreach (var def in defs.AllDefinitions)
            {
                this.context.Definitions.Add(new Definition(def));
            }
        }
    }
}
