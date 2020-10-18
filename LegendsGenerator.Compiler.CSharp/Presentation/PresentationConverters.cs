// -------------------------------------------------------------------------------------------------
// <copyright file="PresentationConverters.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Compiler.CSharp.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using LegendsGenerator.Contracts;
    using LegendsGenerator.Contracts.Things;

    /// <summary>
    /// Converts typical types to presentation types.
    /// </summary>
    internal static class PresentationConverters
    {
        /// <summary>
        /// The mapping of normal types to their associated presentation types, with the wrappers.
        /// </summary>
        private static IDictionary<Type, PresentationMapping> mapping = new Dictionary<Type, PresentationMapping>()
        {
            { typeof(BaseThing), new PresentationMapping<BaseThing, BaseThingPres>((site, world) => throw new InvalidOperationException("Can not create an instance of BaseThing presentation")) },
            { typeof(BaseMovingThing), new PresentationMapping<BaseMovingThing, BaseMovingThingPres>((site, world) => throw new InvalidOperationException("Can not create an instance of BaseThing presentation")) },
            { typeof(Site), new PresentationMapping<Site, SitePres>((site, world) => new SitePres(site, world)) },
            { typeof(WorldSquare), new PresentationMapping<WorldSquare, WorldSquarePres>((square, world) => new WorldSquarePres(world.Grid.GetSquare(square.X, square.Y), world)) },
            { typeof(NotablePerson), new PresentationMapping<NotablePerson, NotablePersonPres>((person, world) => new NotablePersonPres(person, world)) },
        };

        /// <summary>
        /// Converts a normal type to it's presentation type.
        /// </summary>
        /// <param name="input">The input normal type.</param>
        /// <param name="world">The world object.</param>
        /// <param name="output">The output, if applicable.</param>
        /// <returns>True if successfully converted, false if no conversion is possible.</returns>
        public static object ConvertToPresentationType(object input, World world)
        {
            return GetMapping(input.GetType()).WrapAsPresentation(input, world);
        }

        /// <summary>
        /// Converts a normal type to it's presentation type.
        /// </summary>
        /// <param name="input">The input normal type.</param>
        /// <param name="world">The world object.</param>
        /// <param name="output">The output, if applicable.</param>
        /// <returns>True if successfully converted, false if no conversion is possible.</returns>
        public static bool TryConvertToPresentationType(object input, World world, [NotNullWhen(true)] out object? output)
        {
            if (TryGetMapping(input.GetType(), out PresentationMapping? map))
            {
                output = map.WrapAsPresentation(input, world);
                return true;
            }

            output = null;
            return false;
        }

        /// <summary>
        /// Tries to get the presentation type associated with a normal type.
        /// </summary>
        /// <param name="input">The input type.</param>
        /// <param name="output">The matching presentation type.</param>
        /// <returns>True if a matching presentation type exists, false otherwise.</returns>
        public static bool TryGetPresentationType(Type input, [NotNullWhen(true)] out Type? output)
        {
            if (TryGetMapping(input, out PresentationMapping? map))
            {
                output = map.PresentationType;
                return true;
            }

            output = null;
            return false;
        }

        /// <summary>
        /// Gets the mapping for the specified map.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="output">The matching output.</param>
        /// <returns>True if a matching mapping exists, false otherwise.</returns>
        private static bool TryGetMapping(Type type, [NotNullWhen(true)] out PresentationMapping? output)
        {
            if (mapping.TryGetValue(type, out output))
            {
                return true;
            }

            output = null;
            return false;
        }

        /// <summary>
        /// Gets the mapping for the specified map.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The presentation mapping.</returns>
        private static PresentationMapping GetMapping(Type type)
        {
            if (mapping.TryGetValue(type, out PresentationMapping? presMap))
            {
                return presMap;
            }

            throw new InvalidOperationException($"Conversation of {type} to presentation is not supported.");
        }

        /// <summary>
        /// A mapping of object to presentation type.
        /// </summary>
        private class PresentationMapping
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="PresentationMapping"/> class.
            /// </summary>
            /// <param name="presentationType">The type of this presentation.</param>
            /// <param name="wrapAsPresentation">The function to convert a normal, matching object to a presentation object.</param>
            public PresentationMapping(Type presentationType, Func<object, World, object> wrapAsPresentation)
            {
                this.PresentationType = presentationType;
                this.WrapAsPresentation = wrapAsPresentation;
            }

            /// <summary>
            /// Gets the type for this presentation.
            /// </summary>
            public Type PresentationType { get; }

            /// <summary>
            /// Gets the converter which turns a normal object into a presentation object.
            /// </summary>
            public Func<object, World, object> WrapAsPresentation { get; }
        }

        /// <summary>
        /// A mapping of object to presentation type.
        /// </summary>
        /// <typeparam name="TNormal">The normal type.</typeparam>
        /// <typeparam name="TPresentation">The presentation type.</typeparam>
        private class PresentationMapping<TNormal, TPresentation> : PresentationMapping
            where TNormal : notnull
            where TPresentation : notnull
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="PresentationMapping{TNormal, TPresentation}"/> class.
            /// </summary>
            /// <param name="wrapAsPresentation">THe function to wrap a normal object as a presentation object.</param>
            public PresentationMapping(Func<TNormal, World, TPresentation> wrapAsPresentation)
                : base(typeof(TPresentation), (x, world) => SafeWrap(x, world, wrapAsPresentation))
            {
            }

            /// <summary>
            /// Wraps the typed wrapper in a non-typed function.
            /// </summary>
            /// <param name="input">The input.</param>
            /// <param name="world">The world object.</param>
            /// <param name="wrapper">The typed wrapping function.</param>
            /// <returns>The wrapped value.</returns>
            private static object SafeWrap(object input, World world, Func<TNormal, World, TPresentation> wrapper)
            {
                if (input is not TNormal normalObj)
                {
                    throw new InvalidOperationException($"Invalid object passed in for wrapping, expected {typeof(TNormal)} got {input.GetType()}");
                }

                return wrapper(normalObj, world);
            }
        }
    }
}
