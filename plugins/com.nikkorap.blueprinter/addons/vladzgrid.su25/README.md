# Su-25 Frogfoot 0.1.4 (experimental)

Requires Nuclear Option 0.34.2, BepInEx 5 and Blueprinter 2.0.1.
Tier 2, $25,000,000, ten weapon stations, dedicated icon and camouflage.

## Installation
NOMM: install this add-on and accept its Blueprinter dependency.
Manual: close the game and extract all files directly into
BepInEx/plugins/com.nikkorap.blueprinter/addons/vladzgrid.su25/
The .nobp and Su25FlightFix.dll must be together, without another BepInEx folder.
If upgrading an earlier private build, remove the old Su25FlightFix.dll from
BepInEx/plugins/vladzgrid.su25.flightfix/ to avoid duplicate plugin versions.
Restart the game and spawn a new aircraft after updating.

## Validation and limitations
The owner reports normal flight; earlier fresh-spawn logs confirmed three
owned native landing gears and 58 source animation tracks. Runtime helper 0.1.4
corrects mass placement, local Su-25 automatic wheel braking, and gear animation.
Latest bundle compiled in Unity 2022.3.62f2 on 2026-10-18. The physical HUD
combiner and olive flare housings were inspected in editor renders, but this
last visual revision has not been flight-tested in game. Native HUD symbology
and functional game displays are retained; source analog dials are decorative.
This is an experimental Blueprinter adaptation, not a fully authentic simulator.
Multiplayer compatibility and a complete weapons/landing campaign are unverified.

Controller users: disable native mouse virtual-joystick flight if camera motion
steers the aircraft. If NOTT-plus supplies a resting brake axis, disable its
Game Bindings Patch in that mod's configuration. This archive does not change
your input bindings or install testing tools.

GPL-3.0 model adaptation and runtime source: https://github.com/Braianvladzgrid/su25-frogfoot-nuclear-option
See CREDITS.md and LICENSE.txt. No game DLLs or exported game bundles are included.
