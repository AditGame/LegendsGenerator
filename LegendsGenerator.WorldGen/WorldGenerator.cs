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
    using LegendsGenerator.WorldGen.Contracts;

    /// <summary>
    /// Generates a world.
    /// </summary>
    public class WorldGenerator
    {
        /// <summary>
        /// The definitions collection to source definitions from.
        /// </summary>
        private readonly IThingFactory thingFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldGenerator"/> class.
        /// </summary>
        /// <param name="thingFactory">The thing factory.</param>
        public WorldGenerator(IThingFactory thingFactory)
        {
            this.Seed = new Random().Next();
            this.Random = new Random(this.Seed);
            this.thingFactory = thingFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldGenerator"/> class.
        /// </summary>
        /// <param name="seed">The seed to use.</param>
        /// <param name="thingFactory">The thing factory.</param>
        public WorldGenerator(int seed, IThingFactory thingFactory)
        {
            this.Seed = seed;
            this.Random = new Random(this.Seed);
            this.thingFactory = thingFactory;
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
        /// Generates the world.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns>The legends generator world.</returns>
        public World GenerateWorld(int width, int height)
        {
            GeneratedWorld world = LandGenerator.Generate(width, height, this.Random);

            return this.ConvertToWorld(world);
        }

        /// <summary>
        /// Converts a generated world into a legends generator world.
        /// </summary>
        /// <param name="generated">The generated world.</param>
        /// <returns>The legends generator world.</returns>
        private World ConvertToWorld(GeneratedWorld generated)
        {
            World world = new World()
            {
                Grid = new WorldGrid(generated.Width, generated.Height),
            };

            for (int x = 0; x < generated.Width; x++)
            {
                for (int y = 0; y < generated.Height; y++)
                {
                    var genSquare = generated.Grid[x, y];

                    WorldSquare square = this.thingFactory.CreateWorldSquare(this.Random, x, y, this.GetDefinitionName(genSquare));

                    // Overwrite the attributes provided by the world generation process.
                    square.BaseAttributes[nameof(genSquare.Elevation)] = genSquare.Elevation;
                    square.BaseAttributes[nameof(genSquare.Drainage)] = genSquare.Drainage;
                    square.BaseAttributes[nameof(genSquare.Rainfall)] = genSquare.Rainfall;
                    square.BaseAttributes[nameof(genSquare.Temperature)] = genSquare.Temperature;
                    square.BaseAttributes[nameof(genSquare.Evil)] = genSquare.Evil;
                    square.BaseAttributes[nameof(genSquare.Savagery)] = genSquare.Savagery;
                    square.BaseAttributes[nameof(genSquare.Materials)] = genSquare.Materials;

                    world.Grid.GetSquare(x, y).SquareDefinition = square;
                }
            }

            return world;
        }

        /// <summary>
        /// Find the correct definition for the input parameters.
        /// </summary>
        /// <param name="square">The square to convert.</param>
        /// <returns>The matching definition.</returns>
        private string GetDefinitionName(GeneratedSquare square)
        {
            WorldSquareDefinition? matchingDefinition = this.thingFactory.Definitions.WorldSquareDefinitions.FirstOrDefault(
                d =>
                    square.Elevation >= d.MinElevation && square.Elevation <= d.MaxElevation &&
                    square.Rainfall >= d.MinRainfall && square.Rainfall <= d.MaxRainfall &&
                    square.Drainage >= d.MinDrainage && square.Drainage <= d.MaxDrainage &&
                    square.Water == d.IsWater && square.SaltWater == d.IsSaltWater);

            if (matchingDefinition == null)
            {
                throw new InvalidOperationException($"No matching definition found for {square}");
            }

            return matchingDefinition.Name;
        }
    }
}
