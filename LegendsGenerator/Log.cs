// <copyright file="Log.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator
{
    using System.Diagnostics;

    /// <summary>
    /// Class with logging methods.
    /// </summary>
    internal static class Log
    {
        /// <summary>
        /// Gets the trace source.
        /// </summary>
        public static TraceSource Ts { get; } = new TraceSource("LegendsGenerator");

        /// <summary>
        /// Logs information.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args, if any.</param>
        public static void Info(string format, params object[] args)
        {
            Ts.TraceEvent(TraceEventType.Information, 0, format, args);
        }

        /// <summary>
        /// Logs warning.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args, if any.</param>
        public static void Warning(string format, params object[] args)
        {
            Ts.TraceEvent(TraceEventType.Warning, 0, format, args);
        }

        /// <summary>
        /// Logs error.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args, if any.</param>
        public static void Error(string format, params object[] args)
        {
            Ts.TraceEvent(TraceEventType.Error, 0, format, args);
        }
    }
}
