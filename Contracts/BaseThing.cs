// <copyright file="BaseThing.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using LegendsGenerator.Contracts.Definitions;

    /// <summary>
    /// The base implementation of any Thing.
    /// </summary>
    public abstract class BaseThing
    {
        /// <summary>
        /// Gets the name of the thing.
        /// </summary>
        public abstract string ThingTypeName { get; }

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
        /// Gets the base Attribute on this object before Effects are applied.
        /// </summary>
        public Dictionary<string, int> BaseAttributes { get; } = new Dictionary<string, int>();

        /// <summary>
        /// Gets all effects on this thing. Expired effects should be culled on Step.
        /// </summary>
        public IList<Effect> Effects { get; } = new List<Effect>();

        /// <summary>
        /// Gets the effects which are modifying the specified value.
        /// </summary>
        /// <param name="attribute">The attribute to get.</param>
        /// <returns>The list of all effects which are modifying this specified attrbute.</returns>
        public IEnumerable<Effect> GetEffectsModifying(string attribute)
        {
            return this.Effects.Where(a => a.Attribute.Equals(attribute));
        }

        /// <summary>
        /// Calculates the effective attribute value based on the current effects.
        /// </summary>
        /// <param name="attribute">The attribute to get.</param>
        /// <returns>The effective attribute value.</returns>
        public int EffectiveAttribute(string attribute)
        {
            if (!this.BaseAttributes.TryGetValue(attribute, out int value))
            {
                throw new ArgumentException($"{this.ThingTypeName} Type does not have attribute {attribute}", nameof(attribute));
            }

            return value + this.GetEffectsModifying(attribute).Sum(a => a.AttributeEffect);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder($"{this.ThingTypeName} {this.Name}");

            foreach (Effect effect in this.Effects)
            {
                sb.AppendLine($"  {effect.Title}: {effect.Attribute} {effect.AttributeEffect}");
            }

            return sb.ToString();
        }
    }
}
