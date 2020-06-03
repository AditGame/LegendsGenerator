using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LegendsGenerator.Editor.ContractParsing
{
    public class EnumDefinitionNode : DefinitionNode
    {
        private ComboBox box = new ComboBox();

        public EnumDefinitionNode(
            string description,
            string name,
            bool nullable,
            Type enumType,
            Func<Enum> getValue,
            Action<Enum> setValue)
            : base(description, nullable)
        {
            this.Name = name;
            this.GetContentsFunc = getValue;

            if (setValue != null)
            {
                this.ContentsModifiable = true;
                this.SetContentsFunc = ConvertAction(setValue);
            }

            this.box.ItemsSource = Enum.GetNames(enumType);

            this.box.SelectedIndex = (int) Convert.ChangeType(getValue(), typeof(int));
        }

        /// <inheritdoc/>
        public override string Name { get; }

        /// <inheritdoc/>
        public override UIElement GetContentElement()
        {
            return this.box;
        }
    }
}
