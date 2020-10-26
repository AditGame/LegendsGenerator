// <copyright file="RandomFactory.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator
{
    using System;

    /// <summary>
    /// Creates a random number generated based on inputs, giving a consistent set of random numbers.
    /// </summary>
    public static class RandomFactory
    {
        /// <summary>
        /// Gets the random number generator.
        /// </summary>
        /// <param name="worldSeed">The world seed.</param>
        /// <param name="stepCount">The current step.</param>
        /// <param name="thingId">The ID of the thing.</param>
        /// <returns>A random number generator initialized with the inputted info.</returns>
        public static Random GetRandom(int worldSeed, int stepCount, Guid thingId)
        {
            unchecked
            {
                return new Random(worldSeed * stepCount * thingId.ToString().GetStableHashCode());
            }
        }

        /// <summary>
        /// Gets the random number generator.
        /// </summary>
        /// <param name="worldSeed">The world seed.</param>
        /// <param name="stepCount">The current step.</param>
        /// <param name="thingId">The ID of the thing.</param>
        /// <param name="stage">The stage of this generation.</param>
        /// <returns>A random number generator initialized with the inputted info.</returns>
        public static Random GetRandom(int worldSeed, int stepCount, Guid thingId, RandomStage stage)
        {
            unchecked
            {
                return new Random((worldSeed * stepCount * thingId.ToString().GetStableHashCode()) + (int)stage);
            }
        }
    }
}
