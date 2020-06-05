// -------------------------------------------------------------------------------------------------
// <copyright file="IDeletable.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor.ContractParsing
{
    using System.Windows;

    /// <summary>
    /// Nodes which can be deleted.
    /// </summary>
    public interface IDeletable
    {
        /// <summary>
        /// Handles delete events.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The args.</param>
        void HandleDelete(object sender, RoutedEventArgs e);
    }
}
