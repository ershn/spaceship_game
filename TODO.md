# TODO

## Game systems
- add fertilizer items
- add multi world
- add spaceship movement to arbitrary worlds
- add a storage furniture
- add a sweep order
- add a wall furniture

### Food consumption
- low priority: equally distribute food between clones when there isn't enough of it

### Path finding
- upgrade to A* Pathfinding Pro when the yen gets stronger
- allow to calculate a path that ends within a set distance of a target

### Item deliveries
- handle scenarios where an item is unreachable
- optimization: allow to pick several items during the same task
- optimization: pick the closest items and executors to fulfill a task

## Task scheduling
- add task priorities
- retry a task on a different executor when its executor is removed

## Graphics

### Clone
- add an eating animation
- add an idle animation

### Items
- fix the rendering order of items on the same tile

## Building
- allow to build a rectangular area of floors

## UX/UI
- add an info window to show being/item/building properties

## Program architecture
- regroup StructureDef subclass specific methods in one place
  - *Constructor.IsConstructibleAt
- update to the new Input System
- pass a TaskResult object to onEnd callbacks
  - Success property