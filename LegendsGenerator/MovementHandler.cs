// -------------------------------------------------------------------------------------------------
// <copyright file="MovementHandler.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;

    using LegendsGenerator.Contracts;
    using LegendsGenerator.Contracts.Things;
    using LegendsGenerator.PathFinding;

    /// <summary>
    /// State machine to handle movement.
    /// </summary>
    public partial class MovementHandler
    {
        /// <summary>
        /// The world.
        /// </summary>
        private World world;

        /// <summary>
        /// The random number generator.
        /// </summary>
        private Random random;

        /// <summary>
        /// The pathfinder for A* calculations.
        /// </summary>
        private PathFinder finder;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovementHandler"/> class.
        /// </summary>
        /// <param name="rdm">Random number generator.</param>
        /// <param name="world">The world.</param>
        public MovementHandler(Random rdm, World world)
        {
            this.random = rdm;
            this.world = world;

            // We need to "bake" the path
            GridPoint[,] pathGrid = new GridPoint[world.Grid.Width, world.Grid.Width];
            for (int x = 0; x < world.Grid.Width; x++)
            {
                for (int y = 0; y < world.Grid.Height; y++)
                {
                    pathGrid[x, y].Cost = world.Grid.GetSquare(x, y).GetMovementCost(rdm);
                    pathGrid[x, y].IsWater = world.Grid.GetSquare(x, y).SquareDefinition?.Definition.IsWater ?? false;
                }
            }

            this.finder = new PathFinder(pathGrid);
        }

        /// <summary>
        /// Completes the movement onto the specified thing.
        /// </summary>
        /// <param name="newThing">The new version of the inputted thing.</param>
        public void ApplyMovement(BaseMovingThing newThing)
        {
            bool canFly = newThing.MovingThingDefinition.EvalFlies(this.random, newThing);
            float landSpeed = newThing.MovingThingDefinition.EvalLandSpeed(this.random, newThing);
            float waterSpeed = newThing.MovingThingDefinition.EvalWaterSpeed(this.random, newThing);

            float waterSpeedRatio = 1;
            if (!canFly)
            {
                waterSpeedRatio = landSpeed / waterSpeed;
            }

            int startX = newThing.X;
            int startY = newThing.Y;

            int destinationX, destinationY;
            Guid? thingMovingTowards = null;
            switch (newThing.MoveType)
            {
                case MoveType.ToCoords:
                    destinationX = newThing.MoveToCoordX ?? throw new InvalidOperationException("Object is moving towards a coord but MoveToCoordX is null.");
                    destinationY = newThing.MoveToCoordY ?? throw new InvalidOperationException("Object is moving towards a coord but MoveToCoordY is null.");
                    break;
                case MoveType.ToThing:
                    BaseThing thingToMoveTo =
                        this.world.FindThing(newThing.MoveToThing ?? throw new InvalidOperationException("Object is moving towards a thing but MoveToThing is null."));
                    destinationX = thingToMoveTo.X;
                    destinationY = thingToMoveTo.Y;
                    thingMovingTowards = newThing.MoveToThing;
                    break;
                default:
                    throw new InvalidOperationException($"Unsupported movetype {newThing.MoveType}");
            }

            bool NeedsRecalcPath()
            {
                // If we're moving towards a thing, we need to ensure it hasn't moved.
                var lastEntry = newThing.Path?.LastOrDefault();

                if (lastEntry == null || thingMovingTowards == null)
                {
                    return false;
                }

                return lastEntry.X != destinationX || lastEntry.Y != destinationY;
            }

            // The residual movement is filled with full movement, and will be reduced as we process.
            newThing.ResidualMovement = landSpeed + newThing.ResidualMovement;

            if (newThing.Path == null || NeedsRecalcPath())
            {
                // A new path needs calculating.
                if (canFly)
                {
                    newThing.Path = FlightPath((startX, startY), (destinationX, destinationY)).ToImmutableList();
                }
                else
                {
                    var path = this.finder.FindPath(new Point(startX, startY), new Point(destinationX, destinationY), waterSpeedRatio);

                    if (path != null)
                    {
                        newThing.Path = path.Select(p => new Location() { X = p.X, Y = p.Y }).ToImmutableList();
                    }
                }
            }

            // Walk the path until we're out of movement.
            while (true)
            {
                var pathEntry = newThing.Path?.FirstOrDefault();
                if (pathEntry == null)
                {
                    // We're done walking.
                    break;
                }

                var square = this.world.Grid.GetSquare(pathEntry.X, pathEntry.Y);
                float movementCost = square.GetMovementCost(this.random);
                bool isWater = square.SquareDefinition?.Definition.IsWater ?? false;

                if (!CanMove(newThing, movementCost, isWater, waterSpeedRatio))
                {
                    // Costs too much to enter this square.
                    break;
                }

                // Remove this path entry.
                newThing.Path = newThing.Path?.RemoveAt(0);

                // Apply the movement
                newThing.X = pathEntry.X;
                newThing.Y = pathEntry.Y;

                if (isWater)
                {
                    newThing.ResidualMovement -= movementCost * waterSpeedRatio;
                }
                else
                {
                    newThing.ResidualMovement -= movementCost;
                }
            }

            if (newThing.X == destinationX && newThing.Y == destinationY)
            {
                Log.Info($"{newThing.BaseDefinition.Name} {newThing.Name} has completed its movement.");
                newThing.CompleteMovement();

                // If moving towards a site, enter the site.
                if (thingMovingTowards.HasValue && newThing is ICanEnterSites enterSites)
                {
                    enterSites.InSiteId = thingMovingTowards;
                }
            }
        }

        /// <summary>
        /// Checks if the movement is possible.
        /// </summary>
        /// <param name="thing">The thing to eval.</param>
        /// <param name="distance">The distance to move.</param>
        /// <param name="isWater">True if this distance is over water, false if over land.</param>
        /// <param name="waterSpeedRatio">The ratio of land to water speed.</param>
        /// <returns>True if there is enough remaining move to make this distance.</returns>
        private static bool CanMove(BaseMovingThing thing, float distance, bool isWater, float waterSpeedRatio)
        {
            if (isWater)
            {
                return distance <= thing.ResidualMovement;
            }
            else
            {
                return distance <= thing.ResidualMovement * waterSpeedRatio;
            }
        }

        /// <summary>
        /// Paths to the destination using a straight line.
        /// </summary>
        /// <param name="start">The start of the path to find.</param>
        /// <param name="end">The end of the path to find.</param>
        /// <returns>The path to the destination.</returns>
        private static List<Location> FlightPath((int X, int Y) start, (int X, int Y) end)
        {
            var path = new List<Location>();
            int currX = start.X;
            int currY = start.Y;

            while (true)
            {
                int xDif = end.X - currX;
                int yDif = end.Y - currY;

                if (xDif == 0 && yDif == 0)
                {
                    break;
                }
                else if (Math.Abs(yDif) > Math.Abs(xDif))
                {
                    currX += yDif < 0 ? -1 : 1;
                }
                else
                {
                    currY += xDif < 0 ? -1 : 1;
                }

                path.Add(new Location() { X = currX, Y = currY });
            }

            return path;
        }
    }
}
