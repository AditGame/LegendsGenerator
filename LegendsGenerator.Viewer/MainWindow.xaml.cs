﻿// -------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Viewer
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;

    using LegendsGenerator.Compiler.CSharp;
    using LegendsGenerator.Contracts;
    using LegendsGenerator.Contracts.Definitions;
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
        private readonly Context context;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            Context.Instance.Dispatcher = this.Dispatcher;
            this.context = InitContext();
            this.DataContext = this.context;
        }

        /// <summary>
        /// Initializes the context object.
        /// </summary>
        /// <returns>THe context object.</returns>
        public static Context InitContext()
        {
            int worldSeed = new Random().Next();

            ConditionCompiler<BaseGlobalVariables> processor =
                new ConditionCompiler<BaseGlobalVariables>(new BaseGlobalVariables() { World = new World() { WorldSeed = worldSeed }, });
            DefinitionsCollection definitions = DefinitionSerializer.DeserializeFromDirectory(processor, "Definitions");
            ThingFactory factory = new ThingFactory(definitions);

            HistoryGenerator history = new HistoryGenerator(factory, definitions);

            WorldGen.WorldGenerator worldGen = new WorldGen.WorldGenerator(worldSeed, factory);
            int width = 200;
            int height = 200;
            World world = worldGen.GenerateWorld(width, height);
            processor.UpdateGlobalVariables(g => g.World = world);

            Random rdm = new Random(worldSeed);
            for (int i = 0; i < 100; i++)
            {
                int x = rdm.Next(0, width - 1);
                int y = rdm.Next(0, height - 1);
                Site cityInst = factory.CreateSite(rdm, x, y, "City");
                world.Grid.AddThing(cityInst);
            }

            foreach (var thing in world.Grid.AllGridEntries.SelectMany(x => x.Square.GetThings()))
            {
                thing.FinalizeConstruction(rdm);
            }

            Context.Instance.Attach(history, world, processor);

            return Context.Instance;
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
        private async void PrevStep_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => this.context.CurrentStep -= 1).ConfigureAwait(false);
        }

        /// <summary>
        /// Handles the Save click event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The args.</param>
        private async void PrevStep_Click_10(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => this.context.CurrentStep -= 10).ConfigureAwait(false);
        }

        /// <summary>
        /// Handles the Save click event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The args.</param>
        private async void PrevStep_Click_100(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => this.context.CurrentStep -= 100).ConfigureAwait(false);
        }

        /// <summary>
        /// Handles the Save click event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The args.</param>
        private async void NextStep_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => this.context.CurrentStep += 1).ConfigureAwait(false);
        }

        /// <summary>
        /// Handles the Save click event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The args.</param>
        private async void NextStep_Click_10(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => this.context.CurrentStep += 10).ConfigureAwait(false);
        }

        /// <summary>
        /// Handles the Save click event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The args.</param>
        private async void NextStep_Click_100(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => this.context.CurrentStep += 100).ConfigureAwait(false);
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
        /// Handles the clicking of the cancel activation button.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The args.</param>
        private void CancelGeneration_Click(object sender, RoutedEventArgs e)
        {
            Context.Instance.HistoryGenerationCancellationRequested = true;
        }

        /// <summary>
        /// Handles when a graveyard entry is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The args.</param>
        private void GraveyardClicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement(sender as ItemsControl, e.OriginalSource as DependencyObject) as ListBoxItem;
            if (item?.Content is GraveyardEntryView entry)
            {
                if (entry.Thing.HasLocation)
                {
                    foreach (SquareView s in Context.Instance.Squares)
                    {
                        if (s.X == entry.Thing.X && s.Y == entry.Thing.Y)
                        {
                            s.Selected = true;
                        }
                        else
                        {
                            s.Selected = false;
                        }
                    }

                    Context.Instance.SelectedSquare = Context.Instance.CurrentWorld.Grid.GetSquare(entry.Thing.X, entry.Thing.Y);
                }

                Context.Instance.SelectedThing = entry.Thing;
                Context.Instance.SelectedTabIndex = 0;

                e.Handled = true;
            }
        }
    }
}
