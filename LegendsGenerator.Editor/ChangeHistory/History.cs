// -------------------------------------------------------------------------------------------------
// <copyright file="History.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor.ChangeHistory
{
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// Keeps track of history of changes and applies/unapplies them.
    /// </summary>
    public class History
    {
        /// <summary>
        /// Gets the history items.
        /// </summary>
        public ObservableCollection<HistoryItem> Items { get; } = new ObservableCollection<HistoryItem>();

        /// <summary>
        /// Gets a value indicating whether history can be undone.
        /// </summary>
        public bool CanUndo => this.Items.Any(x => !x.Undone);

        /// <summary>
        /// Gets a value indicating whether history can be redone.
        /// </summary>
        public bool CanRedo => this.Items.Any(x => x.Undone);

        /// <summary>
        /// Undoes the last history item.
        /// </summary>
        public void Undo()
        {
            HistoryItem? item = this.Items.LastOrDefault(x => !x.Undone);

            if (item != null)
            {
                item.Undo();
            }
        }

        /// <summary>
        /// Redoes the last item.
        /// </summary>
        public void Redo()
        {
            HistoryItem? item = this.Items.FirstOrDefault(x => x.Undone);

            if (item != null)
            {
                item.Redo();
            }
        }

        /// <summary>
        /// Adds a new history item.
        /// </summary>
        /// <param name="item">The history item.</param>
        public void AddHistoryItem(HistoryItem item)
        {
            // Detroy the future history to resolve time travel paradoxes.
            foreach (HistoryItem undone in this.Items.Where(x => x.Undone).ToList())
            {
                this.Items.Remove(undone);
            }

            this.Items.Add(item);
        }
    }
}
