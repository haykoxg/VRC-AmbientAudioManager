# Ambient Audio System for VRChat

A flexible ambient audio tool for VRChat worlds, featuring:

- **Indoor/Outdoor Detection**: Switch ambient tracks when players enter or exit defined zones.
- **Trigger & Polling Modes**: Use collider-based triggers or optional position polling for unreliable physics.
- **Smooth Cross‐Fades**: Configurable fade duration and non-linear fade curves.
- **Playback Control**: Random or sequential clip selection, loop intervals, and one‐time intro clips.
- **Volume & Spatial Settings**: Master, inside, and outside volume sliders; 2D/3D spatial blend.
- **Debug & Visualization**: Console logging and editor gizmos to assist setup and troubleshooting.

---

## Quick Start

1. **Add the Manager to Your Scene**
   - In your **world** project, create an empty GameObject (e.g. `AmbientAudioManager`) under any non-avatar root.
   - Add two child GameObjects named `SourceA` and `SourceB`, each with an **AudioSource** component.
   - Attach `AmbientAudioManager.cs` to the parent object.
   - In the Inspector, assign **Source A** and **Source B** AudioSources.

2. **Configure Clip Lists**
   - **Outside Clips[]**: clips for outdoor ambience.
   - **Inside Clips[]**: default clips for interiors.

3. **Set Playback Options**
   - Choose **Playback Mode**: `Random` or `Sequential`.
   - Optionally set a **Clip Loop Interval** to insert gaps between loops.

4. **Adjust Volume & Spatial**
   - **Master Volume**: global volume multiplier.
   - **Outside Volume** / **Inside Volume**: relative levels for each mode.
   - **Spatial Blend** (0 = 2D, 1 = 3D).

5. **Cross-Fade & Intro**
   - **Fade Duration**: seconds to cross-fade.
   - **Fade Curve**: define custom easing.
   - **Start Delay** / **Intro Clip**: one-time intro before looping.

6. **Zone Detection**
   - **Trigger Mode** (default):
     1. Create a GameObject with a collider (set **Is Trigger**).
     2. Attach `AudioZone.cs` and drag the manager into its **Manager** slot.
   - **Polling Mode** (fallback):
     1. Enable **Zone Polling** in the manager.
     2. Set a **Polling Interval** and drag each zone’s collider into **Zone Colliders[]**.

7. **Enable Debug & Gizmos**
   - Check **Enable Debug** to log enter/exit and fade events.
   - Check **Show Zone Gizmos** to draw zone wireframes in the Editor.

8. **Publish & Test**
   - Build & Publish your world with the VRChat SDK.
   - In VRChat, walk into and out of your zones to confirm ambient transitions.

---

## Detailed Settings

| Section              | Field                 | Description                                          |
|----------------------|-----------------------|------------------------------------------------------|
| **Audio Sources**    | Source A / Source B   | AudioSources used for cross-fades.                   |
| **Clips**            | Outside / Inside      | Arrays of `AudioClip` for each mode.                 |
| **Playback**         | Playback Mode         | `Random` or `Sequential` selection.                  |
|                      | Clip Loop Interval    | Delay between loops if > 0 (disables auto-loop).     |
| **Volume**           | Master Volume         | Global multiplier (0–1).                             |
|                      | Outside Volume        | Volume for outdoor ambience (0–1).                   |
|                      | Inside Volume         | Volume for indoor ambience (0–1).                    |
| **Spatial**          | Spatial Blend         | 0 = 2D (UI); 1 = 3D (world).                         |
| **Fade**             | Fade Duration         | Seconds to cross-fade.                               |
|                      | Fade Curve            | `AnimationCurve` for non-linear fade.                |
| **Intro**            | Start Delay           | Wait before initial playback.                        |
|                      | Intro Clip            | One-time clip before looping starts.                 |
| **Zone Polling**     | Enable Polling        | Overrides triggers with position-based checks.       |
|                      | Polling Interval      | Seconds between each position check.                 |
|                      | Zone Colliders[]      | Colliders to test for position polling.              |
| **Debug & Viz**      | Enable Debug          | Log enter/exit and fade steps.                       |
|                      | Show Zone Gizmos      | Draw zone bounds in the Scene view.                  |

---

## Troubleshooting

- **No inside/outside switch**: enable Debug and watch for `NotifyEnter` / `NotifyExit` in logs.
- **Trigger not firing**: ensure colliders are on the **Default** layer with **Is Trigger** checked.
- **Player never exits**: colliders must span from ground to head height so the player’s capsule leaves fully.
- **No audio at all**: verify clip arrays are non-empty and volumes are above zero.
- **Location-based audio still stuck**: use polling mode with properly assigned colliders.

---

## Contribution & License

Contributions are welcome. Please fork, commit, and open a pull request. Include clear descriptions and test steps.

This project is released under the MIT License. See [LICENSE](LICENSE) for details.
