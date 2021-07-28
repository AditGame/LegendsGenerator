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
            if (e.NewValue is not PropertyNode item)
            {
                return;
            }

            if (this.DataContext is not Context context)
            {
                throw new InvalidOperationException("DataContext must be Context.");
            }

            context.SelectedNode = item;
        }

        /// <summary>
        /// Continues the focus event to the underlying TreeViewItem.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The args.</param>
        private void PassFocusToTreeItem(object sender, RoutedEventArgs e)
        {
            if (sender is not DependencyObject dep)
            {
                return;
            }

            DependencyObject obj = System.Windows.Media.VisualTreeHelper.GetParent(dep);
            while (obj is not TreeViewItem)
            {
                obj = System.Windows.Media.VisualTreeHelper.GetParent(obj);
            }

            TreeViewItem item = (obj as TreeViewItem)!;
            item.IsSelected = true;
        }

        /// <summary>
        /// Continues the focus event to the underlying TreeViewItem.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The args.</param>
        private void Undo(object sender, RoutedEventArgs e)
        {
            if (sender is not DependencyObject dep)
            {
                return;
            }

            DependencyObject obj = System.Windows.Media.VisualTreeHelper.GetParent(dep);
            while (obj is not TreeViewItem)
            {
                obj = System.Windows.Media.VisualTreeHelper.GetParent(obj);
            }

            TreeViewItem item = (obj as TreeViewItem)!;
            item.IsSelected = true;
        }
    }
}
