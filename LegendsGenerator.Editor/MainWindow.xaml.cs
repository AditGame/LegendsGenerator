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
        public MainWindow()
        {
            InitializeComponent();

            var defs = DefinitionSerializer.DeserializeFromDirectory(@"C:\Users\tt\Source\Repos\LegendsGenerator\LegendsGenerator\Definitions");

            this.DefList.AttachDefinitions(defs);
            this.DefList.Selected += this.ItemSelected;
        }

        public BaseDefinition? CurrentDefinition { get; set; }

        public void ItemSelected(object sender, EventArgs e)
        {
            this.CurrentDefinition = sender as BaseDefinition;

            if (this.CurrentDefinition != null)
            {
                this.EditView.SetDefinition(this.CurrentDefinition);
            }
        }
    }
}
