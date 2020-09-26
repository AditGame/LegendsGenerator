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
                Grid = new WorldGrid(width, height),
            };

            Noise.Seed = this.Random.Next();
            float[,] elevationMap = Noise.Calc2D(width, height, 0.10f);
            Noise.Seed = this.Random.Next();
            float[,] rainfallMap = Noise.Calc2D(width, height, 0.10f);
            Noise.Seed = this.Random.Next();
            float[,] drainageMap = Noise.Calc2D(width, height, 0.10f);
            Noise.Seed = this.Random.Next();
            float[,] tempertureMap = Noise.Calc2D(width, height, 0.10f);
            Noise.Seed = this.Random.Next();
            float[,] evilMap = Noise.Calc2D(width, height, 0.10f);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int elevation = (int)(elevationMap[x, y] / 255f * 400f);
                    int rainfall = (int)(rainfallMap[x, y] / 255f * 100f);
                    int drainage = (int)(drainageMap[x, y] / 255f * 100f);

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

                    square.Name = square.BaseAttributes["Elevation"].ToString();

                    this.GeneratedWorld.Grid.GetSquare(x, y).AddThing(square);
                }
            }
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
