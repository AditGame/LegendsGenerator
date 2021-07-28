// -------------------------------------------------------------------------------------------------
// <copyright file="Heuristic.cs" company="Tom Luppi">
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

    /// <summary>
    /// The formula to use.
    /// </summary>
    public enum HeuristicFormula
    {
        /// <summary>
        /// AN unknown formula, will throw if set.
        /// </summary>
        Unknown,

        /// <summary>
        /// AKA Taxicab distance, assumes no diagonals.
        /// </summary>
        Manhattan,

        /// <summary>
        /// Either difference in the Y coord, or difference in the X coord, whichever is larger.
        /// </summary>
        MaxDXDY,

        /// <summary>
        /// Assumes diagonals are free.
        /// </summary>
        DiagonalShortCut,

        /// <summary>
        /// Straight line distance to the target.
        /// </summary>
        Euclidean,

        /// <summary>
        /// Straight line distance to the target, without the final (expensive) sqrt.
        /// </summary>
        EuclideanNoSQR,
    }

    /// <summary>
    /// The heuristic to use to calculate estimated distance to target.
    /// </summary>
    public static class Heuristic
    {
        /// <summary>
        /// Determines an estimated H value.
        /// </summary>
        /// <param name="heuristicFormula">The formula to use.</param>
        /// <param name="end">The destination to work towards.</param>
        /// <param name="heuristicEstimate">The weight to apply to the heuristic (default 1).</param>
        /// <param name="newLocationY">The starting location.</param>
        /// <param name="newLocationX">The ending location.</param>
        /// <returns>An estimate to the distance to the end.</returns>
        public static int DetermineH(HeuristicFormula heuristicFormula, Point end, int heuristicEstimate, int newLocationY, int newLocationX)
        {
            int h;

            switch (heuristicFormula)
            {
                case HeuristicFormula.MaxDXDY:
                    h = heuristicEstimate * Math.Max(Math.Abs(newLocationX - end.X), Math.Abs(newLocationY - end.Y));
                    break;

                case HeuristicFormula.DiagonalShortCut:
                    var hDiagonal = Math.Min(
                        Math.Abs(newLocationX - end.X),
                        Math.Abs(newLocationY - end.Y));
                    var hStraight = Math.Abs(newLocationX - end.X) + Math.Abs(newLocationY - end.Y);
                    h = (heuristicEstimate * 2 * hDiagonal) + (heuristicEstimate * (hStraight - (2 * hDiagonal)));
                    break;

                case HeuristicFormula.Euclidean:
                    h = (int)(heuristicEstimate * Math.Sqrt(Math.Pow(newLocationY - end.X, 2) + Math.Pow(newLocationY - end.Y, 2)));
                    break;

                case HeuristicFormula.EuclideanNoSQR:
                    h = (int)(heuristicEstimate * (Math.Pow(newLocationX - end.X, 2) + Math.Pow(newLocationY - end.Y, 2)));
                    break;

                case HeuristicFormula.Manhattan:
                    h = heuristicEstimate * (Math.Abs(newLocationX - end.X) + Math.Abs(newLocationY - end.Y));
                    break;
                default:
                    throw new InvalidOperationException($"Unrecognized pathfinding heuristic {heuristicFormula}.");
            }

            return h;
        }
    }
}
