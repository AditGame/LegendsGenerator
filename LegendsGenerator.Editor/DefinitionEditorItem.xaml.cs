// -------------------------------------------------------------------------------------------------
// <copyright file="DefinitionEditorItem.xaml.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;
    using LegendsGenerator.Contracts.Definitions;
    using LegendsGenerator.Editor.ContractParsing;

    /// <summary>
    /// Interaction logic for DefinitionEditorItem.
    /// </summary>
    [ContentProperty("AdditionalContent")]
    public partial class DefinitionEditorItem : UserControl
    {
        /// <summary>
        /// The Additional Content property.
        /// </summary>
        public static readonly DependencyProperty AdditionalContentProperty =
            DependencyProperty.Register(
                "AdditionalContent",
                typeof(object),
                typeof(DefinitionEditorItem),
                new PropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the <see cref="DefinitionEditorItem"/> class.
        /// </summary>
        public DefinitionEditorItem()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets or sets additional content for the UserControl.
        /// </summary>
        public object AdditionalContent
        {
            get { return (object)this.GetValue(AdditionalContentProperty); }
            set { this.SetValue(AdditionalContentProperty, value); }
        }

        /// <summary>
        /// Fires when the create button is clicked, routes it to the node.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The arguments.</param>
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button element)
            {
                return;
            }

            if (element.DataContext is not ICreateDelete node)
            {
                throw new InvalidOperationException(
                    $"Datacontext must be of type ICreateDelete, is {element.DataContext.GetType().Name}");
            }

            node.HandleCreate(sender, e);

            // Reattach all nodes so things work.
            Context.Instance?.Attach();
        }

        /// <summary>
        /// Fires when the delete button is clicked, routes it to the node.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The arguments.</param>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button element)
            {
                return;
            }

            if (element.DataContext is not ICreateDelete node)
            {
                throw new InvalidOperationException(
                    $"Datacontext must be of type IDeletable, is {element.DataContext.GetType().Name}");
            }

            node.HandleDelete(sender, e);

            // Reattach all nodes so things work.
            Context.Instance?.Attach();
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
    }
}
