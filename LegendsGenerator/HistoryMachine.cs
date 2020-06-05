// <copyright file="HistoryMachine.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LegendsGenerator.Contracts;
    using LegendsGenerator.Contracts.Definitions.Events;

    /// <summary>
    /// Iterates the world state.
    /// </summary>
    public static class HistoryMachine
    {
        /// <summary>
        /// Advances the history of the world by one step.
        /// </summary>
        /// <param name="world">The world.</param>
        public static void Step(World world)
        {
            IDictionary<ThingType, List<EventDefinition>> eventsBySubjectType =
                world.Events.GroupBy(ev => ev.Subject.Type).ToDictionary(kv => kv.Key, kv => kv.ToList());

            foreach (Site site in world.Sites)
            {
                Log.Info($"Processing site {site.Name}");
                Random rdm = RandomFactory.GetRandom(world.WorldSeed, world.StepCount, site.ThingId);

                IEnumerable<OccurredEvent> occurredEvents =
                    GetOccurringEvents(rdm, world, eventsBySubjectType[ThingType.Site], site);

                foreach (OccurredEvent occurredEvent in occurredEvents)
                {
                    Log.Info($"Adding event {occurredEvent.Event.Description}");
                    ApplyEvent(rdm, world, occurredEvent.Event, site, occurredEvent.Objects!);
                }
            }
        }

        /// <summary>
        /// Applies the result of an event on a Thing.
        /// </summary>
        /// <param name="rdm">The random nubmer generator for this thing.</param>
        /// <param name="world">The world.</param>
        /// <param name="ev">The event definition.</param>
        /// <param name="thing">The thing to apply the event to.</param>
        /// <param name="objects">The objects of this event.</param>
        private static void ApplyEvent(Random rdm, World world, EventDefinition ev, BaseThing thing, IDictionary<string, BaseThing> objects)
        {
            int minChance = rdm.Next(1, 100);
            EventResultDefinition? result = ev.Results
                .Shuffle(rdm)
                .FirstOrDefault(e => Matches(minChance, () => e.EvalChance(rdm, thing, objects), () => e.EvalCondition(rdm, thing, objects)));

            result ??= ev.Results.FirstOrDefault(r => r.Default);

            // If there's no result which matches, discard this event.
            if (result == null)
            {
                Log.Warning($"The event had no matching results and no default result. Description: [{ev.Description}]");
                return;
            }

            foreach (EffectDefinition effectDefinition in result.Effects)
            {
                Effect effect = new Effect()
                {
                    Title = effectDefinition.EvalTitle(rdm, thing, objects),
                    Description = effectDefinition.EvalDescription(rdm, thing, objects),
                    Attribute = effectDefinition.AffectedAttribute,
                    AttributeEffect = effectDefinition.EvalMagnitude(rdm, thing, objects),
                    Duration = effectDefinition.EvalDuration(rdm, thing, objects),
                    TookEffect = world.StepCount,
                };

                foreach (var appliedTo in effectDefinition.AppliedTo)
                {
                    if (appliedTo.Equals("Subject"))
                    {
                        thing.Effects.Add(effect);
                    }
                    else if (objects.TryGetValue(appliedTo, out BaseThing? value))
                    {
                        value.Effects.Add(effect);
                    }
                    else
                    {
                        Log.Error($"Enable to apply effect to unknown Object {appliedTo}.");
                    }
                }
            }
        }

        /// <summary>
        /// Gets the events which will occur for the given thing.
        /// </summary>
        /// <param name="rdm">The random number generator.</param>
        /// <param name="world">The world definition.</param>
        /// <param name="applicableEvents">The events by subject type.</param>
        /// <param name="thing">The thing to get the events for.</param>
        /// <returns>The list of events which will occur for this thing.</returns>
        private static IEnumerable<OccurredEvent> GetOccurringEvents(
            Random rdm,
            World world,
            IEnumerable<EventDefinition> applicableEvents,
            BaseThing thing)
        {
            int maxEvents = thing.BaseDefinition.EvalMaxEvents(rdm, thing);

            int minChance = rdm.Next(1, 100);
            return applicableEvents
                .Shuffle(rdm)
                .Where(e => e.Subject.EvalCondition(rdm, thing))
                .Select(e =>
                {
                    var objects = GetMatchingObjects(
                        rdm,
                        world,
                        e,
                        thing);

                    if (objects == null)
                    {
                        return null;
                    }

                    return new OccurredEvent()
                    {
                        Event = e,
                        Objects = objects,
                    };
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
        /// <param name="eventDef">The event definition of the event.</param>
        /// <param name="subject">The subject of the event.</param>
        /// <returns>The dictionary of objects, or null if the objects coudl not be found.</returns>
        private static IDictionary<string, BaseThing>? GetMatchingObjects(
            Random rdm,
            World world,
            EventDefinition eventDef,
            BaseThing subject)
        {
            IDictionary<string, BaseThing> output = new Dictionary<string, BaseThing>();
            foreach (var @object in eventDef.Objects)
            {
                IEnumerable<BaseThing> things = world.GetThings(@object.Value.Type);
                BaseThing? matchingThing = world
                    .GetThings(@object.Value.Type)
                    .FirstOrDefault(t => @object.Value.EvalCondition(rdm, subject, new Dictionary<string, BaseThing>() { { @object.Key, t } }));

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
    }
}
