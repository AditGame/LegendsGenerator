// -------------------------------------------------------------------------------------------------
// <copyright file="MovementHandler.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LegendsGenerator.Contracts;
    using LegendsGenerator.Contracts.Things;
    using LegendsGenerator.PathFinding;

    /// <summary>
    /// State machine to handle movement.
    /// </summary>
    public class MovementHandler
    {
        /// <summary>
        /// The ratio of how much more expensive water movement is over land movement.
        /// </summary>
        private float waterSpeedRatio = 1;

        /// <summary>
        /// The world.
        /// </summary>
        private World world;

        /// <summary>
        /// The random number generator.
        /// </summary>
        private Random random;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovementHandler"/> class.
        /// </summary>
        /// <param name="rdm">Random number generator.</param>
        /// <param name="thing">The thing to process movement for.</param>
        /// <param name="world">The world.</param>
        public MovementHandler(Random rdm, BaseMovingThing thing, World world)
        {
            this.CanFly = thing.MovingThingDefinition.EvalFlies(rdm, thing);
            this.LandSpeed = thing.MovingThingDefinition.EvalLandSpeed(rdm, thing);
            this.WaterSpeed = thing.MovingThingDefinition.EvalWaterSpeed(rdm, thing);

            this.random = rdm;
            this.world = world;

            if (!this.CanFly)
            {
                this.waterSpeedRatio = this.LandSpeed / this.WaterSpeed;
            }

            this.StartX = thing.X;
            this.StartY = thing.Y;

            switch (thing.MoveType)
            {
                case MoveType.ToCoords:
                    this.DestinationX = thing.MoveToCoordX ?? throw new InvalidOperationException("Object is moving towards a coord but MoveToCoordX is null.");
                    this.DestinationY = thing.MoveToCoordY ?? throw new InvalidOperationException("Object is moving towards a coord but MoveToCoordY is null.");
                    break;
                case MoveType.ToThing:
                    BaseThing thingToMoveTo =
                        world.FindThing(thing.MoveToThing ?? throw new InvalidOperationException("Object is moving towards a thing but MoveToThing is null."));
                    this.DestinationX = thingToMoveTo.X;
                    this.DestinationY = thingToMoveTo.Y;
                    this.ThingMovingTowards = thing.MoveToThing;
                    break;
                default:
                    throw new InvalidOperationException($"Unsupported movetype {thing.MoveType}");
            }
        }

        /// <summary>
        /// Gets a value indicating whether the thing can fly.
        /// </summary>
        public bool CanFly { get; }

        /// <summary>
        /// Gets the starting X position.
        /// </summary>
        public int StartX { get; }

        /// <summary>
        /// Gets the starting Y position.
        /// </summary>
        public int StartY { get; }

        /// <summary>
        /// Gets the final X position.
        /// </summary>
        public int DestinationX { get; }

        /// <summary>
        /// Gets the final Y position.
        /// </summary>
        public int DestinationY { get; }

        /// <summary>
        /// Gets the thing this is moving towards, used to enter it if possible at the completion of it's movement.
        /// </summary>
        public Guid? ThingMovingTowards { get; }

        /// <summary>
        /// Gets the land speed of this thing.
        /// </summary>
        public float LandSpeed { get; }

        /// <summary>
        /// Gets the water speed of this thing.
        /// </summary>
        public float WaterSpeed { get; }

        /// <summary>
        /// Completes the movement onto the specified thing.
        /// </summary>
        /// <param name="newThing">The new version of the inputted thing.</param>
        public void ApplyMovement(BaseMovingThing newThing)
        {
            bool ThingMovingTowardsMoved()
            {
                // If we're moving towards a thing, we need to ensure it hasn't moved.
                var lastEntry = newThing.Path?.LastOrDefault();

                if (lastEntry == null || this.ThingMovingTowards == null)
                {
                    return false;
                }

                return lastEntry.X != this.DestinationX || lastEntry.Y != this.DestinationY;
            }

            // The residual movement is filled with full movement, and will be reduced as we process.
            newThing.ResidualMovement = this.LandSpeed + newThing.ResidualMovement;

            if (newThing.Path == null || ThingMovingTowardsMoved())
            {
                // A new path needs calculating.
                if (this.CanFly)
                {
                    newThing.Path = this.FlightPath();
                }
                else
                {
                    newThing.Path = this.LandPath();
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

                if (!this.CanMove(newThing, movementCost, isWater))
                {
                    // Costs too much to enter this square.
                    break;
                }

                // Remove this path entry.
                newThing.Path?.RemoveAt(0);

                // Apply the movement
                newThing.X = pathEntry.X;
                newThing.Y = pathEntry.Y;

                if (isWater)
                {
                    this.MoveWater(newThing, movementCost);
                }
                else
                {
                    this.MoveLand(newThing, movementCost);
                }
            }

            if (newThing.X == this.DestinationX && newThing.Y == this.DestinationY)
            {
                Log.Info($"{newThing.BaseDefinition.Name} {newThing.Name} has completed its movement.");
                newThing.CompleteMovement();

                // If moving towards a site, enter the site.
                if (this.ThingMovingTowards.HasValue && newThing is ICanEnterSites enterSites)
                {
                    enterSites.InSiteId = this.ThingMovingTowards;
                }
            }
        }

        /// <summary>
        /// Moves on land by the specified distance.
        /// </summary>
        /// <param name="movingThing">The moving thing.</param>
        /// <param name="distance">The distance to move.</param>
        public void MoveLand(BaseMovingThing movingThing, float distance)
        {
            movingThing.ResidualMovement -= distance;
        }

        /// <summary>
        /// Moves on water by the specified distance.
        /// </summary>
        /// <param name="movingThing">The moving thing.</param>
        /// <param name="distance">The distance to move.</param>
        public void MoveWater(BaseMovingThing movingThing, float distance)
        {
            if (this.CanFly)
            {
                movingThing.ResidualMovement -= distance;
            }
            else
            {
                movingThing.ResidualMovement -= distance * this.waterSpeedRatio;
            }
        }

        /// <summary>
        /// Checks if the movement is possible.
        /// </summary>
        /// <param name="thing">The thing to eval.</param>
        /// <param name="distance">The distance to move.</param>
        /// <param name="isWater">True if this distance is over water, false if over land.</param>
        /// <returns>True if there is enough remaining move to make this distance.</returns>
        public bool CanMove(BaseMovingThing thing, float distance, bool isWater)
        {
            if (isWater)
            {
                return distance <= thing.ResidualMovement;
            }
            else
            {
                return distance <= thing.ResidualMovement * this.waterSpeedRatio;
            }
        }

        /// <summary>
        /// Paths to the destination using a straight line.
        /// </summary>
        /// <returns>The path to the destination.</returns>
        private List<Location> FlightPath()
        {
            var path = new List<Location>();
            int currX = this.StartX;
            int currY = this.StartY;

            while (true)
            {
                int xDif = this.DestinationX - currX;
                int yDif = this.DestinationY - currY;

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

        /// <summary>
        /// Calculates A* on the world map.
        /// </summary>
        /// <returns>A list, from start to end, of the locations traveled through for this path.</returns>
        private List<Location> LandPath()
        {
            int ComputeHScore((int X, int Y) square)
            {
                return (Math.Abs(this.DestinationX - square.X) + Math.Abs(this.DestinationY - square.Y)) * 2;
            }

            PathSquare CreateAdjacentLocation(float currentCost, int x, int y)
            {
                var square = this.world.Grid.GetSquare(x, y);
                var movementCost = square.GetMovementCost(this.random);
                if (square.SquareDefinition?.Definition.IsWater == true)
                {
                    movementCost *= this.waterSpeedRatio;
                }

                return new PathSquare { X = x, Y = y, G = currentCost + movementCost, H = ComputeHScore((x, y)) };
            }

            List<PathSquare> GetAdjacentSquares(float currentCost, int x, int y)
            {
                var proposedLocations = new List<PathSquare>()
                {
                    CreateAdjacentLocation(currentCost, x, y + 1),
                    CreateAdjacentLocation(currentCost, x, y - 1),
                    CreateAdjacentLocation(currentCost, x + 1, y),
                    CreateAdjacentLocation(currentCost, x - 1, y),
                };

                return proposedLocations;
            }

            PathSquare? current = null;
            var start = new PathSquare()
            {
                X = this.StartX,
                Y = this.StartY,
                G = 0,
                H = ComputeHScore((this.StartX, this.StartY)),
            };

            var openList = new SortedKeylessCollection<float, PathSquare>();
            var closedList = new List<PathSquare>();
            var closedListSearch = new Dictionary<(int X, int Y), PathSquare>();

            // start by adding the original position to the open list
            openList.Add(start.F, start);

            while (openList.Count > 0)
            {
                // get the square with the lowest F (estimated distance to target) score
                current = openList.First();

                // add the current square to the closed list
                closedList.Add(current);
                closedListSearch.Add((current.X, current.Y), current);

                // remove it from the open list
                openList.Remove(current);

                // if we added the destination to the closed list, we've found a path
                if (current.X == this.DestinationX && current.Y == this.DestinationY)
                {
                    break;
                }

                var adjacentSquares = GetAdjacentSquares(current.F, current.X, current.Y);

                foreach (var adjacentSquare in adjacentSquares)
                {
                    // if this adjacent square is already in the closed list, ignore it
                    if (closedListSearch.ContainsKey((adjacentSquare.X, adjacentSquare.Y)))
                    {
                        continue;
                    }

                    // if it's not in the open list...
                    var existingEntry = openList.FirstOrDefault(l => l.X == adjacentSquare.X && l.Y == adjacentSquare.Y);
                    if (existingEntry == null)
                    {
                        adjacentSquare.Parent = current;

                        // and add it to the open list
                        openList.Add(adjacentSquare.F, adjacentSquare);
                    }
                    else
                    {
                        // test if using the current G score makes the adjacent square's F score
                        // lower, if yes update the parent because it means it's a better path
                        if (adjacentSquare.F < existingEntry.F)
                        {
                            openList.Remove(existingEntry);
                            existingEntry.G = adjacentSquare.G;
                            existingEntry.Parent = current;
                            openList.Add(existingEntry.F, existingEntry);
                        }
                    }
                }
            }

            // Drain the closed list into the output.
            IList<PathSquare> output = new List<PathSquare>();
            PathSquare? currEntry = closedList.Last();
            output.Add(currEntry);
            while (currEntry != null)
            {
                currEntry = currEntry.Parent;

                if (currEntry != null)
                {
                    output.Add(currEntry);
                }
            }

            // Reverse the output so it's a path from start to end.
            // Remove the first element as it's the starting square.
            return output.Reverse().Skip(1).Select(p => new Location() { X = p.X, Y = p.Y }).ToList();
        }

        /// <summary>
        /// Holder class for information needed by A* algorithm.
        /// </summary>
        private class PathSquare
        {
            /// <summary>
            /// Gets the X coord.
            /// </summary>
            public int X { get; init; }

            /// <summary>
            /// Gets the Y coord.
            /// </summary>
            public int Y { get; init; }

            /// <summary>
            /// Gets or sets the computed distance from the starting point.
            /// </summary>
            public float G { get; set; }

            /// <summary>
            /// Gets the taxicab distance to the target.
            /// </summary>
            public float H { get; init; }

            /// <summary>
            /// Gets the estimated distance from the start to the end via this location.
            /// </summary>
            public float F => this.G + this.H;

            /// <summary>
            /// Gets or sets the parent square.
            /// </summary>
            public PathSquare? Parent { get; set; }

            /// <inheritdoc/>
            public override string ToString()
            {
                return $"({this.X},{this.Y}):{this.G}+{this.H}={this.F}";
            }
        }
    }
}
