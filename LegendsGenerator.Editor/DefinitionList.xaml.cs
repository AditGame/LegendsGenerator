// -------------------------------------------------------------------------------------------------
// <copyright file="DefinitionList.xaml.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor
{
    using LegendsGenerator.Contracts;
    using LegendsGenerator.Contracts.Definitions;
    using LegendsGenerator.Contracts.Definitions.Events;

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
    /// Interaction logic for DefinitionList.
    /// </summary>
    public partial class DefinitionList : UserControl
    {
        public DefinitionList()
        {
            this.InitializeComponent();
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            InheritanceNode? item = e.NewValue as InheritanceNode;

            if (item == null)
            {
                return;
            }

            Context? context = this.DataContext as Context;

            if (context == null)
            {
                throw new InvalidOperationException("DataContext must be Context.");
            }

            if (item.Definition != null)
            {
                context.SelectedDefinition = item.Definition;
            }
        }
    }
}
