// -------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Viewer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using Gu.Wpf.DataGrid2D;
    using LegendsGenerator.Compiler.CSharp;
    using LegendsGenerator.Contracts.Definitions;
    using LegendsGenerator.Contracts.Definitions.Events;
    using LegendsGenerator.Contracts.Things;
    using LegendsGenerator.Viewer.Views;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// The context of this world.
        /// </summary>
        private Context context;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.context = InitContext();
            this.DataContext = this.context;
        }

        /// <summary>
        /// Initializes the context object.
        /// </summary>
        /// <returns>THe context object.</returns>
        public static Context InitContext()
        {
            var definitions = DefinitionSerializer.DeserializeFromDirectory("Definitions");

            int worldSeed = 915434125;
            Random rdm = new Random(worldSeed);

            ConditionCompiler processor = new ConditionCompiler(new Dictionary<string, object>());
            definitions.Attach(processor);

            ThingFactory factory = new ThingFactory(definitions);
            HistoryMachine history = new HistoryMachine(factory);

            World world = new World()
            {
                WorldSeed = worldSeed,
                StepCount = 1,
                Events = new List<EventDefinition>(definitions.Events),
                Grid = new LegendsGenerator.Grid(20, 20),
            };

            for (int i = 0; i < 100; i++)
            {
                int x = rdm.Next(0, 19);
                int y = rdm.Next(0, 19);
                Site cityInst = factory.CreateSite(rdm, x, y, "City");
                world.Grid.AddThing(cityInst);
                Console.WriteLine($"City created: {cityInst.EffectiveAttribute("Population")} {cityInst.EffectiveAttribute("Evil")}");
            }

            return new Context(history, world);
        }

        /// <summary>
        /// Handles the Open click event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The args.</param>
        private void Open_Click(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// Handles the Save click event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The args.</param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// Handles the Save click event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The args.</param>
        private void PrevStep_Click(object sender, RoutedEventArgs e)
        {
            this.context.CurrentStep -= 1;
        }

        /// <summary>
        /// Handles the Save click event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The args.</param>
        private void PrevStep_Click_10(object sender, RoutedEventArgs e)
        {
            this.context.CurrentStep -= 10;
        }

        /// <summary>
        /// Handles the Save click event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The args.</param>
        private void PrevStep_Click_100(object sender, RoutedEventArgs e)
        {
            this.context.CurrentStep -= 100;
        }

        /// <summary>
        /// Handles the Save click event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The args.</param>
        private void NextStep_Click(object sender, RoutedEventArgs e)
        {
            this.context.CurrentStep += 1;
        }

        /// <summary>
        /// Handles the Save click event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The args.</param>
        private void NextStep_Click_10(object sender, RoutedEventArgs e)
        {
            this.context.CurrentStep += 10;
        }

        /// <summary>
        /// Handles the Save click event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The args.</param>
        private void NextStep_Click_100(object sender, RoutedEventArgs e)
        {
            this.context.CurrentStep += 100;
        }

        /// <summary>
        /// Hanldes the thing selected button.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The args.</param>
        private void Thing_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count != 0)
            {
                this.context.SelectedThing = e.AddedItems[0] as ThingView;
            }
            else
            {
                this.context.SelectedThing = null;
            }
        }

        /// <summary>
        /// Handles the user clicking a cell on the map.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The args.</param>
        private void DataGrid_SelectedCellsChanged(object sender, System.Windows.Controls.SelectedCellsChangedEventArgs e)
        {
            DataGrid grid = sender as DataGrid ?? throw new InvalidOperationException("Unable to convert sender to a datagrid.");

            if (grid.SelectedCells.Any())
            {
                Array2DRowView row =
                    grid.SelectedCells.FirstOrDefault().Item as Gu.Wpf.DataGrid2D.Array2DRowView ?? throw new InvalidOperationException("Unable to convert row to a Array2DRowView.");

                int x = grid.SelectedCells.FirstOrDefault().Column.DisplayIndex;
                int y = row.Index;
                this.context.SelectedSquare = this.context.CurrentWorld.Grid.GetSquare(y, x);
            }
            else
            {
                this.context.SelectedSquare = null;
            }
        }
    }
}
