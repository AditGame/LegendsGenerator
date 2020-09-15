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
        public static TraceSource Ts { get; } = new TraceSource("LegendsGenerator", SourceLevels.All);

        /// <summary>
        /// Logs information.
        /// </summary>
        /// <param name="format">The format.</param>
        public static void Info(string format)
        {
            Ts.TraceEvent(TraceEventType.Information, 0, format);
        }

        /// <summary>
        /// Logs warning.
        /// </summary>
        /// <param name="format">The format.</param>
        public static void Warning(string format)
        {
            Ts.TraceEvent(TraceEventType.Warning, 0, format);
        }

        /// <summary>
        /// Logs error.
        /// </summary>
        /// <param name="format">The format.</param>
        public static void Error(string format)
        {
            Ts.TraceEvent(TraceEventType.Error, 0, format);
        }
    }
}
