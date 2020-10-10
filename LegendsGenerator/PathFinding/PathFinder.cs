// -------------------------------------------------------------------------------------------------
// <copyright file="PathFinder.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
//
// <remarks>
//     Whole or part copied from https://github.com/valantonini/AStar/blob/master/AStar.Core/PriorityQueueB.cs. See LICENSE.txt in this directory for details.
// </remarks>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.PathFinding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Finds paths to targets.
    /// </summary>
    /// <remarks>The class does a lot of weird stuff for the sake of speed.</remarks>
    public class PathFinder
    {
        /// <summary>
        /// The grid to use to eval path finding.
        /// </summary>
        private readonly GridPoint[,] grid;

        /// <summary>
        /// The list of potential points.
        /// </summary>
        private readonly PriorityQueue<Point> open;

        /// <summary>
        /// The list of points with calculated cost.
        /// </summary>
        private readonly List<PathFinderNode> closed = new List<PathFinderNode>();

        /// <summary>
        /// The grid to use to speed up calculation.
        /// </summary>
        private readonly PathFinderNode[,] mCalcGrid;

        /// <summary>
        /// The available directions to move to from any given point.
        /// </summary>
        private readonly sbyte[,] direction;

        /// <summary>
        /// The options to the pathfinding process.
        /// </summary>
        private PathFinderOptions options;

        /// <summary>
        /// The value to use to indicate a node is in the open list. This will be incremented in every run to avoid needed to reallocate between runs.
        /// </summary>
        private byte openNodeValue = 1;

        /// <summary>
        /// The value to use to indicate a node is in the closed list. This will be incremented in every run to avoid needed to reallocate between runs.
        /// </summary>
        private byte closeNodeValue = 2;

        /// <summary>
        /// The object to lock on.
        /// </summary>
        private object lockObj = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="PathFinder"/> class.
        /// </summary>
        /// <param name="grid">The calculated path finding grid.</param>
        /// <param name="pathFinderOptions">Options to path finding.</param>
        public PathFinder(GridPoint[,] grid, PathFinderOptions? pathFinderOptions = null)
        {
            this.grid = grid;

            if (this.mCalcGrid == null || this.mCalcGrid.GetLength(0) != this.grid.GetLength(0) || this.mCalcGrid.GetLength(1) != this.grid.GetLength(1))
            {
                this.mCalcGrid = new PathFinderNode[this.GridX, this.GridY];
            }

            this.open = new PriorityQueue<Point>(new PointComparer(this.mCalcGrid));

            this.options = pathFinderOptions ?? new PathFinderOptions();

            this.direction = this.options.Diagonals
                    ? new sbyte[,]
                    {
                        { 0, -1 },
                        { 1, 0 },
                        { 0, 1 },
                        { -1, 0 },
                        { 1, -1 },
                        { 1, 1 },
                        { -1, 1 },
                        { -1, -1 },
                    }
                    : new sbyte[,]
                    {
                        { 0, -1 },
                        { 1, 0 },
                        { 0, 1 },
                        { -1, 0 },
                    };
        }

        /// <summary>
        /// Gets the width of the grid.
        /// </summary>
        private ushort GridX => (ushort)this.grid.GetLength(0);

        /// <summary>
        /// Gets the height of the grid.
        /// </summary>
        private ushort GridY => (ushort)this.grid.GetLength(1);

        /// <summary>
        /// Find a path.
        /// </summary>
        /// <param name="start">The start point.</param>
        /// <param name="end">The end point.</param>
        /// <param name="waterCostRatio">The amount water costs based on normal movement.</param>
        /// <returns>The list of path points from start to end, or null if none is possible.</returns>
        public List<PathFinderNode>? FindPath(Point start, Point end, float waterCostRatio)
        {
            lock (this.lockObj)
            {
                var found = false;

                var closedNodeCounter = 0;

                // increment for subsequent runs
                this.openNodeValue += 2;
                this.closeNodeValue += 2;
                this.open.Clear();
                this.closed.Clear();

                this.mCalcGrid[start.X, start.Y].G = 0;
                this.mCalcGrid[start.X, start.Y].H = this.options.HeuristicEstimate;
                this.mCalcGrid[start.X, start.Y].Parent = new Point(start.X, start.Y);
                this.mCalcGrid[start.X, start.Y].Status = this.openNodeValue;

                this.open.Push(start);

                while (this.open.Count > 0)
                {
                    var location = this.open.Pop();

                    // Is it in closed list? means this node was already processed
                    if (this.mCalcGrid[location.X, location.Y].Status == this.closeNodeValue)
                    {
                        continue;
                    }

                    var locationX = location.X;
                    var locationY = location.Y;

                    if (location == end)
                    {
                        this.mCalcGrid[location.X, location.Y].Status = this.closeNodeValue;
                        found = true;
                        break;
                    }

                    if (closedNodeCounter > this.options.SearchLimit)
                    {
                        return null;
                    }

                    // Lets calculate each successors
                    for (var i = 0; i < this.direction.GetLength(0); i++)
                    {
                        // unsigned in case we went out of bounds
                        var newLocationX = (ushort)(locationX + this.direction[i, 0]);
                        var newLocationY = (ushort)(locationY + this.direction[i, 1]);

                        if (newLocationX >= this.GridX || newLocationY >= this.GridY)
                        {
                            continue;
                        }

                        float cost = this.grid[newLocationX, newLocationY].CalcCost(waterCostRatio);

                        // Unpassable?
                        if (cost <= 0)
                        {
                            continue;
                        }

                        float newG;
                        if (this.options.HeavyDiagonals && i > 3)
                        {
                            newG = this.mCalcGrid[location.X, location.Y].G + (cost * 2.41f);
                        }
                        else
                        {
                            newG = this.mCalcGrid[location.X, location.Y].G + cost;
                        }

                        // Is it open or closed?
                        if (this.mCalcGrid[newLocationX, newLocationY].Status == this.openNodeValue || this.mCalcGrid[newLocationX, newLocationY].Status == this.closeNodeValue)
                        {
                            // The current node has less code than the previous? then skip this node
                            if (this.mCalcGrid[newLocationX, newLocationY].G <= newG)
                            {
                                continue;
                            }
                        }

                        this.mCalcGrid[newLocationX, newLocationY].Parent = new Point(locationX, locationY);
                        this.mCalcGrid[newLocationX, newLocationY].G = newG;

                        var h = Heuristic.DetermineH(this.options.Formula, end, this.options.HeuristicEstimate, newLocationY, newLocationX);

                        if (this.options.TieBreaker)
                        {
                            var dx1 = locationX - end.X;
                            var dy1 = locationY - end.Y;
                            var dx2 = start.X - end.X;
                            var dy2 = start.Y - end.Y;
                            var cross = Math.Abs((dx1 * dy2) - (dx2 * dy1));
                            h = (int)(h + (cross * 0.001));
                        }

                        this.mCalcGrid[newLocationX, newLocationY].H = h;

                        this.open.Push(new Point(newLocationX, newLocationY));

                        this.mCalcGrid[newLocationX, newLocationY].Status = this.openNodeValue;
                    }

                    closedNodeCounter++;
                    this.mCalcGrid[location.X, location.Y].Status = this.closeNodeValue;
                }

                // We need to reverse the list and knock off the first square so it's from start to end and does not include the starting square.
                return !found ? null : Enumerable.Reverse(this.OrderClosedListAsPath(end)).Skip(1).ToList();
            }
        }

        /// <summary>
        /// Reorders the list as path.
        /// </summary>
        /// <param name="end">The end point.</param>
        /// <returns>The full path, from end to start.</returns>
        private List<PathFinderNode> OrderClosedListAsPath(Point end)
        {
            this.closed.Clear();

            var fNodeTmp = this.mCalcGrid[end.X, end.Y];

            var fNode = new PathFinderNode
            {
                H = fNodeTmp.H,
                G = fNodeTmp.G,
                Parent = fNodeTmp.Parent,
                X = end.X,
                Y = end.Y,
            };

            while (fNode.X != (fNode.Parent?.X ?? 0) || fNode.Y != (fNode.Parent?.Y ?? 0))
            {
                this.closed.Add(fNode);

                var posX = fNode.Parent?.X ?? 0;
                var posY = fNode.Parent?.Y ?? 0;

                fNodeTmp = this.mCalcGrid[posX, posY];
                fNode.H = fNodeTmp.H;
                fNode.G = fNodeTmp.G;
                fNode.Parent = fNodeTmp.Parent;
                fNode.X = posX;
                fNode.Y = posY;
            }

            this.closed.Add(fNode);

            return this.closed;
        }
    }
}
