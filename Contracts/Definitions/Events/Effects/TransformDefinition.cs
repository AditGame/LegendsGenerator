using LegendsGenerator.Contracts.Compiler;
using LegendsGenerator.Contracts.Definitions.Validation;
using LegendsGenerator.Contracts.Things;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsGenerator.Contracts.Definitions.Events.Effects
{
    [UsesAdditionalParametersForHoldingClass]
    public partial class TransformDefinition : BaseEffectDefinition
    {
        /// <summary>
        /// Gets or sets the definition name to transform this object to, if not set the old definition will be retained.
        /// </summary>
        public string? ChangeDefinitionName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the attributes and aspects SHOULD NOT be retained on transform. If true, they will be recalculated from the definition.
        /// </summary>
        [HideInEditor("value.ChangeDefinitionName == null")]
        public bool ResetAttributesAndAspects { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether effects should be cleared on transform. If true, all effects will immediately end.
        /// </summary>
        public bool ResetEffects { get; set; }

        /// <summary>
        /// Gets the attribute overrides for this spawn.
        /// </summary>
        [CompiledDictionary(typeof(int))]
        [CompiledVariable(EventDefinition.SubjectVarName, typeof(BaseThing))]
        public Dictionary<string, string> AttributeOverrides { get; } = new Dictionary<string, string>();

        /// <summary>
        /// Gets the aspect overrides for this spawn.
        /// </summary>
        [CompiledDictionary(typeof(string))]
        [CompiledVariable(EventDefinition.SubjectVarName, typeof(BaseThing))]
        public Dictionary<string, string> AspectOverrides { get; } = new Dictionary<string, string>();
    }
}
