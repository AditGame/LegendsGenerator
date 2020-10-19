// -------------------------------------------------------------------------------------------------
// <copyright file="CompiledVariableView.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor.Views
{
    using LegendsGenerator.Contracts.Compiler;
    using LegendsGenerator.Contracts.Compiler.EditorIntegration;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CompiledVariableView
    {
        private readonly CompiledVariable inner;

        public CompiledVariableView(CompiledVariable inner)
        {
            this.inner = inner;
        }

        public string Name => this.inner.ToString();

        public IList<BaseTypeMember> Tooltip => Context.Instance.Compiler.EditorIntegration.GetPublicMembers(this.inner.Type);
    }
}
