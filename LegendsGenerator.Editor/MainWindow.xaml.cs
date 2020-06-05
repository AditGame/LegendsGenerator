// -------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor
{
    using LegendsGenerator.Contracts.Definitions;

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

    /// <summary>
    /// Interaction logic for MainWindow.
    /// </summary>
    public partial class MainWindow : Window
    {
        private Context context = new Context();

        public MainWindow()
        {
            this.InitializeComponent();

            var defs = DefinitionSerializer.DeserializeFromDirectory(@"C:\Users\tt\Source\Repos\LegendsGenerator\LegendsGenerator\Definitions");

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

        public void ItemSelected(object sender, EventArgs e)
        {
            this.context.SelectedDefinition = this.context.Definitions[new Random().Next(0, this.context.Definitions.Count())];
        }
    }
}
