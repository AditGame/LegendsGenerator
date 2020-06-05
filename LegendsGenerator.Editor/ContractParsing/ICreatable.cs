// -------------------------------------------------------------------------------------------------
// <copyright file="ICreatable.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor.ContractParsing
{
    using System.Windows;

    public interface ICreatable
    {
        void HandleCreate(object sender, RoutedEventArgs e);
    }
}
