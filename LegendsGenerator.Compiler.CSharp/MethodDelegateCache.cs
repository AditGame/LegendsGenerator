// <copyright file="MethodDelegateCache.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Compiler.CSharp
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using CSScriptLib;

    /// <summary>
    /// The cache of method delegates based on content.
    /// </summary>
    /// <typeparam name="T">The return type.</typeparam>
#pragma warning disable CA1000 // Do not declare static members on generic types. Intentional for speed purposes.
    public static class MethodDelegateCache<T>
    {
        /// <summary>
        /// The cache instance.
        /// </summary>
        private static readonly Dictionary<string, MethodDelegate<T>> Cache =
            new Dictionary<string, MethodDelegate<T>>();

        /// <summary>
        /// Gets a method delete.
        /// </summary>
        /// <param name="content">The script to create a method delegate.</param>
        /// <returns>The method delegate.</returns>
        public static MethodDelegate<T> Get(
            string content)
        {
            if (Cache.TryGetValue(content, out MethodDelegate<T>? d))
            {
                return d;
            }

            try
            {
                var entry = CSScript.RoslynEvaluator.CreateDelegate<T>(content);

                Cache[content] = entry;
                return entry;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed compiling code: " + content, ex);
            }
        }
    }
#pragma warning restore CA1000 // Do not declare static members on generic types
}
