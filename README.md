<h1>ToyCarMania</h1>

This this the game ToyCarMania created by Erik Ståhl for the course TDDD57

**The Game**

ToyCarMania is a fun little game where a toy car arena is placed inside your own home.
The player starts by mapping out a playing area with their camera, calculated by ARCore. When the area is big enough for you, you choose it and start playing.
Walls and an invicible ground is place inside your own home, and a small toy truck is placed in the middle of the arena.
Your phone camera is then used to control the truck. A small crosshair is placed flat at the location that the center of your camera is pointing. 
The toy truck then simply follows that crosshair. Your job is now to collect the crates as they spawn to gain points. However, you only have 30 seconds to collect points.
However, every crate also gives you 5 more seconds on the clock. Your job is the collect as many points as you can before the time runs out.
But hold on, there's a catch.
Every now and then a grenade spawns. The grenades can't destroy you or the points, but they can very forcefully move you and the crates. Did a crate blow out of the arena?
tough luck!
ToyCarMania is a small, hectic and just plain fun game.

**The Code**

All main code is located within Assets/Game. Here you will find all scripts and gameobjects. The other folders are the ARCore package and imported prefabs.
The game code is based around 4 main objects. 
The first is the plane visualiser. his object uses the standard google arcore visualiser together with my script which highlights the borders aswell
as puts a marker in the center of the plane that the player is pointing at. The object is called Plane Highlighter and the scripts DetectedPlaneVisualiser, DetectedPlaneGenerator and BoundaryDataVisualiser.
The next object is the UI. This consists of the Canvas and ButtonCanvas which show score, time left, the end screen and the restart button. These object doesn't have any scripts of their own, they are only referenced by other objects.
The third object is the game handler. A startup-call is made from BoundaryDataVisualiser when the ChoosePlane button is clicked. This starts the game.
The main script in game handler is also called GameHandler and this script generates the ground, walls and instantiates the player truck. This game object also spawns the
point cubes and grenades. Another script part of game handler is the MovePointer script. This moves the marker around so that it is always the the location on the plane where the center of the camera is pointing.
The last main object is the player. The object uses the name of the prefab, IndustrialSmallTruck. This object holds the scripts Move Truck, Player Collision and player score.
The Move Truck is responsible for moving the truck towards the location of the marker that the player controls. I.E, the player never controls the truck, it just controls the position of which the truck is trying to go.
the second script part of the truck is the player collision. This detects whenever the truck hits a point cube, and then gives the player a point and more time. The last script is the player score, which receives input that a point has been scored.
It also calls gameOver whenever the time runs out.

Apart from this there are small scripts responsible for exploding the grenade, removing objects after a certain time etc. Som scripts are old and not used, placed in the oldScripts folder.
