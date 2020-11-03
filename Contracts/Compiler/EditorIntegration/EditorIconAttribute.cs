using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsGenerator.Contracts.Compiler.EditorIntegration
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class EditorIconAttribute : Attribute
    {
        /// <summary>
        /// Constructor yay.
        /// </summary>
        /// <param name="icon">The icon to show.</param>
        public EditorIconAttribute(EditorIcon icon)
        {
            this.Icon = icon;
        }

        /// <summary>
        /// Gets the icon to display with this property.
        /// </summary>
        public EditorIcon Icon { get; }
    }
}
