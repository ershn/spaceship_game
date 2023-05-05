# TODO

## Game systems
- add a storage building
- add a sweep order
- add a food producing plant

### Food consumption
- equally distribute food between clones when there isn't enough of it

### Item deliveries
- handle scenarios where there aren't enough items to fulfill a request
- handle scenarios where an item is unreachable
- optimization: allow to pick several items during the same task
- optimization: pick the closest items and executors to fulfill a task

## Graphics
- add an eating animation
- add an idle animation

## Task scheduling
- add task priorities
- priorities tasks with a fixed executor

## UI
- allow to build a rectangular area of floors
- add an info window to show being/item/building properties

## Program architecture
- update to the new Input System
- pass a TaskResult object to onEnd callbacks
  - Success property