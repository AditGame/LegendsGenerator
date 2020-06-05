using LegendsGenerator.Contracts.Compiler;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsGenerator.Editor.ContractParsing
{
    public class ElementInfo
    {
        public string Name { get; set; }

        public Func<object?> GetMethod { get; set; }

        public Action<object?> SetMethod { get; set; }

        public Func<IList<string>> GetParametersMethod { get; set; }

        public bool Nullable { get; set; }

        public CompiledAttribute? Compiled { get; set; }

        public string Description { get; set; }

        public Type PropertyType { get; set; }
    }
}
