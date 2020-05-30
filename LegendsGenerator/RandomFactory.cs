// <copyright file="RandomFactory.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator
{
    using System;

    /// <summary>
    /// Creates a random number generated based on inputs, giving a consistant set of random numbers.
    /// </summary>
    public static class RandomFactory
    {
        /// <summary>
        /// Gets the random nubmer generator.
        /// </summary>
        /// <param name="worldSeed">The world seed.</param>
        /// <param name="stepCount">The current step.</param>
        /// <param name="thingId">The ID of the thing.</param>
        /// <returns>A random nubmer generator initialized with the inputted info.</returns>
        public static Random GetRandom(int worldSeed, int stepCount, Guid thingId)
        {
            unchecked
            {
                return new Random(worldSeed * stepCount * GetStableHashCode(thingId.ToString()));
            }
        }

        /// <summary>
        /// Gets a hash code which is consistant of the given input string.
        /// </summary>
        /// <param name="str">The input string.</param>
        /// <returns>A hash code which does not changed between executions.</returns>
        private static int GetStableHashCode(string str)
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
    }
}
