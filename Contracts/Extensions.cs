// -------------------------------------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// Useful extensions.
    /// </summary>
    internal static class Extensions
    {
        /// <summary>
        /// COnverts an enumerable to a collection.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="obj">The enumerable.</param>
        /// <returns>A collection matching the enumerable.</returns>
        public static Collection<T> ToCollection<T>(this IEnumerable<T> obj)
        {
            return new Collection<T>(obj.ToList());
        }

        /// <summary>
        /// Adds an enumerable into a collection.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="obj">The collection to add to.</param>
        /// <param name="input">The enumerable to add.</param>
        public static void AddRange<T>(this ICollection<T> obj, IEnumerable<T> input)
        {
            foreach (T item in input)
            {
                obj.Add(item);
            }
        }
    }
}
