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
            Button? element = sender as Button;

            if (element == null)
            {
                return;
            }

            PropertyNode? node = element.DataContext as PropertyNode;

            if (node == null)
            {
                throw new InvalidOperationException(
                    $"Datacontext must be of type ICreatable, is {element.DataContext.GetType().Name}");
            }

            node.HandleCreate(sender, e);

            if (node.Content is BaseDefinition def)
            {
                def.Reattach();
            }
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

            PropertyNode? node = element.DataContext as PropertyNode;

            if (node == null)
            {
                throw new InvalidOperationException(
                    $"Datacontext must be of type IDeletable, is {element.DataContext.GetType().Name}");
            }

            node.HandleDelete(sender, e);

            if (node.Content is BaseDefinition def)
            {
                def.Reattach();
            }
        }
    }
}
