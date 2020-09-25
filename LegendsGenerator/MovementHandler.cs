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
    using System.Text;
    using System.Threading.Tasks;
    using LegendsGenerator.Contracts;
    using LegendsGenerator.Contracts.Things;

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
        /// Initializes a new instance of the <see cref="MovementHandler"/> class.
        /// </summary>
        /// <param name="rdm">Random number generator.</param>
        /// <param name="thing">The thing to process movement for.</param>
        /// <param name="world">The world.</param>
        public MovementHandler(Random rdm, BaseMovingThing thing, World world)
        {
            this.CanFly = thing.MovingThingDefinition.EvalFlies(rdm, thing);
            this.RemainingLandMoveDistance = thing.MovingThingDefinition.EvalLandSpeed(rdm, thing) + thing.ResidualMovement;

            if (!this.CanFly)
            {
                this.waterSpeedRatio = this.RemainingLandMoveDistance / thing.MovingThingDefinition.EvalWaterSpeed(rdm, thing);
            }

            this.CurrentPositionX = thing.X;
            this.CurrentPositionY = thing.Y;

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
        /// Gets the remaining distance this thing can move.
        /// </summary>
        public float RemainingLandMoveDistance { get; private set; }

        /// <summary>
        /// Gets the remaining distance over water this thing can move.
        /// </summary>
        public float RemainingWaterMoveDistance => this.RemainingLandMoveDistance * this.waterSpeedRatio;

        /// <summary>
        /// Gets the starting X position.
        /// </summary>
        public int CurrentPositionX { get; private set; }

        /// <summary>
        /// Gets the starting Y position.
        /// </summary>
        public int CurrentPositionY { get; private set; }

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
        /// Completes the movement onto the specified thing.
        /// </summary>
        /// <param name="newThing">The new version of the inputted thing.</param>
        public void ApplyMovement(BaseMovingThing newThing)
        {
            newThing.X = this.CurrentPositionX;
            newThing.Y = this.CurrentPositionY;
            newThing.ResidualMovement = this.RemainingLandMoveDistance;

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
        /// Runs the movement of this thing to it's completion.
        /// </summary>
        /// <param name="rdm">The random number generator to use.</param>
        /// <param name="world">The world to move through.</param>
        public void ProcessMovement(Random rdm, World world)
        {
            // Really cheap straight-line function to the goal.
            while (true)
            {
                int xDif = this.DestinationX - this.CurrentPositionX;
                int yDif = this.DestinationY - this.CurrentPositionY;

                int desiredX = this.CurrentPositionX;
                int desiredY = this.CurrentPositionY;

                if (xDif == 0 && yDif == 0)
                {
                    break;
                }
                else if (Math.Abs(yDif) > Math.Abs(xDif))
                {
                    desiredY += yDif < 0 ? -1 : 1;
                }
                else
                {
                    desiredX += xDif < 0 ? -1 : 1;
                }

                var square = world.Grid.GetSquare(desiredX, desiredY);
                float movementCost = square.SquareDefinition?.Definition.EvalMovementCost(rdm, square.SquareDefinition) ?? 1;
                bool isWater = square.SquareDefinition?.Definition.IsWater ?? false;

                if (this.CanMove(movementCost, isWater))
                {
                    // Costs too much to enter this square.
                    break;
                }

                this.CurrentPositionX = desiredX;
                this.CurrentPositionY = desiredY;

                if (isWater)
                {
                    this.MoveWater(movementCost);
                }
                else
                {
                    this.MoveLand(movementCost);
                }
            }
        }

        /// <summary>
        /// Moves on land by the specified distance.
        /// </summary>
        /// <param name="distance">The distance to move.</param>
        public void MoveLand(float distance)
        {
            this.RemainingLandMoveDistance -= distance;
        }

        /// <summary>
        /// Moves on water by the specified distance.
        /// </summary>
        /// <param name="distance">The distance to move.</param>
        public void MoveWater(float distance)
        {
            if (this.CanFly)
            {
                this.RemainingLandMoveDistance -= distance;
            }
            else
            {
                this.RemainingLandMoveDistance -= distance * this.waterSpeedRatio;
            }
        }

        /// <summary>
        /// Checks if the movement is possible.
        /// </summary>
        /// <param name="distance">The distance to move.</param>
        /// <param name="isWater">True if this distance is over water, false if over land.</param>
        /// <returns>True if there is enough remaining move to make this distance.</returns>
        public bool CanMove(float distance, bool isWater)
        {
            if (isWater)
            {
                return distance <= this.RemainingWaterMoveDistance;
            }
            else
            {
                return distance <= this.RemainingLandMoveDistance;
            }
        }
    }
}
