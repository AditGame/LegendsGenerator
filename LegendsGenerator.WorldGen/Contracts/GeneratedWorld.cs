// -------------------------------------------------------------------------------------------------
// <copyright file="GeneratedWorld.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.WorldGen.Contracts
{
    /// <summary>
    /// The generated world.
    /// </summary>
    public class GeneratedWorld
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeneratedWorld"/> class.
        /// </summary>
        /// <param name="width">The width of the world.</param>
        /// <param name="height">The height of the world.</param>
        public GeneratedWorld(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.Grid = new GeneratedSquare[width, height];
        }

        /// <summary>
        /// Gets the grid of squares.
        /// </summary>
        public GeneratedSquare[,] Grid { get; }

        /// <summary>
        /// Gets the width of this world.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Gets the height of the world.
        /// </summary>
        public int Height { get; }
    }
}
