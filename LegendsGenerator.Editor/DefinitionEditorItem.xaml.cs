// -------------------------------------------------------------------------------------------------
// <copyright file="DefinitionEditorItem.xaml.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;

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
    }
}
