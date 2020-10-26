// <copyright file="BaseThing.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Things
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;
    using LegendsGenerator.Contracts;
    using LegendsGenerator.Contracts.Definitions;
    using LegendsGenerator.Contracts.Definitions.Events;

    /// <summary>
    /// The base implementation of any Thing.
    /// </summary>
    public abstract record BaseThing
    {
        public BaseThing(BaseThingDefinition definition)
        {
            this.BaseDefinition = definition;
        }

        /// <summary>
        /// Gets the name of the thing.
        /// </summary>
        public abstract ThingType ThingType { get; }

        /// <summary>
        /// Gets the state of this thing.
        /// </summary>
        public ThingState State { get; private set; } = ThingState.Constructing;

        /// <summary>
        /// Gets or sets the base definition of the thing.
        /// </summary>
        public BaseThingDefinition BaseDefinition { get; set; }

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
        /// Gets the Dynamic attribute values, calculated at the end of the generation process.
        /// </summary>
        public Dictionary<string, int> DynamicAttributes { get; init; } = new Dictionary<string, int>();

        /// <summary>
        /// Gets the base Aspect on this object before Effects are applied.
        /// </summary>
        public Dictionary<string, string> BaseAspects { get; init; } = new Dictionary<string, string>();

        /// <summary>
        /// Gets the Dynamic aspect values, calculated at the end of the generation process.
        /// </summary>
        public Dictionary<string, string> DynamicAspects { get; init; } = new Dictionary<string, string>();

        /// <summary>
        /// Gets all attribute effects on this thing. Expired effects should be culled on Step.
        /// </summary>
        public IList<AttributeEffect> AttributeEffects { get; init; } = new List<AttributeEffect>();

        /// <summary>
        /// Gets all aspect effects on this thing. Expired effects should be culled on Step.
        /// </summary>
        public IList<AspectEffect> AspectEffects { get; init; } = new List<AspectEffect>();

        /// <summary>
        /// Gets all effects on this thing.
        /// </summary>
        public IReadOnlyList<BaseEffect> Effects => this.AttributeEffects.OfType<BaseEffect>().Concat(this.AspectEffects).ToList();

        /// <summary>
        /// Gets the random number generator to use during construction finalization.
        /// </summary>
        protected Random? FinalizationRandom { get; private set; }

        /// <summary>
        /// Creates a clone of this thing incrementing the step count by one.
        /// </summary>
        /// <returns>A clone of this with everything aged by one step.</returns>
        public BaseThing CreateAgedClone()
        {
            // Clone the thing with an empty effect list.
            BaseThing newThing = this with
            {
                AttributeEffects = new List<AttributeEffect>(),
                AspectEffects = new List<AspectEffect>(),
                DynamicAspects = new Dictionary<string, string>(),
                DynamicAttributes = new Dictionary<string, int>(),
                State = ThingState.Constructing,
            };

            foreach (AttributeEffect effect in this.AttributeEffects)
            {
                if (effect.Duration == -1)
                {
                    newThing.AttributeEffects.Add(effect with { });
                }
                else if (effect.Duration >= 1)
                {
                    newThing.AttributeEffects.Add(effect with
                    {
                        Duration = effect.Duration - 1,
                    });
                }
            }

            foreach (AspectEffect effect in this.AspectEffects)
            {
                if (effect.Duration == -1)
                {
                    newThing.AspectEffects.Add(effect with { });
                }
                else if (effect.Duration >= 1)
                {
                    newThing.AspectEffects.Add(effect with
                    {
                        Duration = effect.Duration - 1,
                    });
                }
            }

            return newThing;
        }

        /// <summary>
        /// Gets the effects which are modifying the specified attribute.
        /// </summary>
        /// <param name="attribute">The attribute to get.</param>
        /// <returns>The list of all effects which are modifying this specified attribute.</returns>
        public IEnumerable<AttributeEffect> GetAttributeEffectsModifying(string attribute)
        {
            return this.AttributeEffects.Where(a => a.Attribute.Equals(attribute));
        }

        /// <summary>
        /// Gets the effects which are modifying the specified attribute.
        /// </summary>
        /// <param name="aspect">The aspect to get.</param>
        /// <returns>The list of all effects which are modifying this specified aspect.</returns>
        public IEnumerable<AspectEffect> GetAspectEffectsModifying(string aspect)
        {
            return this.AspectEffects.Where(a => a.Aspect.Equals(aspect));
        }

        /// <summary>
        /// Gets the effect which is setting the given aspect.
        /// </summary>
        /// <param name="aspect">The aspect to get.</param>
        /// <returns>The effect which is setting the aspect, or null if none are.</returns>
        public AspectEffect? GetCurrentAspectEffect(string aspect)
        {
            var aspectEffects = this.GetAspectEffectsModifying(aspect);

            if (!aspectEffects.Any())
            {
                return null;
            }

            int maxAge = aspectEffects.Max(X => X.TookEffect);
            return aspectEffects.First(x => x.TookEffect == maxAge);
        }

        /// <summary>
        /// Calculates the effective attribute value based on the current effects.
        /// </summary>
        /// <param name="attribute">The attribute to get.</param>
        /// <param name="defaultValue">The default value, if the attribute does not exist.</param>
        /// <returns>The effective attribute value.</returns>
        public int EffectiveAttribute(string attribute, int defaultValue = 0)
        {
            if (this.State == ThingState.Constructing)
            {
                throw new InvalidOperationException("Can not read attributes when thing is being constructed.");
            }

            if (!this.BaseAttributes.TryGetValue(attribute, out int value))
            {
                value = defaultValue;
            }

            if (!this.DynamicAttributes.TryGetValue(attribute, out int dynamic))
            {
                dynamic = 0;
                if (this.State == ThingState.FinalizingConstruction && this.FinalizationRandom != null)
                {
                    // We need to calculate and set the dynamic value, if it has one.
                    // If there's no attribute definition for this thing, than we can't calculate the dynamic value at all.
                    if (this.BaseDefinition.Attributes.TryGetValue(attribute, out AttributeDefinition? def))
                    {
                        dynamic = def.EvalDynamicValue(this.FinalizationRandom, this);
                        this.DynamicAttributes[attribute] = dynamic;
                    }
                }
            }

            return SafeSum(value + dynamic, this.GetAttributeEffectsModifying(attribute).Select(x => x.Manitude));
        }

        /// <summary>
        /// Calculates the effective aspect value based on the current effects.
        /// </summary>
        /// <param name="aspect">The aspect to get.</param>
        /// <param name="defaultValue">The default value, if the aspect does not exist.</param>
        /// <returns>The effective aspect value.</returns>
        public string EffectiveAspect(string aspect, string defaultValue)
        {
            return this.EffectiveAspect(aspect) ?? defaultValue;
        }

        /// <summary>
        /// Calculates the effective aspect value based on the current effects.
        /// </summary>
        /// <param name="aspect">The aspect to get.</param>
        /// <returns>The effective aspect value.</returns>
        public string? EffectiveAspect(string aspect)
        {
            if (this.State == ThingState.Constructing)
            {
                throw new InvalidOperationException("Can not read aspects when thing is being constructed.");
            }

            if (!this.BaseAspects.TryGetValue(aspect, out string? value))
            {
                value = null;
            }

            if (!this.DynamicAspects.TryGetValue(aspect, out string? dynamic))
            {
                dynamic = null;
                if (this.State == ThingState.FinalizingConstruction && this.FinalizationRandom != null)
                {
                    // We need to calculate and set the dynamic value, if it has one.
                    // If there's no aspect definition for this thing, than we can't calculate the dynamic value at all.
                    if (this.BaseDefinition.Aspects.TryGetValue(aspect, out AspectDefinition? def) && def.Dynamic)
                    {
                        dynamic = def.EvalDynamicValueSafe(this.FinalizationRandom, this);
                        this.DynamicAspects[aspect] = dynamic;
                    }
                }
            }

            var effect = this.GetCurrentAspectEffect(aspect);

            return effect?.Value ?? dynamic ?? value ?? null;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder($"{ThingType} {Name}");
            sb.AppendLine();

            foreach (AttributeEffect effect in this.AttributeEffects)
            {
                sb.AppendLine($"  {effect.Title} ({effect.Duration}): {effect.Attribute} {effect.Manitude}");
            }

            foreach (AspectEffect effect in this.AspectEffects)
            {
                sb.AppendLine($"  {effect.Title} ({effect.Duration}): {effect.Aspect} {effect.Value}");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Finalizes the construction of this object.
        /// </summary>
        /// <param name="rdm">The random number generator.</param>
        public void FinalizeConstruction(Random rdm)
        {
            this.State = ThingState.FinalizingConstruction;
            this.FinalizationRandom = rdm;

            this.FinalizeConstruction();

            this.State = ThingState.Finalized;
            this.FinalizationRandom = null;
        }

        /// <summary>
        /// Finalizes the construction of this object.
        /// </summary>
        protected virtual void FinalizeConstruction()
        {
            foreach (var (name, _) in this.BaseDefinition.Attributes)
            {
                // Getting the effective attribute will calculate and set it's dynamic value.
                this.EffectiveAttribute(name);
            }

            foreach (var (name, _) in this.BaseDefinition.Aspects.Where(x => x.Value.Dynamic))
            {
                // Getting the effective aspect will calculate and set it's dynamic value.
                this.EffectiveAspect(name);
            }
        }

        /// <summary>
        /// Safely adds many values together, changing any overflow/underflow to the nearest value.
        /// </summary>
        /// <param name="startValue">The start value.</param>
        /// <param name="ints">The ints to add.</param>
        /// <returns>The summed integer, bounded by int min and max value.</returns>
        private static int SafeSum(int startValue, IEnumerable<int> ints)
        {
            if (!ints.Any())
            {
                return startValue;
            }

            BigInteger sum = startValue + ints.Select(x => new BigInteger(x)).Aggregate(BigInteger.Add);
            if (sum < int.MinValue)
            {
                return int.MinValue;
            }
            else if (sum > int.MaxValue)
            {
                return int.MaxValue;
            }
            else
            {
                return (int)sum;
            }
        }
    }
}
