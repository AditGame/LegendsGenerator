// -------------------------------------------------------------------------------------------------
// <copyright file="DescriptionProvider.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor.ContractParsing
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;

    /// <summary>
    /// Provides the description for a member.
    /// </summary>
    public static class DescriptionProvider
    {
        /// <summary>
        /// The cache of documentation files.
        /// </summary>
        private static IDictionary<string, XmlDocument> docCache = new Dictionary<string, XmlDocument>();

        /// <summary>
        /// The cache of descriptions.
        /// </summary>
        private static IDictionary<string, string?> descriptionCache = new Dictionary<string, string?>();

        /// <summary>
        /// Gets the descritpion, if it exists, for the specified property.
        /// </summary>
        /// <param name="property">the property.</param>
        /// <returns>The description.</returns>
        public static string GetDescription(MemberInfo property)
        {
            string fullName = $"{property.DeclaringType?.FullName}.{property.Name}";

            if (descriptionCache.TryGetValue(fullName, out string? description))
            {
                return description ?? string.Empty;
            }

            DescriptionAttribute? descAttribute = property.GetCustomAttribute<DescriptionAttribute>();
            if (descAttribute != null)
            {
                description = descAttribute.Description;
            }
            else
            {
                description = GetFromDocumentationFile(fullName, property);
            }

            descriptionCache[fullName] = description;
            return description ?? string.Empty;
        }

        /// <summary>
        /// Gets the description from the documentation file.
        /// </summary>
        /// <param name="fullName">The full name of the property.</param>
        /// <param name="property">The property.</param>
        /// <returns>The documentation string, if it exists.</returns>
        private static string? GetFromDocumentationFile(string fullName, MemberInfo property)
        {
            const string BoolDoc = "Gets or sets a value indicating";
            const string ReadWriteDoc = "Gets or sets";
            const string ReadDoc = "Gets";

            string xpath = $"//member[@name='P:{fullName}']";

            string? assemblyLocation = property.DeclaringType?.Assembly.Location;

            if (assemblyLocation == null)
            {
                // the assembly doesn't exist, can't get a doc from that.
                return null;
            }

            string docLocation = Path.ChangeExtension(assemblyLocation, "xml");

            if (!docCache.TryGetValue(docLocation, out XmlDocument? document))
            {
                document = new XmlDocument();
                document.Load(docLocation);
                docCache[docLocation] = document;
            }

            string summary;
            try
            {
                summary = document.SelectSingleNode(xpath).SelectSingleNode("summary").InnerText.Trim();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }

            string summaryWithoutStart = summary;
            if (summary.StartsWith(BoolDoc, StringComparison.OrdinalIgnoreCase))
            {
                summaryWithoutStart = summary.Substring(BoolDoc.Length).Trim();
            }
            else if (summary.StartsWith(ReadWriteDoc, StringComparison.OrdinalIgnoreCase))
            {
                summaryWithoutStart = summary.Substring(ReadWriteDoc.Length).Trim();
            }
            else if (summary.StartsWith(ReadDoc, StringComparison.OrdinalIgnoreCase))
            {
                summaryWithoutStart = summary.Substring(ReadDoc.Length).Trim();
            }

            return char.ToUpper(summaryWithoutStart[0]) + summaryWithoutStart.Substring(1);
        }
    }
}
