# Blast Corps Level Editor
Tools for hacking the Blast Corps ROM for N64.

## Current Features
* Change vehicle types and positions
* Change carrier position, heading, and speed
* Change TNT position and timer
* Move RDUs, ammo boxes, communication points, buildings
* View train/barge platforms, collision modifiers, and bounds
* Edit gravity and other level header data

### Changelog ###
0.0.3: More details on comm points and buildings
* add control for comm point H6 value to control animation
* add more descriptive fields for buildings
* improved item selection UI
* export terrain and collision data to Wavefront OBJ model

0.0.2: UI cleanup, improved parsing, bug fixes

* use internal GZipStream instead of external gzip dependency
* add train/barge platforms and stopping zones
* accept all N64 ROM types: n64, v64, z64
* better ROM validation
* move level table to 0x7FC000 to potentially support (E) in future
* add "Save & Run" menu option
* add controls and better listing of header data

0.0.1: Initial release

* supports vehicles, carrier, TNT, RDUs, ammo, comm. point, buildings
* renders collision mods and bounds