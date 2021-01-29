// -------------------------------------------------------------------------------------------------
// <copyright file="WorldGrid.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using LegendsGenerator.Contracts.Things;

    /// <summary>
    /// The grid of the world.
    /// </summary>
    public class WorldGrid
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorldGrid"/> class.
        /// </summary>
        /// <param name="width">The max width.</param>
        /// <param name="height">The max height.</param>
        public WorldGrid(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.Squares = new GridSquare[width, height];

            for (int x = 0; x < this.Width; x++)
            {
                for (int y = 0; y < this.Height; y++)
                {
                    this.Squares[x, y] = new GridSquare(x, y);
                }
            }
        }

        /// <summary>
        /// Gets the width of this grid.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Gets the height of this grid.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Gets the grid.
        /// </summary>
        public GridSquare[,] Squares { get; }

        /// <summary>
        /// Gets all grid entries with their coordinates.
        /// </summary>
        /// <returns>Every grid entry.</returns>
        public IEnumerable<(int X, int Y, GridSquare Square)> AllGridEntries
        {
            get
            {
                for (int x = 0; x < this.Width; x++)
                {
                    for (int y = 0; y < this.Height; y++)
                    {
                        yield return (x, y, this.Squares[x, y]);
                    }
                }
            }
        }

        /// <summary>
        /// Clones this grid, without any Things.
        /// </summary>
        /// <returns>A matching grid, devoid of life.</returns>
        public WorldGrid CloneWithoutThings()
        {
            WorldGrid clone = new WorldGrid(this.Width, this.Height);

            for (int x = 0; x < this.Width; x++)
            {
                for (int y = 0; y < this.Height; y++)
                {
                    clone.SetSquare(x, y, this.GetSquare(x, y).CloneWithoutThings());
                }
            }

            return clone;
        }

        /// <summary>
        /// Gets a single grid square.
        /// </summary>
        /// <param name="width">The X coordiante to get.</param>
        /// <param name="height">The Y coordinate to get.</param>
        /// <returns>The matching thing.</returns>
        public GridSquare GetSquare(int width, int height)
        {
            this.ConstrainToGrid(ref width, ref height);
            return this.Squares[width, height];
        }

        /// <summary>
        /// Gets a single grid square.
        /// </summary>
        /// <param name="width">The X coordiante to get.</param>
        /// <param name="height">The Y coordinate to get.</param>
        /// <param name="grid">THe grid to set.</param>
        public void SetSquare(int width, int height, GridSquare grid)
        {
            this.ConstrainToGrid(ref width, ref height);
            this.Squares[width, height] = grid;
        }

        /// <summary>
        /// Adds a thing to the correct grid square.
        /// </summary>
        /// <param name="thing">The thing to add.</param>
        public void AddThing(BaseThing thing)
        {
            if (thing is null)
            {
                throw new ArgumentNullException(nameof(thing));
            }

            this.GetSquare(thing.X, thing.Y).AddThing(thing);
        }

        /// <summary>
        /// Gets all grid entries within range of a specified center.
        /// </summary>
        /// <remarks>The range is expressed as a max X OR Y entry, returning a large square of matching entries. This may be changed in the future.</remarks>
        /// <param name="centerX">The center of the search in Width.</param>
        /// <param name="centerY">The center of the search in Height.</param>
        /// <param name="range">The range, expressed in squares.</param>
        /// <returns>All matching entries.</returns>
        public IEnumerable<(int X, int Y, GridSquare Square)> GetGridEntriesWithinRange(int centerX, int centerY, int range)
        {
            for (int x = Math.Max(0, centerX - range); x <= Math.Min(this.Width - 1, centerX + range); x++)
            {
                for (int y = Math.Max(0, centerY - range); y <= Math.Min(this.Height - 1, centerY + range); y++)
                {
                    yield return (x, y, this.Squares[x, y]);
                }
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var (x, y, square) in this.AllGridEntries)
            {
                sb.AppendLine($"Grid ({x},{y}):");
                sb.AppendLine(square.ToString());
            }

            return sb.ToString();
        }

        /// <summary>
        /// Ensures the provided coords are within range.
        /// </summary>
        /// <param name="width">The X coordiante to check.</param>
        /// <param name="height">The Y coordinate to check.</param>
        private void ConstrainToGrid(ref int width, ref int height)
        {
            if (width >= this.Width)
            {
                width = this.Width - 1;
            }
            else if (width < 0)
            {
                width = 0;
            }

            if (height >= this.Height)
            {
                height = this.Height - 1;
            }
            else if (height < 0)
            {
                height = 0;
            }
        }
    }
}
