// -------------------------------------------------------------------------------------------------
// <copyright file="StringContractNode.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor.ContractParsing
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// A contract node which is a string.
    /// </summary>
    public class StringDefinitionNode : DefinitionNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringDefinitionNode"/> class.
        /// </summary>
        /// <param name="description">The description of this node.</param>
        /// <param name="name">The name of this node.</param>
        /// <param name="nullable">If this node can be null or not.</param>
        /// <param name="contents">The contents of this node.</param>
        /// <param name="update">An optional function to use to update the contents.</param>
        public StringDefinitionNode(
            string description,
            string name,
            bool nullable,
            Func<string> contents,
            Action<string>? update = null)
            : base(description, nullable)
        {
            this.Name = name;
            this.GetContentsFunc = contents;

            if (update != null)
            {
                this.ContentsModifiable = true;
                this.SetContentsFunc = ConvertAction(update);
            }

            if (this.Name.Contains("Condition"))
            {
                this.ValidateNodeFunc = () => new List<string>()
                {
                    "An issue!",
                };
            }

            this.ValidateNodeFunc = () =>
            {
                IList<string> errors = new List<string>();
                if (string.IsNullOrWhiteSpace(contents()) && !this.Nullable)
                {
                    errors.Add("Can not be null, empty, or whitespace.");
                }

                return errors;
            };
        }

        /// <inheritdoc/>
        public override string Name { get; }

        /// <inheritdoc/>
        public override UIElement GetContentElement()
        {
            TextBox box = new TextBox()
            {
                Text = this.Content as string,
                IsReadOnly = !this.ContentsModifiable,
            };

            box.TextChanged += this.OnTextChanged;

            return box;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            this.Content = (sender as TextBox).Text;
        }
    }
}
