// <copyright file="HistoryMachine.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Text;
    using LegendsGenerator.Contracts;
    using LegendsGenerator.Contracts.Definitions.Events;

    /// <summary>
    /// Iterates the world state.
    /// </summary>
    public class HistoryMachine
    {
        /// <summary>
        /// The condition compiler.
        /// </summary>
        private ConditionCompiler compiler;

        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryMachine"/> class.
        /// </summary>
        /// <param name="compiler">The condition compiler.</param>
        public HistoryMachine(ConditionCompiler compiler)
        {
            this.compiler = compiler;
        }

        /// <summary>
        /// Advances the history of the world by one step.
        /// </summary>
        /// <param name="world">The world.</param>
        public void Step(World world)
        {
            IDictionary<ThingType, List<EventDefinition>> eventsBySubjectType =
                world.Events.GroupBy(ev => ev.Subject.Type).ToDictionary(kv => kv.Key, kv => kv.ToList());

            foreach (Site site in world.Sites)
            {
                Log.Info($"Processing site {site.Name}");
                Random rdm = RandomFactory.GetRandom(world.WorldSeed, world.StepCount, site.ThingId);

                IEnumerable<EventDefinition> occurredEvents =
                    this.GetOccurringEvents(rdm, eventsBySubjectType[ThingType.Site], site);

                foreach (EventDefinition occurredEvent in occurredEvents)
                {
                    Log.Info($"Adding event {occurredEvent.Description}");
                    this.ApplyEvent(rdm, world, occurredEvent, site);
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
        private void ApplyEvent(Random rdm, World world, EventDefinition ev, BaseThing thing)
        {
            int minChance = rdm.Next(1, 100);
            EventResultDefinition? result = ev.Results
                .Shuffle(rdm)
                .FirstOrDefault(e => this.Matches(minChance, () => e.EvalChance(rdm, thing), () => e.EvalCondition(rdm, thing)));

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
                    Title = effectDefinition.EvalTitle(rdm, thing),
                    Description = effectDefinition.EvalDescription(rdm, thing),
                    Attribute = effectDefinition.AffectedAttribute,
                    AttributeEffect = effectDefinition.EvalMagnitude(rdm, thing),
                    Duration = effectDefinition.EvalDuration(rdm, thing),
                    TookEffect = world.StepCount,
                };

                thing.Effects.Add(effect);
            }
        }

        /// <summary>
        /// Gets the events which will occur for the given thing.
        /// </summary>
        /// <param name="rdm">The random number generator.</param>
        /// <param name="applicableEvents">The events by subject type.</param>
        /// <param name="thing">The thing to get the events for.</param>
        /// <returns>The list of events which will occur for this thing.</returns>
        private IEnumerable<EventDefinition> GetOccurringEvents(
            Random rdm,
            IEnumerable<EventDefinition> applicableEvents,
            BaseThing thing)
        {
            int maxEvents = thing.BaseDefinition.EvalMaxEvents(rdm, thing);

            int minChance = rdm.Next(1, 100);
            return applicableEvents
                .Shuffle(rdm)
                .Where(e => this.Matches(minChance, () => e.EvalChance(rdm, thing), () => e.Subject.EvalCondition(rdm, thing)))
                .Take(maxEvents);
        }

        /// <summary>
        /// Gets if a particular event matches the specified thing.
        /// </summary>
        /// <param name="minChance">The minimum chance for this thing this step.</param>
        /// <param name="chance">The chance function.</param>
        /// <param name="condition">The condition function.</param>
        /// <returns>True if this event applies to the subject, false otherwise.</returns>
        private bool Matches(int minChance, Func<int> chance, Func<bool> condition)
        {
            if (chance() < minChance)
            {
                return false;
            }

            return condition();
        }
    }
}
