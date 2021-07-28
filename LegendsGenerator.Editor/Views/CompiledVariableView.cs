// -------------------------------------------------------------------------------------------------
// <copyright file="CompiledVariableView.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor.Views
{
    using System.Collections.Generic;
    using LegendsGenerator.Contracts.Compiler;
    using LegendsGenerator.Contracts.Compiler.EditorIntegration;

    /// <summary>
    /// A variable which can be used in a compiled statement.
    /// </summary>
    public class CompiledVariableView
    {
        /// <summary>
        /// The inner compiled variable.
        /// </summary>
        private readonly CompiledVariable inner;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompiledVariableView"/> class.
        /// </summary>
        /// <param name="inner">The inner compiled variable.</param>
        public CompiledVariableView(CompiledVariable inner)
        {
            this.inner = inner;
        }

        /// <summary>
        /// Gets the name of this object.
        /// </summary>
        public string Name => this.inner.ToString();

        /// <summary>
        /// Gets infomation about the variable.
        /// </summary>
        public IList<BaseTypeMember> Tooltip => Context.LastLoadedInstance.Compiler.EditorIntegration.GetPublicMembers(this.inner.Type);
    }
}
