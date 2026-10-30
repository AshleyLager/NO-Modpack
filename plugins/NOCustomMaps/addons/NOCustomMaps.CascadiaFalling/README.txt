Cascadia Falling - custom map and mission for Nuclear Option (work in progress)

Map: a 400 km box of Puget Sound, Vancouver to Yakima, built from real elevation, land cover
and OpenStreetMap data. Mission: PALA attacks from the Pacific and the Strait of Georgia;
BDF defends from Tacoma. PvP, both sides can be joined.

Needs: BepInEx 5 and the NOCustomMaps plugin v1.4.0 or newer (com.javoski.custommaps).
Everyone in a session needs the same map build: map key cm.cascadia.2546c698.

Easiest install (Nuclear Option Mod Manager + Steam Workshop):
1. In the Nuclear Option Mod Manager search for "Cascadia Falling" and install it. It pulls in
   NOCustomMaps and installs the map (about 1.7 GB). Install "NOGridLetters" too (optional, see below).
2. Subscribe to the "Cascadia Falling" mission on the Steam Workshop. It appears in the game's
   mission list under Workshop and updates itself.

By hand:
1. From cascadia-0.1.0.zip put cascadia-0.1.0.nomapz into BepInEx\plugins\NOCustomMaps\maps\ in your
   Nuclear Option game folder (e.g. ...\steamapps\common\Nuclear Option\). The CustomMaps folder
   under %USERPROFILE%\AppData\LocalLow\Shockfront\NuclearOption\ works too.
2. From the mission-only zip copy the "Missions\Cascadia Falling" folder to
   %USERPROFILE%\AppData\LocalLow\Shockfront\NuclearOption\Missions\ (replace the old one), and
   NOGridLetters.dll into BepInEx\plugins\.
3. Start the game and pick Cascadia Falling.

NOGridLetters.dll is a 6 KB plugin of ours (https://github.com/netmaster296/NOGridLetters): on the tactical map and in the coordinate
readout the rows past Z read A*, B*, C* ... instead of the unreadable characters the game uses after Z,
and the zoomed-in column labels read 12-0, 12-1 ... instead of 120, 121. The game runs without it;
the southern half of the map is just hard to call out. A callout is row + minor row + column,
e.g. "D*f12-3".

The .nomapz is the .nomap compressed (NOCustomMaps 1.4 reads it directly); its map key is the same.
Delete any earlier Cascadia build (cm.cascadia.982e4baa, ad4b5110, bbcd790f, 12f4e54d): the game
cannot tell them apart by name. The first minute is slow while the map streams in; it settles after that.
Map, mission and news: https://github.com/netmaster296/CascadiaFalling

Mission 2026-10-23 (same map, mission-only zip is enough): the PALA fleets now sail all the way in.
The first western carrier group starts in the Strait of Juan de Fuca 15 km short of Port Townsend
and steams east; the first northern group comes south out of Rosario Strait; they meet off Port
Townsend and push through Admiralty Inlet and down the Sound in line astern, the western force to
Commencement Bay off Tacoma, the northern to Elliott Bay off Seattle (lead groups: Admiralty Inlet
about T+35, Kingston T+100, Seattle T+120-145, Tacoma T+180). Every later group enters further back
along the same lanes and follows the same track. The I-5, Stevens Pass and Hope columns march again
(they used to be re-targeted by the game to the Port of Tacoma, whose road piece is an island, and
stop dead), and the AI aircraft at every airfield taxi out and take off again (the taxi lanes were
nine unjoined pieces per field; the mission now carries corrected lanes for the plugin).

Map and mission 2026-10-22 (night-2 build), after the first look at the paved build ("laggy once loaded
in, PALA craft dying"): the terrain now has three levels of detail per tile (full mesh to about
20 km, coarser beyond, which cuts the triangles drawn from tens of millions to a few million), the
small ponds and thin river reaches are gone (the plugin walks the whole lake list for every wing of
every aircraft every physics step, so 1,859 sheets became 747), the BDF pickets are single ships
(10 instead of 20), and the parked aircraft are gone: an AI airframe that starts on the ground
taxis out at T+0 and ejects when its takeoff stalls, which was the line of PALA (and BDF) losses in
the first minutes. PALA's T+0 probes and T+5 raids now go for the pickets at the straits instead of
flying 200 km into Whidbey, Everett and Bremerton to die. 18 BDF ships at T+0, 144 aircraft and 180
ships over the whole mission. Mission update the same night: PALA's first two carrier groups start
on station at T+0 (off Port Townsend and off Birch Bay) instead of sailing 190 km from the Pacific,
every later group enters 20 km short of its own station, and the formations are 1.8 times wider
(nothing closer than 2 km). Same map, so the mission-only zip is enough if you have this nomap.

Map and mission 2026-10-23 (late build): every airfield, FOB, pad, silo field and naval base is one
flat concrete block with the runway cut into it, the ground under it levelled half a metre lower
and a skirt round the edge, so nothing spawns on grass, no taxi lane leaves the tarmac and no two
surfaces overlap any more. Parked aircraft stand at their proper rest height. The grid starts with
row A and column 0 at the map's north-west corner, and the chart no longer prints cell names of its
own: the game's edge labels are the designations.

Night build of 2026-10-23, for frame rate: the city scenery is cut from
200,000 buildings to 40,000 and the forests thinned a little, since the plugin draws every
building near you one by one. The bases are consolidated from 46 to 28 sites (airfields,
FOBs and pads merged into their neighbours) and rebuilt leaner and tidier. PALA's navy is
bigger and steadier: the first carrier groups arrive at T+10 and a new group enters on
each axis every ten minutes until T+120 (carrier groups at T+10, T+30 and T+70), so BDF
has to sink six Annexes. The opening is still light: at T+0 only the BDF's bases and ships
exist and PALA flies from Comox. Alkyon strikes come in at 30,000 ft instead of on the
deck. The BDF navy sits as picket pairs across the Sound.

Map 2026-10-22 evening: real seabed in the whole Sound (ships no longer ground), ocean
colour and depth textures (no more grey water), real-time sun (no stepping), carrier
groups halved and kept to deep water on water-only sea lanes, PALA aircraft can land
(hooked Ifrits, Comox is PALA's), Mission Editor loads the mission again.

Known WIP: a few outlying airfields (Paine, Whidbey, Bellingham, Yakima, Vancouver Island)
are not joined to the main road network, so AI ground units from them stop at the first gap.
Data: OpenStreetMap contributors (ODbL), ESA WorldCover 2021 (CC BY 4.0), USGS 3DEP /
NRCan CDEM / NOAA via Mapzen Terrarium.
