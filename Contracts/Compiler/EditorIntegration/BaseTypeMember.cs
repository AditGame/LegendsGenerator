using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsGenerator.Contracts.Compiler.EditorIntegration
{
    public abstract class BaseTypeMember
    {
        public BaseTypeMember(string name, Type returnType)
        {
            this.Name = name;
            this.ReturnType = returnType;
        }

        public string Name { get; set; }

        public Type ReturnType { get; }

        public abstract bool RequiresParens { get; }
    }
}
