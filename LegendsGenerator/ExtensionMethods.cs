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
        /// Gets a hash code which is consistent of the given input string.
        /// </summary>
        /// <param name="str">The input string.</param>
        /// <returns>A hash code which does not changed between executions.</returns>
        public static int GetStableHashCode(this string str)
        {
            unchecked
            {
                int hash1 = 5381;
                int hash2 = hash1;

                for (int i = 0; i < str.Length && str[i] != '\0'; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ str[i];
                    if (i == str.Length - 1 || str[i + 1] == '\0')
                    {
                        break;
                    }

                    hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
                }

                return hash1 + (hash2 * 1566083941);
            }
        }

        /// <summary>
        /// Turns a single element into itself and additional elements.
        /// </summary>
        /// <typeparam name="T">The source type.</typeparam>
        /// <param name="source">The source list.</param>
        /// <param name="moreElements">Function to select.</param>
        /// <returns>The list.</returns>
        public static IEnumerable<T> SelectWithSelf<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> moreElements)
        {
            foreach (T obj in source)
            {
                yield return obj;

                foreach (T add in moreElements(obj))
                {
                    yield return add;
                }
            }
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
