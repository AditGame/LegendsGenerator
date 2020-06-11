// -------------------------------------------------------------------------------------------------
// <copyright file="HistoryItem.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor.ChangeHistory
{
    using System.ComponentModel;

    /// <summary>
    /// An item of history.
    /// </summary>
    public abstract class HistoryItem : INotifyPropertyChanged
    {
        /// <summary>
        /// Backing property to Undone property.
        /// </summary>
        private bool undone;

        /// <summary>
        /// Fires when a property is changed.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets the name of what is affects.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the string representation of the previous value.
        /// </summary>
        public abstract string PreviousString { get; }

        /// <summary>
        /// Gets the string representation of the next value.
        /// </summary>
        public abstract string NextString { get; }

        /// <summary>
        /// Gets a value indicating whether this item has been undone.
        /// </summary>
        public bool Undone
        {
            get => this.undone;

            private set
            {
                this.undone = value;
                this.OnPropertyChanged(nameof(this.Undone));
                this.OnPropertyChanged(nameof(this.NotUndone));
            }
        }

        /// <summary>
        /// Gets a value indicating whether this item has been undone.
        /// </summary>
        public bool NotUndone
        {
            get => !this.Undone;
        }

        /// <summary>
        /// Undoes this history item.
        /// </summary>
        public void Undo()
        {
            this.UndoInner();
            this.Undone = true;
        }

        /// <summary>
        /// Redoes this history item.
        /// </summary>
        public void Redo()
        {
            this.RedoInner();
            this.Undone = false;
        }

        /// <summary>
        /// Inner function to undo.
        /// </summary>
        protected abstract void UndoInner();

        /// <summary>
        /// Inner function to redo.
        /// </summary>
        protected abstract void RedoInner();

        /// <summary>
        /// Fires property changed events.
        /// </summary>
        /// <param name="propertyName">True when property changed.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
