using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsGenerator.Contracts.Compiler
{
    /// <summary>
    /// Indicates the property's values should be compiled.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CompiledDictionaryAttribute : CompiledAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompiledDictionaryAttribute"/> class.
        /// </summary>
        /// <param name="returnType">The return type of the compiled statement.</param>
        /// <param name="parameterNames">The names of the parameters expected in this compiled statement.</param>
        public CompiledDictionaryAttribute(Type returnType, params string[] parameterNames)
            : base(returnType, parameterNames)
        {
        }
    }
}
