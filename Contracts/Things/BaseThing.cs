// <copyright file="BaseThing.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Things
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using LegendsGenerator.Contracts;
    using LegendsGenerator.Contracts.Definitions;
    using LegendsGenerator.Contracts.Definitions.Events;

    /// <summary>
    /// The base implementation of any Thing.
    /// </summary>
    public abstract record BaseThing
    {
        /// <summary>
        /// Gets the name of the thing.
        /// </summary>
        public abstract ThingType ThingType { get; }

        /// <summary>
        /// Gets or sets the base definition of the thing.
        /// </summary>
        public BaseThingDefinition BaseDefinition { get; set; } = new BaseThingDefinition();

        /// <summary>
        /// Gets or sets the name of this thing.
        /// </summary>
        public string Name { get; set; } = "UNNAMED";

        /// <summary>
        /// Gets or sets the ID of this Thing.
        /// </summary>
        public Guid ThingId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the X coordinate of this thing.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate of this thing.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Gets the base Attribute on this object before Effects are applied.
        /// </summary>
        public Dictionary<string, int> BaseAttributes { get; init; } = new Dictionary<string, int>();

        /// <summary>
        /// Gets all effects on this thing. Expired effects should be culled on Step.
        /// </summary>
        public IList<Effect> Effects { get; init; } = new List<Effect>();

        /// <summary>
        /// Creates a clone of this thing incrementing the step count by one.
        /// </summary>
        /// <returns>A clone of this with everything aged by one step.</returns>
        public BaseThing CreateAgedClone()
        {
            // Clone the thing with an empty effect list.
            BaseThing newThing = this with
            {
                Effects = new List<Effect>(),
            };

            foreach (Effect effect in Effects)
            {
                if (effect.Duration == -1)
                {
                    newThing.Effects.Add(effect with { });
                }
                else if (effect.Duration >= 1)
                {
                    newThing.Effects.Add(effect with
                    {
                        Duration = effect.Duration - 1,
                    });
                }
            }

            return newThing;
        }

        /// <summary>
        /// Gets the effects which are modifying the specified value.
        /// </summary>
        /// <param name="attribute">The attribute to get.</param>
        /// <returns>The list of all effects which are modifying this specified attrbute.</returns>
        public IEnumerable<Effect> GetEffectsModifying(string attribute)
        {
            return Effects.Where(a => a.Attribute.Equals(attribute));
        }

        /// <summary>
        /// Calculates the effective attribute value based on the current effects.
        /// </summary>
        /// <param name="attribute">The attribute to get.</param>
        /// <param name="defaultValue">The default value, if the attribtue does not exist.</param>
        /// <returns>The effective attribute value.</returns>
        public int EffectiveAttribute(string attribute, int defaultValue)
        {
            if (!BaseAttributes.TryGetValue(attribute, out int value))
            {
                return defaultValue;
            }

            return value + GetEffectsModifying(attribute).Sum(a => a.AttributeEffect);
        }

        /// <summary>
        /// Calculates the effective attribute value based on the current effects.
        /// </summary>
        /// <param name="attribute">The attribute to get.</param>
        /// <returns>The effective attribute value.</returns>
        public int EffectiveAttribute(string attribute)
        {
            if (!BaseAttributes.TryGetValue(attribute, out int value))
            {
                throw new ArgumentException($"{ThingType} Type does not have attribute {attribute}", nameof(attribute));
            }

            // TODO: Handle this scenario better.
            try
            {
                return value + GetEffectsModifying(attribute).Sum(a => a.AttributeEffect);
            }
            catch (OverflowException)
            {
                return int.MaxValue;
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder($"{ThingType} {Name}");
            sb.AppendLine();

            foreach (Effect effect in Effects)
            {
                sb.AppendLine($"  {effect.Title} ({effect.Duration}): {effect.Attribute} {effect.AttributeEffect}");
            }

            return sb.ToString();
        }
    }
}
