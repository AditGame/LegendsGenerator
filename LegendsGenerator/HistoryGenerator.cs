// <copyright file="HistoryGenerator.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using LegendsGenerator.Contracts;
    using LegendsGenerator.Contracts.Compiler;
    using LegendsGenerator.Contracts.Definitions;
    using LegendsGenerator.Contracts.Definitions.Events;
    using LegendsGenerator.Contracts.Definitions.Events.Effects;
    using LegendsGenerator.Contracts.Things;

    /// <summary>
    /// Iterates the world state.
    /// </summary>
    public class HistoryGenerator
    {
        /// <summary>
        /// The thing factory.
        /// </summary>
        private readonly ThingFactory thingFactory;

        /// <summary>
        /// The collection of definitions.
        /// </summary>
        private readonly DefinitionCollection definitions;

        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryGenerator"/> class.
        /// </summary>
        /// <param name="compiler">THe condition compiler to use.</param>
        /// <param name="definitionDirectory">A directory full of definition json files.</param>
        /// <param name="additionalDefinitionDirectories">Optional additional directories to get definition json files from.</param>
        public HistoryGenerator(IConditionCompiler compiler, string definitionDirectory, params string[] additionalDefinitionDirectories)
            : this(DefinitionSerializer.DeserializeFromDirectory(compiler, definitionDirectory)
                  .Combine(additionalDefinitionDirectories.Select(d => DefinitionSerializer.DeserializeFromDirectory(compiler, d)).ToArray()))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryGenerator"/> class.
        /// </summary>
        /// <param name="definitions">The definitions collection to use.</param>
        public HistoryGenerator(DefinitionCollection definitions)
        {
            this.definitions = definitions;
            this.thingFactory = new ThingFactory(definitions);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryGenerator"/> class.
        /// </summary>
        /// <param name="thingFactory">The thing factory to use.</param>
        /// <param name="definitions">The definitions collection to use.</param>
        public HistoryGenerator(ThingFactory thingFactory, DefinitionCollection definitions)
        {
            this.definitions = definitions;
            this.thingFactory = thingFactory;
        }

#if DEBUG
        /// <summary>
        /// Gets or sets the thing ID to open the debugger at.
        /// </summary>
        public Guid? OpenDebuggerAtThing { get; set; }
#endif

        /// <summary>
        /// Advances the history of the world by one step.
        /// </summary>
        /// <param name="currWorld">The current world.</param>
        /// <returns>The next world definition.</returns>
        public World Step(World currWorld)
        {
            if (currWorld is null)
            {
                throw new ArgumentNullException(nameof(currWorld));
            }

#pragma warning disable SA1101 // Prefix local calls with this. Stylecop has not been updated with support for Records.
            World nextWorld = currWorld with
            {
                StepCount = currWorld.StepCount + 1,
                Grid = currWorld.Grid.CloneWithoutThings(),
                OccurredEvents = new List<OccurredEvent>(),
            };
#pragma warning restore SA1101 // Prefix local calls with this

            IDictionary<ThingType, List<EventDefinition>> eventsBySubjectType =
                this.definitions.Events.GroupBy(ev => ev.Subject.Type).ToDictionary(kv => kv.Key, kv => kv.ToList());

            IDictionary<Guid, StagedThing> stagedThings = new Dictionary<Guid, StagedThing>();

            // Iterate through every gridsquare to create the new world.
            foreach (var (x, y, square) in currWorld.Grid.GetAllGridEntries())
            {
                foreach (BaseThing thing in square.GetThings())
                {
                    Log.Info($"Processing {thing.ThingType} {thing.Name}");
#if DEBUG
                    // Allows easy debugging if a thing isn't working as expected.
                    if (this.OpenDebuggerAtThing == thing.ThingId)
                    {
                        if (Debugger.IsAttached)
                        {
                            Debugger.Break();
                        }
                        else
                        {
                            Debugger.Launch();
                        }
                    }
#endif

                    StagedThing newThing = GetOrCreate(stagedThings, thing);

                    if (newThing.Destroyed)
                    {
                        // Do not process events from destroyed things.
                        Log.Info($"Skipping as this thing is destroyed.");
                        continue;
                    }

                    Random rdm = RandomFactory.GetRandom(currWorld.WorldSeed, currWorld.StepCount, thing.ThingId);

                    if (!eventsBySubjectType.TryGetValue(thing.ThingType, out var availableEvents))
                    {
                        continue;
                    }

                    IEnumerable<OccurredEvent> occurredEvents = GetOccurringEvents(rdm, currWorld, (x, y), availableEvents, thing);

                    foreach (OccurredEvent occurredEvent in occurredEvents)
                    {
                        Log.Info($"Adding event {occurredEvent.Event.Description}");
                        this.ApplyEvent(rdm, nextWorld, occurredEvent, newThing.Thing, stagedThings);
                    }

                    ProcessMovement(rdm, currWorld, newThing.Thing, stagedThings);
                }
            }

            foreach (var stagedThing in stagedThings.Where(t => !t.Value.Destroyed))
            {
                nextWorld.Grid.AddThing(stagedThing.Value.Thing);
            }

            return nextWorld;
        }

        /// <summary>
        /// Gets the events which will occur for the given thing.
        /// </summary>
        /// <param name="rdm">The random number generator.</param>
        /// <param name="world">The world definition.</param>
        /// <param name="coordinates">The coordinates of the center of the event.</param>
        /// <param name="applicableEvents">The events by subject type.</param>
        /// <param name="thing">The thing to get the events for.</param>
        /// <returns>The list of events which will occur for this thing.</returns>
        private static IEnumerable<OccurredEvent> GetOccurringEvents(
            Random rdm,
            World world,
            (int X, int Y) coordinates,
            IEnumerable<EventDefinition> applicableEvents,
            BaseThing thing)
        {
            int maxEvents = thing.BaseDefinition.EvalMaxEvents(rdm, thing);

            bool isMoving = (thing is BaseMovingThing movingThing) && movingThing.IsMoving;

            int minChance = rdm.Next(1, 100);
            return applicableEvents
                .Shuffle(rdm)
                .Where(e => !isMoving || e.CanTriggerWhileMoving)
                .Where(e => !e.Subject.Definitions.Any() || e.Subject.Definitions.Any(d => thing.BaseDefinition.InheritedDefinitionNames.Any(x => x == d)))
                .Where(e => e.Subject.EvalCondition(rdm, thing))
                .Select(e =>
                {
                    var objects = GetMatchingObjects(
                        rdm,
                        world,
                        coordinates,
                        e,
                        thing);

                    if (objects == null)
                    {
                        return null;
                    }

                    return new OccurredEvent(e.EvalDescription(rdm, thing, objects), e, thing, objects);
                })
                .Where(o => o != null)
                .Select(o => o!)
                .Where(o => o.Event.EvalChance(rdm, thing, o.Objects!) >= minChance)
                .Take(maxEvents);
        }

        /// <summary>
        /// Gets if a particular event matches the specified thing.
        /// </summary>
        /// <param name="minChance">The minimum chance for this thing this step.</param>
        /// <param name="chance">The chance function.</param>
        /// <param name="condition">The condition function.</param>
        /// <returns>True if this event applies to the subject, false otherwise.</returns>
        private static bool Matches(int minChance, Func<int> chance, Func<bool> condition)
        {
            if (chance() < minChance)
            {
                return false;
            }

            return condition();
        }

        /// <summary>
        /// Gets objects matching the definition.
        /// </summary>
        /// <param name="rdm">The random number generator.</param>
        /// <param name="world">The world information.</param>
        /// <param name="coordinates">The coordinates of the center of the event.</param>
        /// <param name="eventDef">The event definition of the event.</param>
        /// <param name="subject">The subject of the event.</param>
        /// <returns>The dictionary of objects, or null if the objects could not be found.</returns>
        private static IDictionary<string, BaseThing>? GetMatchingObjects(
            Random rdm,
            World world,
            (int X, int Y) coordinates,
            EventDefinition eventDef,
            BaseThing subject)
        {
            IDictionary<string, BaseThing> output = new Dictionary<string, BaseThing>();
            foreach (var @object in eventDef.Objects)
            {
                IEnumerable<(int X, int Y, GridSquare Square)> squaresInRange = world.Grid.GetGridEntriesWithinRange(coordinates.X, coordinates.Y, @object.Value.Distance).Shuffle(rdm);

                BaseThing? matchingThing = null;
                foreach (var (x, y, square) in squaresInRange)
                {
                    matchingThing =
                        square.GetThings(@object.Value.Type)
                        .Where(x => !@object.Value.Definitions.Any() || @object.Value.Definitions.Any(d => x.BaseDefinition.InheritedDefinitionNames.Any(x => x == d)))
                        .FirstOrDefault(t => @object.Value.EvalCondition(rdm, subject, new Dictionary<string, BaseThing>() { { @object.Key, t } }));

                    // Break from the loop once a matching thing is found in ANY square.
                    if (matchingThing != null)
                    {
                        break;
                    }
                }

                if (matchingThing == null && !@object.Value.Optional)
                {
                    return null;
                }

                if (matchingThing != null)
                {
                    output[@object.Key] = matchingThing;
                }
            }

            return output;
        }

        /// <summary>
        /// Gets or creates a staged thing change.
        /// </summary>
        /// <param name="dictionary">THe dictionary of staged things.</param>
        /// <param name="currWorldThing">The thing in the current world.</param>
        /// <returns>The staged thing.</returns>
        private static StagedThing GetOrCreate(IDictionary<Guid, StagedThing> dictionary, BaseThing currWorldThing)
        {
            if (!dictionary.TryGetValue(currWorldThing.ThingId, out StagedThing? staged))
            {
                staged = StagedThing.FromCurrWorldThing(currWorldThing);
                dictionary[currWorldThing.ThingId] = staged;
            }

            return staged;
        }

        /// <summary>
        /// Processes movement for this thing.
        /// </summary>
        /// <param name="rdm">The random number generator for this thing.</param>
        /// <param name="world">The world.</param>
        /// <param name="thing">The thing to apply the event to.</param>
        /// <param name="stagedThings">The staged things.</param>
        private static void ProcessMovement(Random rdm, World world, BaseThing thing, IDictionary<Guid, StagedThing> stagedThings)
        {
            // Handle movement
            BaseMovingThing? newMovingThing = GetOrCreate(stagedThings, thing)?.Thing as BaseMovingThing;
            if (newMovingThing != null && newMovingThing.IsMoving)
            {
                MovementHandler handler = new MovementHandler(rdm, newMovingThing, world);
                handler.ApplyMovement(newMovingThing);
            }
        }

        /// <summary>
        /// Applies the result of an event on a Thing.
        /// </summary>
        /// <param name="rdm">The random nubmer generator for this thing.</param>
        /// <param name="world">The world.</param>
        /// <param name="ev">The occurred event definition.</param>
        /// <param name="thing">The thing to apply the event to.</param>
        /// <param name="stagedThings">The staged things.</param>
        private void ApplyEvent(Random rdm, World world, OccurredEvent ev, BaseThing thing, IDictionary<Guid, StagedThing> stagedThings)
        {
            BaseThing? GetThing(string name)
            {
                if (name.Equals("Subject"))
                {
                    return GetOrCreate(stagedThings, thing).Thing;
                }
                else if (ev.Objects.TryGetValue(name, out var @object))
                {
                    return GetOrCreate(stagedThings, @object).Thing;
                }
                else
                {
                    Log.Error($"Unable to find object {name}, available objects are Subject, {string.Join(", ", ev.Objects.Keys)}.");
                    return null;
                }
            }

            int minChance = rdm.Next(1, 100);
            EventResultDefinition? result = ev.Event.Results
                .Shuffle(rdm)
                .FirstOrDefault(e => Matches(minChance, () => e.EvalChance(rdm, thing, ev.Objects), () => e.EvalCondition(rdm, thing, ev.Objects)));

            result ??= ev.Event.Results.FirstOrDefault(r => r.Default);

            // If there's no result which matches, discard this event.
            if (result == null)
            {
                Log.Warning($"The event had no matching results and no default result. Description: [{ev.Description}]");
                return;
            }

#pragma warning disable SA1101 // Prefix local calls with this. False positive due to C#9 feature.
            world.OccurredEvents.Add(ev with { Result = result });
#pragma warning restore SA1101 // Prefix local calls with this

            foreach (AttributeEffectDefinition effectDefinition in result.Effects)
            {
                Effect effect = new Effect()
                {
                    Title = effectDefinition.EvalTitle(rdm, thing, ev.Objects),
                    Description = effectDefinition.EvalDescription(rdm, thing, ev.Objects),
                    Attribute = effectDefinition.AffectedAttribute,
                    AttributeEffect = effectDefinition.EvalMagnitude(rdm, thing, ev.Objects),
                    Duration = effectDefinition.EvalDuration(rdm, thing, ev.Objects),
                    TookEffect = world.StepCount,
                };

                foreach (var appliedTo in effectDefinition.AppliedTo)
                {
                    BaseThing? appliedToThing = GetThing(appliedTo);
                    if (appliedToThing != null)
                    {
                        GetOrCreate(stagedThings, appliedToThing).Thing.Effects.Add(effect);
                    }
                }
            }

            foreach (SpawnDefinition spawnDefinition in result.Spawns)
            {
                int xPosition = spawnDefinition.EvalPositionX(rdm, thing, ev.Objects);
                int yPosition = spawnDefinition.EvalPositionY(rdm, thing, ev.Objects);

                if (spawnDefinition.PositionType == PositionType.RelativeAbsolute)
                {
                    xPosition += thing.X;
                    yPosition += thing.Y;
                }

                BaseThing spawnedThing = this.thingFactory.CreateThing(
                    rdm,
                    xPosition,
                    yPosition,
                    spawnDefinition.Type,
                    spawnDefinition.DefinitionNameToSpawn);

                foreach (var overrideAttr in spawnDefinition.AttributeOverrides.Keys)
                {
                    spawnedThing.BaseAttributes[overrideAttr] = spawnDefinition.EvalAttributeOverrides(overrideAttr, rdm, spawnedThing, ev.Objects);
                }

                // Should never get, and always create.
                GetOrCreate(stagedThings, spawnedThing);
            }

            foreach (DestroyDefinition destroyDefinition in result.Destroys)
            {
                foreach (string affected in destroyDefinition.AppliedTo)
                {
                    BaseThing? destroyed = GetThing(affected);
                    if (destroyed != null)
                    {
                        GetOrCreate(stagedThings, destroyed).Destroyed = true;
                    }
                }
            }

            foreach (MoveDefinition moveDefinition in result.Moves)
            {
                foreach (string applied in moveDefinition.AppliedTo)
                {
                    BaseThing? thingToMove = GetThing(applied);
                    if (thingToMove == null)
                    {
                        continue;
                    }

                    if (thingToMove is not BaseMovingThing movingThing)
                    {
                        Log.Error($"Tried to move {applied} ({thingToMove.ThingId}) but it not the type of thing to be able to move.");
                        continue;
                    }

                    movingThing.MoveType = moveDefinition.MoveType;

                    switch (moveDefinition.MoveType)
                    {
                        case MoveType.ToCoords:
                            if (moveDefinition.CoordToMoveToX == null)
                            {
                                Log.Error("Movetype is ToCoords, but CoordToMoveToX is null.");
                                continue;
                            }

                            if (moveDefinition.CoordToMoveToY == null)
                            {
                                Log.Error("Movetype is ToCoords, but CoordToMoveToY is null.");
                                continue;
                            }

                            movingThing.MoveToCoordX = moveDefinition.EvalCoordToMoveToX(rdm, thing, ev.Objects);
                            movingThing.MoveToCoordY = moveDefinition.EvalCoordToMoveToY(rdm, thing, ev.Objects);
                            break;
                        case MoveType.ToThing:
                            if (moveDefinition.ThingToMoveTo == null)
                            {
                                Log.Error("Movetype is ToThing, but ThingToMoveTo is null.");
                                continue;
                            }

                            BaseThing? thingToMoveTo = GetThing(moveDefinition.ThingToMoveTo);
                            if (thingToMoveTo == null)
                            {
                                continue;
                            }

                            movingThing.MoveToThing = thingToMoveTo.ThingId;
                            break;
                        default:
                            Log.Error("Invalid move type");
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// A thing which is staged to be placed on the world stage.
        /// </summary>
        private class StagedThing
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="StagedThing"/> class.
            /// </summary>
            /// <param name="thing">The thing.</param>
            private StagedThing(BaseThing thing)
            {
                this.Thing = thing;
            }

            /// <summary>
            /// Gets or sets a value indicating whether this thing is destroyed.
            /// </summary>
            public bool Destroyed { get; set; }

            /// <summary>
            /// Gets or sets the thing.
            /// </summary>
            public BaseThing Thing { get; set; }

            /// <summary>
            /// Creates a staged thing from the current world's thing.
            /// </summary>
            /// <param name="thing">The thing to clone with age.</param>
            /// <returns>The staged thing.</returns>
            public static StagedThing FromCurrWorldThing(BaseThing thing)
            {
                return new StagedThing(thing.CreateAgedClone());
            }
        }
    }
}
