// -------------------------------------------------------------------------------------------------
// <copyright file="ICreateDelete.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor
{
    using System.Windows;

    /// <summary>
    /// Interface for items which can be created and deleted.
    /// </summary>
    public interface ICreateDelete
    {
        /// <summary>
        /// Gets a value indicating whether this node can be deleted.
        /// </summary>
        bool CanDelete { get; }

        /// <summary>
        /// Gets a value indicating whether this node can be created.
        /// </summary>
        bool CanCreate { get; }

        /// <summary>
        /// Handles create events.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The args.</param>
#pragma warning disable CA2109 // Review visible event handlers. This library is not externally exposed.
        void HandleCreate(object sender, RoutedEventArgs e);

        /// <summary>
        /// Handles delete events.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The args.</param>
        void HandleDelete(object sender, RoutedEventArgs e);
#pragma warning restore CA2109 // Review visible event handlers
    }
}
