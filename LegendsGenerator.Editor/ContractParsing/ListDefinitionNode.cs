using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LegendsGenerator.Editor.ContractParsing
{
    public class ListDefinitionNode : DefinitionNode, ICreatable
    {
        private ElementInfo info;

        private object underlyingObject;

        private Type objectType;

        public ListDefinitionNode(object? thing, ElementInfo info, IEnumerable<PropertyInfo> options, bool readOnly = false)
            : base(thing, info, options, readOnly)
        {
            this.info = info;
            this.underlyingObject = thing;
            this.objectType = info.PropertyType.GenericTypeArguments.First();

            this.CreateNodes();
        }

        /// <inheritdoc/>
        public void HandleCreate(object sender, RoutedEventArgs e)
        {
            object def;
            if (this.objectType == typeof(string))
            {
                def = string.Empty;
            }
            else
            {
                def = Activator.CreateInstance(this.objectType);
            }

            (this.Content as IList).Add(def);
            this.CreateNodes();
        }

        private void CreateNodes()
        {
            this.Nodes.Clear();

            IList list = this.Content as IList;

            for (int i = 0; i < list.Count; i++)
            {
                int iCopy = i;
                object? value = list[i];

                ElementInfo kvpInfo = new ElementInfo()
                {
                    Name = $"[{i}]",
                    Description = this.Description,
                    Compiled = this.info.Compiled,
                    Nullable = false,
                    PropertyType = this.objectType,
                    GetMethod = () => (info.GetMethod() as IList)[iCopy],
                    SetMethod = value => (info.GetMethod() as IList)[iCopy] = value,
                };

                this.Nodes.Add(DefinitionParser.ToNode(this.underlyingObject, kvpInfo));
            }
        }
    }
}
