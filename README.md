# Legends Generator

Creates the history of a world one step at a time.

🚧 This library is very WIP. Any item with 🚧 next to it is still a work in progress and may not be done yet.

Don't rely on literally anything staying static here until we hit version 1.0.

## Description

The legends generator uses an initial state of the world to advance the history of the world, returning a copy of the world advanced by one Step.

![Overview of History Generator](Docs\Overview.png)

## Definitions

* Step: The smallest unit of calculated time. Subvision of this time, from a historical perspective, is not possible. Typically a step should be approx one week, however the exact value is by convention of the Definition files.
* World: A state of the world at any given step. The world state will be mutated by the history generator in a Step. Each Step has it's own distinct world.
* Grid: The world is subdivided into a grid of squares. Each square is the smallest subdivision of position. Typically this is 1 mile.
* Square: A single X/Y on the grid, the smallest subvision of space.
* Thing: A thing is anything (square of land, building, lake, person, notable crocodile, army divison) which occurs on the map.
* Attribute: Each Thing has a list of attributes, which are string:int pairs. These attributes are free form and are used to define what the thing is. Typically attributes are modified by Effects which chagne the attribute value in a timed perspective, however direct modification of the attribute is certainly possible. THese are typically thigns like health, strength, population.
* Aspect: 🚧 Each thing has a list of aspects, which are string:string pairs. These aspects define what the thing is, an are set instead of modified. These are things like race, 
* Event: Events describe change in the world. Each event has a subject, 0-to-n objects, and effects. Events are randomly generated during the Step generation process and applied to the Things on the map.
* Forced Event: 🚧 These events are injected into the world state for the next generation process to evalulate, preferring them over all other possible events ensuring they occur. These are used to model player interractions into the world so their effects occur naturally on the world stage.
* Effect: An effect of an event describes the change in the world. They modify an attribute/aspect, create/destroy/change a Thing, start movement, or a variety of other things. Any effect can do any number of things, an event can thus have massive effects on multiple things on the map.
* Definition: Define how things are generated in the world. There are two major types:
   * Thing Definition: Define randomly created initial values for a given thing, as well as other static parts of the generated thing.
   * Event Definition: Defines an event, who is involved in the event, the effects of the event, and the chances of the event occurring.
* Condition: Used extensively in Definitions, are code snippets evalulated at runtime.

### Thing Types

There are multiple types of things with small but essential differences between them.

* Site (eg Building, lake, city): a static location on the map which does not move.
* Notable Person (eg Settler, Mayor): A person who can move arround the map
* Unit (eg Millitary Unit): 🚧 A collection of something. Units are typically (but not always) lead by a notable person.


## Tools

There are tools available for the development of Definition files.
* Editor: Allows easy editing and some validation of the definition files.
* Viewer: Allows viewing of the world state, specifically designed to play with the world as you develop the definition files.

Combined these tools comprise a very simple IDE for development of the Definition files.

## Basic use

There are 3 things needed to start the generator:

* A directory full of Definition json files.
* An initial world state.
* A condition compiler (We provide a C# one with LegendsGenerator.Compiler.CSharp). This needs to match the conditions you use in your Definition json files.

Once you have those three things, you can iterate the world state as much as you'd like to obtain a new world state.

```csharp
// Initialize your choice of condition compiler.
ConditionCompiler processor = new ConditionCompiler(new Dictionary<string, object>());

// Select the directory full of definitions.
string definitionsDirectory = "Definitions";

// Initialize the history generator.
HistoryGenerator history = new HistoryGenerator(definitionsDirectory, proces);

// Create some initial world state (in this case, an empty world).
World world = new World()
{
    WorldSeed = worldSeed,
    Grid = new LegendsGenerator.Grid(20, 20),
};

// Iterate the world state to step 2, using the definitions.
World world2 = history.Step(world);

// Iterate again, to step 3
World world3 = history.Step(world2);
```