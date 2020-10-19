using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsGenerator.Contracts.Compiler.EditorIntegration
{
    public class MethodParameter
    {
        public MethodParameter(string name, Type type)
        {
            this.Name = name;
            this.Type = type;
        }

        /// <summary>
        /// Gets the name of this parameter.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the type of this parameter.
        /// </summary>
        public Type Type { get; }
    }
}
