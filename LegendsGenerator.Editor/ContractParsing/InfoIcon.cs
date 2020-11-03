// -------------------------------------------------------------------------------------------------
// <copyright file="InfoIcon.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor.ContractParsing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using LegendsGenerator.Contracts.Compiler.EditorIntegration;

    /// <summary>
    /// An icon to show with a property.
    /// </summary>
    public sealed class InfoIcon
    {
        /// <summary>
        /// The mapping of editoricon enum to infoicon.
        /// </summary>
        private static IDictionary<EditorIcon, InfoIcon>? mapping;

        /// <summary>
        /// The lock around creating the map.
        /// </summary>
        private static object mapInitLock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="InfoIcon"/> class.
        /// </summary>
        /// <param name="icon">The icon enum.</param>
        /// <param name="resource">The resource to display.</param>
        /// <param name="tooltip">The tooltip to display.</param>
        private InfoIcon(EditorIcon icon, ImageBrush resource, string tooltip)
        {
            this.Icon = icon;
            this.Resource = resource;
            this.Tooltip = tooltip;
        }

        /// <summary>
        /// Gets the Compiled Static value.
        /// </summary>
        public static InfoIcon CompiledStatic => new InfoIcon(
            EditorIcon.CompiledStatic,
            new ImageBrush(new BitmapImage(new Uri(@"resources\Static.png", UriKind.Relative))),
            "This condition will be evaluated at Thing creation time and the value stored on the object. The value will never be reevaluated during the lifetime of the object.");

        /// <summary>
        /// Gets the Compiled Dynamic value.
        /// </summary>
        public static InfoIcon CompiledDynamic => new InfoIcon(
            EditorIcon.CompiledDynamic,
            new ImageBrush(new BitmapImage(new Uri(@"resources\Dynamic.png", UriKind.Relative))),
            "This condition will be reevaluated with every 'step' and cached on the object. You can use this as a derived value which will changed based on other values.");

        /// <summary>
        /// Gets the tooltip to show with it.
        /// </summary>
        public string Tooltip { get; }

        /// <summary>
        /// Gets the icon itself to show in the editor.
        /// </summary>
        public ImageBrush Resource { get; }

        /// <summary>
        /// Gets the EditorIcon enum associated with this.
        /// </summary>
        public EditorIcon Icon { get; }

        /// <summary>
        /// Gets the InfoIcon associated with the EditorIcon enum.
        /// </summary>
        /// <param name="icon">The icon enum.</param>
        /// <returns>The matching InfoIcon.</returns>
        public static InfoIcon Get(EditorIcon icon)
        {
            if (mapping == null)
            {
                lock (mapInitLock)
                {
                    if (mapping == null)
                    {
                        IDictionary<EditorIcon, InfoIcon> map = new Dictionary<EditorIcon, InfoIcon>();
                        foreach (PropertyInfo prop in typeof(InfoIcon)
                            .GetProperties(BindingFlags.Public | BindingFlags.Static)
                            .Where(p => p.PropertyType == typeof(InfoIcon)))
                        {
                            InfoIcon? iconInst = prop.GetValue(null) as InfoIcon;
                            if (iconInst is not null)
                            {
                                if (map.ContainsKey(iconInst.Icon))
                                {
                                    throw new InvalidOperationException($"The EditorIcon {iconInst.Icon} is duplicated in InfoIcon.");
                                }

                                map[iconInst.Icon] = iconInst;
                            }
                        }

                        mapping = map;
                    }
                }
            }

            if (!mapping.TryGetValue(icon, out InfoIcon? value))
            {
                throw new InvalidOperationException($"The EditorIcon {icon} does not exist in InfoIcon.");
            }

            return value;
        }
    }
}
