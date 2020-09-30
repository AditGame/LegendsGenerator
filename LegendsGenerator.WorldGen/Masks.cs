// -------------------------------------------------------------------------------------------------
// <copyright file="Masks.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.WorldGen
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Masks to change values based on linear interpolation.
    /// </summary>
    public static class Masks
    {
        /// <summary>
        /// Fades the initial value to 0 on either end, starting at a percentage of the max coord.
        /// </summary>
        /// <param name="fadeStart">The percentage to start the fade on either side.</param>
        /// <param name="coord">The coord to eval.</param>
        /// <param name="maxCoord">The max coord value.</param>
        /// <param name="initialValue">The value to mask.</param>
        /// <returns>A faded value, fading to 0 at either end.</returns>
        public static float FadeToZeroAtEnds(float fadeStart, int coord, int maxCoord, float initialValue)
        {
            if (coord == 0)
            {
                return 0;
            }

            float position = (float)coord / (float)maxCoord;
            float positionInverse = 1.0f - position;

            if (position < fadeStart)
            {
                return Lerp(0, initialValue, position / fadeStart);
            }
            else if (positionInverse < fadeStart)
            {
                return Lerp(0, initialValue, positionInverse / fadeStart);
            }

            return initialValue;
        }

        /// <summary>
        /// Linear interpolation from between two points.
        /// </summary>
        /// <param name="v0">The start value.</param>
        /// <param name="v1">The end value.</param>
        /// <param name="t">A percentage (from 0 to 1) of the values between. Values past/before will be extrapolated.</param>
        /// <returns>The calculated value for the given t value.</returns>
        public static float Lerp(float v0, float v1, float t)
        {
            return v0 + (t * (v1 - v0));
        }
    }
}
