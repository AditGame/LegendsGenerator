// <copyright file="ExtensionMethods.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Extension methods.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Shuffles the enumerable's members.
        /// </summary>
        /// <typeparam name="T">THe enumerable type.</typeparam>
        /// <param name="source">The enumerable to shuffle.</param>
        /// <returns>A shuffled enumerable of the source elements.</returns>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.Shuffle(new Random());
        }

        /// <summary>
        /// Shuffles the enumerable's members.
        /// </summary>
        /// <typeparam name="T">THe enumerable type.</typeparam>
        /// <param name="source">The enumerable to shuffle.</param>
        /// <param name="rng">The random number generator.</param>
        /// <returns>A shuffled enumerable of the source elements.</returns>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (rng == null)
            {
                throw new ArgumentNullException(nameof(rng));
            }

            return source.ShuffleIterator(rng);
        }

        /// <summary>
        /// Shuffles the enumerable's members.
        /// </summary>
        /// <typeparam name="T">THe enumerable type.</typeparam>
        /// <param name="source">The enumerable to shuffle.</param>
        /// <param name="rng">The random number generator.</param>
        /// <returns>A shuffled enumerable of the source elements.</returns>
        private static IEnumerable<T> ShuffleIterator<T>(
            this IEnumerable<T> source, Random rng)
        {
            var buffer = source.ToList();
            for (int i = 0; i < buffer.Count; i++)
            {
                int j = rng.Next(i, buffer.Count);
                yield return buffer[j];

                buffer[j] = buffer[i];
            }
        }
    }
}
