// -------------------------------------------------------------------------------------------------
// <copyright file="WorldGenerator.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.WorldGen
{
    using System;
    using System.Linq;
    using LegendsGenerator.Contracts;
    using LegendsGenerator.Contracts.Definitions;
    using LegendsGenerator.Contracts.Things;
    using SimplexNoise;

    /// <summary>
    /// Generates a world.
    /// </summary>
    public class WorldGenerator
    {
        /// <summary>
        /// The definitions collection to source definitions from.
        /// </summary>
        private DefinitionCollection definitions;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldGenerator"/> class.
        /// </summary>
        /// <param name="definitions">The definition collection.</param>
        public WorldGenerator(DefinitionCollection definitions)
        {
            this.Seed = new Random().Next();
            this.Random = new Random(this.Seed);
            this.definitions = definitions;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldGenerator"/> class.
        /// </summary>
        /// <param name="seed">The seed to use.</param>
        /// <param name="definitions">The definition collection.</param>
        public WorldGenerator(int seed, DefinitionCollection definitions)
        {
            this.Seed = seed;
            this.Random = new Random(this.Seed);
            this.definitions = definitions;
        }

        /// <summary>
        /// Gets the seed used to generate this world.
        /// </summary>
        public int Seed { get; }

        /// <summary>
        /// Gets the random number generator used to generate this world.
        /// </summary>
        public Random Random { get; }

        /// <summary>
        /// Gets the generated world.
        /// </summary>
        public World GeneratedWorld { get; private set; } = new World();

        /// <summary>
        /// Generates the world.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public void GenerateWorld(int width, int height)
        {
            this.GeneratedWorld = this.GeneratedWorld with
            {
#pragma warning disable SA1101 // Prefix local calls with this. New C#9 feature.
                Grid = new WorldGrid(width, height),
#pragma warning restore SA1101 // Prefix local calls with this
            };

            Noise.Seed = this.Random.Next();
            float[,] elevationMap = Noise.Calc2D(width, height, 0.05f);
            Noise.Seed = this.Random.Next();
            float[,] rainfallMap = Noise.Calc2D(width, height, 0.05f);
            Noise.Seed = this.Random.Next();
            float[,] drainageMap = Noise.Calc2D(width, height, 0.05f);
            Noise.Seed = this.Random.Next();
            float[,] tempertureMap = Noise.Calc2D(width, height, 0.05f);
            Noise.Seed = this.Random.Next();
            float[,] evilMap = Noise.Calc2D(width, height, 0.05f);
            Noise.Seed = this.Random.Next();
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

                    WorldSquare square = new WorldSquare()
                    {
                        Name = $"{x},{y}",
                        X = x,
                        Y = y,
                        BaseDefinition = this.GetDefinition(elevation, rainfall, drainage),
                    };

                    square.BaseAttributes["Elevation"] = elevation;
                    square.BaseAttributes["Rainfall"] = rainfall;
                    square.BaseAttributes["Drainage"] = drainage;
                    square.BaseAttributes["Temperature"] = temperture;
                    square.BaseAttributes["Evil"] = evil;
                    square.BaseAttributes["Savagery"] = savagery;

                    square.Name = square.BaseAttributes["Elevation"].ToString();

                    this.GeneratedWorld.Grid.GetSquare(x, y).AddThing(square);
                }
            }

            // Convert tiles which connect to the edge to ocean.
            WorldSquareDefinition ocean = this.definitions.WorldSquareDefinitions.First(d => d.Name.Equals("Ocean"));
            WorldSquareDefinition lake = this.definitions.WorldSquareDefinitions.First(d => d.Name.Equals("Lake"));
            ConvertToOcean(ocean, lake, this.GeneratedWorld.Grid, 0, 0);
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
        /// A mask to make temperture look right.
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

        /// <summary>
        /// Converts a square, and adjacent lake squares, into an ocean.
        /// </summary>
        /// <param name="ocean">The ocean definition.</param>
        /// <param name="lake">The lake definition.</param>
        /// <param name="grid">The grid.</param>
        /// <param name="x">The X coord.</param>
        /// <param name="y">The Y coord.</param>
        private static void ConvertToOcean(WorldSquareDefinition ocean, WorldSquareDefinition lake, WorldGrid grid, int x, int y)
        {
            GridSquare? square = grid.GetSquare(x, y);
            if (square.SquareDefinition == null || square.SquareDefinition?.Definition != lake)
            {
                return;
            }

            square.SquareDefinition.BaseDefinition = ocean;

            ConvertToOcean(ocean, lake, grid, x - 1, y);
            ConvertToOcean(ocean, lake, grid, x + 1, y);
            ConvertToOcean(ocean, lake, grid, x, y - 1);
            ConvertToOcean(ocean, lake, grid, x, y + 1);
        }

        /// <summary>
        /// Find the correct definition for the input parameters.
        /// </summary>
        /// <param name="elevation">The elevation.</param>
        /// <param name="rainfall">The rainfall.</param>
        /// <param name="drainage">The drainage.</param>
        /// <returns>The matching definition.</returns>
        private WorldSquareDefinition GetDefinition(int elevation, int rainfall, int drainage)
        {
            WorldSquareDefinition? matchingDefinition = this.definitions.WorldSquareDefinitions.FirstOrDefault(
                d =>
                    elevation >= d.MinElevation && elevation <= d.MaxElevation &&
                    rainfall >= d.MinRainfall && rainfall <= d.MaxRainfall &&
                    drainage >= d.MinDrainage && drainage <= d.MaxDrainage);

            if (matchingDefinition == null)
            {
                throw new InvalidOperationException($"No matching definition found for elevation:{elevation} rainfall:{rainfall} drainage:{drainage}");
            }

            return matchingDefinition;
        }
    }
}
