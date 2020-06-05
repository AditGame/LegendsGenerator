// -------------------------------------------------------------------------------------------------
// <copyright file="IDeletable.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor.ContractParsing
{
    using System.Windows;

    public interface IDeletable
    {
        void HandleDelete(object sender, RoutedEventArgs e);
    }
}
