Camera Buffer Drag Controller
==========

This Unity script will set the camera to follow a player with a buffer towards the direction on a controller to show more of the area in level.

This is useful for 2D or platformer games where you will want the camera to show what is ahead of the player towards the direction of the joystick.
Settings can be adjusted to add to the buffer fast or slow and then decay back to the player at a linear rate or a variable setting.

![gif](https://i.imgur.com/IEI8NQp.gif)
![gif](https://i.imgur.com/pKodWmi.gif)



Installation
------------

Download the github project and copy the CameraBufferDrag.cs file to your unity project.
Then drag the script to an empty gameobject or controller & attach a camera & character to the script and set the joystick string controls which match your project.
