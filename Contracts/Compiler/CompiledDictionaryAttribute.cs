using System;

namespace LegendsGenerator.Contracts.Compiler
{
    /// <summary>
    /// Indicates the property's values should be compiled.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class CompiledDictionaryAttribute : CompiledAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompiledDictionaryAttribute"/> class.
        /// </summary>
        /// <param name="returnType">The return type of the compiled statement.</param>
        public CompiledDictionaryAttribute(Type returnType)
            : base(returnType)
        {
        }
    }
}
