# TODO

## Game system
- add clone death
- add a storage building
- add a sweep order
- add a food producing plant

## Graphics
- add an eating animation
- add an idle animation

## Task scheduling
- add task priorities
- priorities tasks with a fixed executor

## Item search
- handle scenarios where there aren't enough items to fulfill a request
- handle scenarios where an item is unreachable
- optimization: allow to pick several items during the same task
- optimization: pick the closest items and executors to fulfill a task

## UI
- allow to build a rectangular area of floors
- add an info window to show being/item/building properties

## Program architecture
- update to the new Input System
- pass a TaskResult object to onEnd callbacks
  - Success property