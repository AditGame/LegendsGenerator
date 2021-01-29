// <copyright file="ThingFactory.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator
{
    using System;
    using LegendsGenerator.Contracts.Definitions;
    using LegendsGenerator.Contracts.Definitions.Events;
    using LegendsGenerator.Contracts.Things;

    /// <summary>
    /// Interface for a thing which creates things.
    /// </summary>
    public interface IThingFactory
    {
        /// <summary>
        /// The definitions.
        /// </summary>
        Definitions Definitions { get; }

        /// <summary>
        /// Creates a thing.
        /// </summary>
        /// <param name="rdm">The random number generator.</param>
        /// <param name="x">The X coordinate of the site.</param>
        /// <param name="y">The Y coordinate of the site.</param>
        /// <param name="type">The type of thing to create.</param>
        /// <param name="definitionName">The name of the definition to create.</param>
        /// <returns>The created thing.</returns>
        BaseThing CreateThing(Random rdm, int x, int y, ThingType type, string definitionName);

        /// <summary>
        /// Creates a notable person.
        /// </summary>
        /// <param name="rdm">The random number generator.</param>
        /// <param name="x">The X coordinate of the person.</param>
        /// <param name="y">The Y coordinate of the person.</param>
        /// <param name="personDefinitionName">The name of the person's definition.</param>
        /// <returns>The notable person.</returns>
        NotablePerson CreateNotablePerson(Random rdm, int x, int y, string personDefinitionName);

        /// <summary>
        /// Creates a site.
        /// </summary>
        /// <param name="rdm">The random number generator.</param>
        /// <param name="x">The X coordinate of the site.</param>
        /// <param name="y">The Y coordinate of the site.</param>
        /// <param name="siteDefinitionName">The name of the site's definition.</param>
        /// <returns>The site.</returns>
        Site CreateSite(Random rdm, int x, int y, string siteDefinitionName);

        /// <summary>
        /// Creates a notable person.
        /// </summary>
        /// <param name="rdm">The random number generator.</param>
        /// <param name="x">The X coordinate of the person.</param>
        /// <param name="y">The Y coordinate of the person.</param>
        /// <param name="worldSquareDefinitionName">The name of the world square's definition.</param>
        /// <returns>The world square.</returns>
        WorldSquare CreateWorldSquare(Random rdm, int x, int y, string worldSquareDefinitionName);

        /// <summary>
        /// Creates a world.
        /// </summary>
        /// <param name="rdm">The random number generator.</param>
        /// <param name="x">The X coordinate of the person.</param>
        /// <param name="y">The Y coordinate of the person.</param>
        /// <param name="worldDefinitionName">The name of the world definition.</param>
        /// <returns>The world.</returns>
        WorldThing CreateWorld(Random rdm, int x, int y, string worldDefinitionName);

        /// <summary>
        /// Creates a quest.
        /// </summary>
        /// <param name="rdm">The random number generator.</param>
        /// <param name="x">The X coordinate of the person.</param>
        /// <param name="y">The Y coordinate of the person.</param>
        /// <param name="questDefinitionName">The name of the quest definition.</param>
        /// <returns>The quest.</returns>
        Quest CreateQuest(Random rdm, int x, int y, string questDefinitionName);

        /// <summary>
        /// Creates a unit.
        /// </summary>
        /// <param name="rdm">The random number generator.</param>
        /// <param name="x">The X coordinate of the person.</param>
        /// <param name="y">The Y coordinate of the person.</param>
        /// <param name="unitDefinitionName">The name of the unit definition.</param>
        /// <returns>The unit.</returns>
        Unit CreateUnit(Random rdm, int x, int y, string unitDefinitionName);
    }
}