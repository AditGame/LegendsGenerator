// -------------------------------------------------------------------------------------------------
// <copyright file="ICreatable.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor.ContractParsing
{
    using System.Windows;

    /// <summary>
    /// An element which can be create.
    /// </summary>
    public interface ICreatable
    {
        /// <summary>
        /// Gets a value indicating whether creating can be done on this node.
        /// </summary>
        bool CanCreate { get; }

        /// <summary>
        /// Handles create events.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The args.</param>
        void HandleCreate(object sender, RoutedEventArgs e);
    }
}
