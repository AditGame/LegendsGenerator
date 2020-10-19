using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsGenerator.Contracts.Compiler.EditorIntegration
{
    public class MethodMember : BaseTypeMember
    {
        public MethodMember(string name, Type returnType, IList<MethodParameter> parameters)
            : base(name, returnType)
        {
            this.Parameters = parameters;
        }

        public IList<MethodParameter> Parameters { get; }

        public override bool RequiresParens => true;
    }
}
