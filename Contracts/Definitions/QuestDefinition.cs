using LegendsGenerator.Contracts.Compiler;
using LegendsGenerator.Contracts.Definitions.Events;
using LegendsGenerator.Contracts.Definitions.Validation;
using LegendsGenerator.Contracts.Things;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsGenerator.Contracts.Definitions
{
    public partial class QuestDefinition : BaseThingDefinition
    {
        /// <inheritdoc/>
        public override ThingType ThingType => ThingType.Quest;

        /// <summary>
        /// Gets or sets the name of the thing, such as Scholar.
        /// </summary>
        [ControlsDefinitionName]
        [Compiled(typeof(string), AsFormattedText = true)]
        [CompiledVariable("Subject", typeof(BaseThing))]
        public string Name { get; set; } = UnsetString;
    }
}
