using LegendsGenerator.Contracts.Definitions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsGenerator.Contracts.Definitions
{
    public partial class WorldDefinition : BaseThingDefinition
    {
        public override ThingType ThingType => ThingType.World;
    }
}
