# Movement 🚧

Movement describes the transition of anything from one location to another. Movement should be considered a seporate state from normal.

The easiest way to view movement as a built in exclusive quest. 

Movement only applies with inter-square movement. Movement between things within a square (intra-square movement) is immediate and does not consume movement. The mechanisms for intra-square movement is the same as inter-square movement, but applies immediately at the end of the turn.

Everything that can move has 3 computed qualities about movement:

* Land speed: This is the number of land tiles that the thing can move in a Step, assuming that the tile's movement cost is 1
* Water speed: This is the number of water tiles that the thing can move in a Step, assuming that the tile's movement cost is 1
* Can Fly: If the thing can fly, only land speed is used and the tile movement cost is ignored, every movement will be a striaght line from start to end.
   * Some logic should be added to avoid dangerous areas

Every square has one computed quality which effects movement:

* Movement Modifier: This is the cost to move through the tile. The default movement should be 1, but numbers higher and lower are permitted.

At the start of every turn:

* Air and land speed are calcuated
* The ratio between these are calulated
* Pathfinding is run, using the ratio of land to water movement to modify the tile movement costs.
* The thing is moved a number of times until all movement is exhausted. Each square moved is subtracted from both Land and Water movement using the ratio between land and water movement.
   * For example, if all squares have a 1 movement modifier, and a thing has Land speed of 15 and Water speed of 5
      * If the thing moves into a Land tile, it will have 14 land movement left and 4.66 water movement left (as the land/water ratio is 1/3)
      * Than if the thing moves into a water tile, it was will 11 land movement and 3.66 water movement left
* During movement most Events should not apply to this Thing, but some Events may.
* Any movement left is stored on the Thing as "Movement Progress", stored as Land movement. This will be used in the next Step.
   * This can be used to move into squares which have a movement cost which far exceeds the thing's movement speed.
   * This value is cleared when movement completes to ensure it does not build up when somebody with high movement moves between two nearby sites, than instantly moves to a far away site.