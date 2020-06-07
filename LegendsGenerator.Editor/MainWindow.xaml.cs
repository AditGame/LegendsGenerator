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
        private Context context;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();

            DefinitionCollection? defs = DefinitionSerializer.DeserializeFromDirectory(@"..\..\..\..\LegendsGenerator\Definitions");

            this.context = new Context(defs);
            this.DataContext = this.context;
            this.context.Attach();
        }
    }
}
