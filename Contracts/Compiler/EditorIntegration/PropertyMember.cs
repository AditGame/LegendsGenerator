using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsGenerator.Contracts.Compiler.EditorIntegration
{
    public class PropertyMember : BaseTypeMember
    {
        public PropertyMember(string name, Type returnType)
            : base(name, returnType)
        {
        }

        public override bool RequiresParens => false;
    }
}
