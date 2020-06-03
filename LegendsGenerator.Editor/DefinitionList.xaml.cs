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
        private EventCollection events;

        private DefinitionCollections definitions;

        public DefinitionList()
        {
            this.InitializeComponent();
        }

        public delegate void ItemSelected(object sender, EventArgs e);

        // Declare the event.
        public event ItemSelected Selected;

        public void AttachDefinitions((DefinitionCollections, EventCollection) definitions)
        {
            (this.definitions, this.events) = definitions;

            if (!this.TreeView.IsLoaded)
            {
                this.UpdateItemList(this.TreeView);
            }
        }

        private void TreeView_Loaded(object sender, RoutedEventArgs e)
        {
            this.UpdateItemList(sender as TreeView);
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem? item = e.NewValue as TreeViewItem;

            if (item == null)
            {
                return;
            }

            BaseThingDefinition? matchingThing = this.definitions.AllDefinitions.FirstOrDefault(e => e.Name.Equals(item.Header));
            if (matchingThing != null)
            {
                this.Selected.Invoke(matchingThing, new EventArgs());
            }

            EventDefinition? matchingEvent = this.events.Events.FirstOrDefault(e => e.Description.Equals(item.Header));
            if (matchingEvent != null)
            {
                this.Selected.Invoke(matchingEvent, new EventArgs());
            }
        }

        public void UpdateItemList(TreeView tree)
        {
            tree.Items.Clear();

            TreeViewItem eventsList = new TreeViewItem()
            {
                Header = "Events",
            };

            foreach (var eveDef in this.events.Events)
            {
                eventsList.Items.Add(new TreeViewItem()
                {
                    Header = eveDef.Description,
                });
            }

            tree.Items.Add(eventsList);

            this.RenderThingList(this.definitions.SiteDefinitions, "Sites", tree);
        }

        private void RenderThingList(IEnumerable<BaseThingDefinition> things, string name, TreeView view)
        {
            TreeViewItem list = new TreeViewItem()
            {
                Header = name,
            };

            ILookup<string?, BaseThingDefinition> inheritanceList =
                things.ToLookup(s => s.InheritsFrom);

            List<BaseThingDefinition> orphans = new List<BaseThingDefinition>();
            foreach (var item in inheritanceList)
            {
                if (item.Key == null)
                {
                    continue;
                }

                if (!things.Any(x => x.Name.Equals(item.Key)))
                {
                    orphans.AddRange(item);
                }
            }

            if (orphans.Any())
            {
                var orphanItem = new TreeViewItem()
                {
                    Header = "_Orphans_",
                    ToolTip = "Items with missing InheritsFrom definitions",
                };

                foreach (var thing in orphans)
                {
                    var topItem = new TreeViewItem()
                    {
                        Header = thing.Name,
                        ToolTip = thing.Description,
                    };

                    this.AddInherited(topItem, thing, inheritanceList);

                    orphanItem.Items.Add(topItem);
                }

                list.Items.Add(orphanItem);
            }

            foreach (var thing in inheritanceList[null])
            {
                var topItem = new TreeViewItem()
                {
                    Header = thing.Name,
                    ToolTip = thing.Description,
                };

                this.AddInherited(topItem, thing, inheritanceList);

                list.Items.Add(topItem);
            }

            view.Items.Add(list);
        }

        private void AddInherited(TreeViewItem item, BaseThingDefinition thing, ILookup<string?, BaseThingDefinition> inheritanceList)
        {
            foreach (var inh in inheritanceList[thing.Name])
            {
                var inhItem = new TreeViewItem()
                {
                    Header = inh.Name,
                    ToolTip = inh.Description,
                };

                this.AddInherited(inhItem, inh, inheritanceList);

                item.Items.Add(inhItem);
            }
        }
    }
}
