// -------------------------------------------------------------------------------------------------
// <copyright file="DefinitionEditor.xaml.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using LegendsGenerator.Contracts.Definitions;
    using LegendsGenerator.Editor.ContractParsing;

    /// <summary>
    /// Interaction logic for DefinitionEditor.
    /// </summary>
    public partial class DefinitionEditor : UserControl
    {
        public DefinitionEditor()
        {
            this.InitializeComponent();
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            DefinitionNode? item = e.NewValue as DefinitionNode;

            if (item == null)
            {
                return;
            }

            Context? context = this.DataContext as Context;

            if (context == null)
            {
                throw new InvalidOperationException("DataContext must be Context.");
            }

            context.SelectedNode = item;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            Button? element = sender as Button;

            if (element == null)
            {
                return;
            }

            ICreatable? node = element.DataContext as ICreatable;

            if (node == null)
            {
                throw new InvalidOperationException(
                    $"Datacontext must be of type ICreatable, is {element.DataContext.GetType().Name}");
            }

            node.HandleCreate(sender, e);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button? element = sender as Button;

            if (element == null)
            {
                return;
            }

            IDeletable? node = element.DataContext as IDeletable;

            if (node == null)
            {
                throw new InvalidOperationException(
                    $"Datacontext must be of type IDeletable, is {element.DataContext.GetType().Name}");
            }

            node.HandleDelete(sender, e);
        }
    }
}
