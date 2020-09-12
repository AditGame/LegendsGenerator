﻿// -------------------------------------------------------------------------------------------------
// <copyright file="Grid.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;

    using LegendsGenerator.Contracts;

    /// <summary>
    /// THe grid of the world.
    /// </summary>
    public class Grid
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Grid"/> class.
        /// </summary>
        /// <param name="width">The max width.</param>
        /// <param name="height">The max height.</param>
        public Grid(int width, int height)
        {
            this.Width = width;
            this.Height = height;
#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional. Multidimensional is more preformant for our use case.
            this.Squares = new GridSquare[width, height];
#pragma warning restore CA1814 // Prefer jagged arrays over multidimensional

            for (int x = 0; x < this.Width; x++)
            {
                for (int y = 0; y < this.Height; y++)
                {
                    this.Squares[x, y] = new GridSquare();
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
#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional. Multidimensional is more preformant for our use case.
        public GridSquare[,] Squares { get; }
#pragma warning restore CA1814 // Prefer jagged arrays over multidimensional

        /// <summary>
        /// Clones this grid, without any Things.
        /// </summary>
        /// <returns>A matching grid, devoid of life.</returns>
        public Grid CloneWithoutThings()
        {
            Grid clone = new Grid(this.Width, this.Height);

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
            this.DebugThrowIfOutOfRange(width, height);
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
            this.DebugThrowIfOutOfRange(width, height);
            this.Squares[width, height] = grid;
        }

        /// <summary>
        /// Adds a thing to the correct grid square.
        /// </summary>
        /// <param name="thing">The thing to add.</param>
        public void AddThing(BaseThing thing)
        {
            this.GetSquare(thing.X, thing.Y).ThingsInGrid.Add(thing);
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

        /// <summary>
        /// Gets all grid entries with their coordinates.
        /// </summary>
        /// <returns>Every grid entry.</returns>
        public IEnumerable<(int X, int Y, GridSquare Square)> GetAllGridEntries()
        {
            for (int x = 0; x < this.Width; x++)
            {
                for (int y = 0; y < this.Height; y++)
                {
                    yield return (x, y, this.Squares[x, y]);
                }
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var (x, y, square) in this.GetAllGridEntries())
            {
                sb.AppendLine($"Grid ({x},{y}):");
                sb.AppendLine(square.ToString());
            }

            return sb.ToString();
        }

        /// <summary>
        /// Throws if the provided coordinates are out of range.
        /// </summary>
        /// <param name="width">The X coordiante to check.</param>
        /// <param name="height">The Y coordinate to check.</param>
        private void DebugThrowIfOutOfRange(int width, int height)
        {
#if DEBUG
            if (width >= this.Width)
            {
                throw new IndexOutOfRangeException($"Attempted to access square with width {width} but grid has width {this.Width}");
            }

            if (height >= this.Height)
            {
                throw new IndexOutOfRangeException($"Attempted to access square with height {height} but grid has height {this.Height}");
            }
#endif
        }
    }
}