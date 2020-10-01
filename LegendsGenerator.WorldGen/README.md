# World Generation

The world generation process works by generating a series of grids of values for various aspects of a world. Once generated, it plays with the values to make things make more sense (lower temp in high elevations, water bodies which touch the sides are salt water, etc).

## Process

* Generate a bunch of maps for elevation, temperture, drainage, rainfall, etc.
* Modify these maps based on the valeus in other maps (Lower temp based elevations)
* 🚧 Pick a couple mountains to become volcanos, crank up the elevation a bit
* 🚧 In high rainfall and elevation areas, generate the start to streams
   * If a stream goes through an area with high rainfall:drainage ratio, it should get bigger and potentially becomes a river.
   * If a stream goes through an area with low rainfall:drainage ratio, the stream should decrease in size.
   * Randomly split the stream at points, with a random ratio of water to each fork.
   * If a stream encounters an area with minimal change in elevation to surrounding nodes, generate a lake.
* 🚧 Pepper the world with some interesting points, such as:
   * Oasises in deserts
   * Caves in places with high differences between elevation
* Only once everything is done, convert the whole lot to LegendsGenerator format.