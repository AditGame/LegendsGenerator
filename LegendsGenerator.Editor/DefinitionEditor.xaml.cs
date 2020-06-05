// -------------------------------------------------------------------------------------------------
// <copyright file="DefinitionEditor.xaml.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    using LegendsGenerator.Editor.ContractParsing;

    /// <summary>
    /// Interaction logic for DefinitionEditor.
    /// </summary>
    public partial class DefinitionEditor : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefinitionEditor"/> class.
        /// </summary>
        public DefinitionEditor()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Fires when a Node is selected.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The arguments.</param>
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

        /// <summary>
        /// Fires when the create button is clicked, routes it to the node.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The arguments.</param>
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

            // Regenerate compilation
            Context? context = this.DataContext as Context;
            if (context == null)
            {
                throw new InvalidOperationException("DataContext msut be Context");
            }

            context.Attach();
        }

        /// <summary>
        /// Fires when the delete button is clicked, routes it to the node.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The arguments.</param>
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

            // Regenerate compilation
            Context? context = this.DataContext as Context;
            if (context == null)
            {
                throw new InvalidOperationException("DataContext msut be Context");
            }

            context.Attach();
        }
    }
}
