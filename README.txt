Ambient Audio System for VRChat
----------------------------------------------------------------------------
This package provides an easy-to-integrate ambient audio solution for VRChat worlds.
It automatically switches between "outside" and "inside" ambient tracks based on trigger volumes, with smooth cross-fades, configurable playback modes, and a range of customization options.
----------------------------------------------------------------------------
Key Components

- AmbientAudioManager
	- Controls audio playback, cross-fades, and global settings.
	- Exposes fields for clip arrays, volumes, fade curves, and more.

- AudioZone
	- Attach to any trigger collider to mark an interior zone.
	- Notifies the manager when the local player enters or exits.
----------------------------------------------------------------------------
Setup Instructions

1) Place the Manager anywhere in the Hierarchy.
2) Create an empty GameObject under your headAttachmentHandler.
3) Add two AudioSource components (For SourceA and SourceB).
4) Attach AmbientAudioManager.cs to the parent.
5) In the Inspector, assign:
	- Source A and Source B
	- Clips
6) Mark Your Zones
	- For each interior, create a trigger collider in the scene.
7) Attach AudioZone.cs to the collider object.
8) Drag the AmbientAudioManager into the zone’s Manager field.
9) Test in Editor
----------------------------------------------------------------------------
Manager Options

Outside Clips: ambient clips for outdoor areas.
Inside Clips: fallback clips for interiors.
Playback Settings Random/Sequential.
Loop Interval: How long in between clips.
Volume Settings (Master, Outside, Inside)
Spatial Blend: 0 = 2D. 1 = 3D* (See bottom)
Fade Duration and Fade Curve: Use an AnimationCurve for non-linear cross-fades.
Start Delay and Intro Clip (Play a one-off introduction before looping ambient tracks.)
Debug: Enable logging and zone outlines to assist level design.
----------------------------------------------------------------------------
Troubleshooting

No audio on start: Check that AudioSources have "Play On Awake" disabled and clips are assigned.
Zone not detected: Ensure the collider is set to "Is Trigger" and the zone object has AudioZone attached.
Overlap on spawn: Zones handle spawn-inside detection automatically; verify the manager is registered in each zone.
Fade stutter: Increase FADE_STEPS constant in manager for finer fade resolution.

Support

For questions, bug reports, or feature requests, contact "haykoxg" on discord.

* Spatial Blend additional info: (It is reccomended to keep this 2D, for 3D, I reccomend that you install my HeadAttachmentHandler on my github and attach this to the position only array)