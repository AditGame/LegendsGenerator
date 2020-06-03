// -------------------------------------------------------------------------------------------------
// <copyright file="DefinitionEditor.xaml.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor
{
    using System;
    using System.Collections.Generic;
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
        /// <summary>
        /// the current nodes to display.
        /// </summary>
        private IList<DefinitionNode> nodes = new List<DefinitionNode>();

        public DefinitionEditor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets the definition to display/edit.
        /// </summary>
        /// <param name="definition">The definition.</param>
        public void SetDefinition(BaseDefinition definition)
        {
            this.nodes = DefinitionParser.ParseToNodes(definition.GetType(), definition);

            if (this.TreeView.IsLoaded)
            {
                this.UpdateItemList(this.TreeView);
            }
        }

        private void TreeView_Loaded(object sender, RoutedEventArgs e)
        {
            this.UpdateItemList(sender as TreeView);
        }

        private void UpdateItemList(TreeView tree)
        {
            tree.Items.Clear();

            foreach (DefinitionNode subNode in this.nodes)
            {
                tree.Items.Add(ToTreeViewItem(subNode));
            }
        }

        private static TreeViewItem ToTreeViewItem(DefinitionNode node)
        {
            var validationIssues = node.ValidationFailures;
            StackPanel panel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
            };

            TextBlock header = new TextBlock();
            header.Text = $"{node.Name}: ";
            panel.Children.Add(header);
            panel.Children.Add(node.GetContentElement());

            StackPanel tooltip = new StackPanel();
            tooltip.Children.Add(new TextBlock()
            {
                Text = node.Description,
            });

            if (validationIssues.Any())
            {
                header.Foreground = Brushes.DarkRed;

                foreach (string validaitonIssue in validationIssues)
                {
                    tooltip.Children.Add(new TextBlock()
                    {
                        Text = validaitonIssue,
                        Foreground = Brushes.DarkRed,
                    });
                }
            }

            TreeViewItem item = new TreeViewItem()
            {
                Header = panel,
                ToolTip = tooltip,
                IsExpanded = true,
                DataContext = node,
            };

            foreach (DefinitionNode subNode in node.SubNodes)
            {
                item.Items.Add(ToTreeViewItem(subNode));
            }

            return item;
        }
    }
}
