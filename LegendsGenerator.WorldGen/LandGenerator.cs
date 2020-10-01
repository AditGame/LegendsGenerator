// -------------------------------------------------------------------------------------------------
// <copyright file="LandGenerator.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.WorldGen
{
    using System;

    using LegendsGenerator.WorldGen.Contracts;
    using SimplexNoise;

    /// <summary>
    /// Generates the land.
    /// </summary>
    public static class LandGenerator
    {
        /// <summary>
        /// Generates the land.
        /// </summary>
        /// <param name="width">The width of the generated world.</param>
        /// <param name="height">The height of the generated world.</param>
        /// <param name="rdm">The random number generator.</param>
        /// <returns>The generated world.</returns>
        public static GeneratedWorld Generate(int width, int height, Random rdm)
        {
            GeneratedWorld world = new GeneratedWorld(width, height);

            Noise.Seed = rdm.Next();
            float[,] elevationMap = Noise.Calc2D(width, height, 0.05f);
            Noise.Seed = rdm.Next();
            float[,] rainfallMap = Noise.Calc2D(width, height, 0.05f);
            Noise.Seed = rdm.Next();
            float[,] drainageMap = Noise.Calc2D(width, height, 0.05f);
            Noise.Seed = rdm.Next();
            float[,] tempertureMap = Noise.Calc2D(width, height, 0.05f);
            Noise.Seed = rdm.Next();
            float[,] evilMap = Noise.Calc2D(width, height, 0.05f);
            Noise.Seed = rdm.Next();
            float[,] savageMap = Noise.Calc2D(width, height, 0.05f);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int elevation = (int)(ElevationMask(elevationMap[x, y], x, y, width, height) / 255f * 400f);
                    int rainfall = (int)(rainfallMap[x, y] / 255f * 100f);
                    int drainage = (int)(drainageMap[x, y] / 255f * 100f);
                    int temperture = (int)TempertureMask(tempertureMap[x, y], y, height);
                    int evil = (int)(evilMap[x, y] / 255f * 100f);
                    int savagery = (int)(savageMap[x, y] / 255f * 100f);

                    world.Grid[x, y] = new GeneratedSquare()
                    {
                        Elevation = elevation,
                        Rainfall = rainfall,
                        Drainage = drainage,
                        Temperature = temperture,
                        Evil = evil,
                        Savagery = savagery,
                        Water = elevation < 100,
                    };
                }
            }

            // Convert to ocean on both sides of the world.
            ConvertToOcean(world, 0, height / 2);
            ConvertToOcean(world, width, height / 2);

            return world;
        }

        /// <summary>
        /// Converts a square, and adjacent lake squares, into an ocean.
        /// </summary>
        /// <param name="world">The world.</param>
        /// <param name="x">The X coord.</param>
        /// <param name="y">The Y coord.</param>
        private static void ConvertToOcean(GeneratedWorld world, int x, int y)
        {
            if (x < 0 || x >= world.Width || y < 0 || y >= world.Height)
            {
                return;
            }

            GeneratedSquare square = world.Grid[x, y];

            if (!square.Water || square.SaltWater)
            {
                return;
            }

            square.SaltWater = true;

            ConvertToOcean(world, x - 1, y);
            ConvertToOcean(world, x + 1, y);
            ConvertToOcean(world, x, y - 1);
            ConvertToOcean(world, x, y + 1);
        }

        /// <summary>
        /// A mask to make elevation look right.
        /// </summary>
        /// <param name="initalValue">The input value.</param>
        /// <param name="x">The X coord of this value.</param>
        /// <param name="y">The Y coord of this value.</param>
        /// <param name="maxX">The width of the world.</param>
        /// <param name="maxY">The height of the world.</param>
        /// <returns>The elevation to use.</returns>
        private static float ElevationMask(float initalValue, int x, int y, int maxX, int maxY)
        {
            const float ColdFadeStart = 0.20f;

            return
                Masks.FadeToZeroAtEnds(
                    ColdFadeStart,
                    y,
                    maxY,
                    Masks.FadeToZeroAtEnds(
                        ColdFadeStart,
                        x,
                        maxX,
                        initalValue));
        }

        /// <summary>
        /// A mask to make temperature look right.
        /// </summary>
        /// <param name="mapValue">The input value.</param>
        /// <param name="y">The Y Coord of this value.</param>
        /// <param name="maxY">The height of the world.</param>
        /// <returns>The elevation to use.</returns>
        private static float TempertureMask(float mapValue, int y, int maxY)
        {
            const int MinTemp = 0;
            const int MaxTemp = 80;

            float initialValue = (mapValue / 255f * 40f) - 20f;

            float position = (float)y / (float)maxY * 2;
            float positionInverse = (1.0f - ((float)y / (float)maxY)) * 2;

            float baseValue = position <= 1.0f ? Masks.Lerp(MinTemp, MaxTemp, position) : Masks.Lerp(MinTemp, MaxTemp, positionInverse);

            return baseValue + initialValue;
        }
    }
}
