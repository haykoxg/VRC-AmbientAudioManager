# Ambient Audio System for VRChat

This package delivers a robust ambient audio solution for VRChat worlds. It automatically switches between outdoor and indoor soundtracks based on defined trigger volumes, with smooth cross-fades and extensive customization.

---

## Overview

- **Automatic Detection**: Identifies when the local player enters or exits interior zones.
- **Cross-Fade Support**: Seamlessly transitions between ambient clip sets.
- **Playback Modes**: Choose between random selection or sequential order.
- **Configurable Loop Interval**: Add a delay between ambient loops when needed.
- **Intro Clip & Start Delay**: Optionally play a one-time introduction before loops begin.
- **Volume Controls**: Adjust master, inside, and outside volumes independently.
- **Spatial Blend**: Fine-tune 2D (UI) vs. 3D (world) audio mixing.
- **Custom Fade Curves**: Define non-linear fade shapes with an AnimationCurve.
- **Debug & Visualization**: Enable logging and show zone boundaries in the editor.

---

## Installation & Setup

1. **Ambient Audio Manager**
   - Create an empty GameObject under your `headAttachmentHandler`.
   - Add two child GameObjects, each with an `AudioSource` component (call them **SourceA** and **SourceB**).
   - Attach `AmbientAudioManager.cs` to the parent object.
   - In the Inspector, configure:
     - **Source A** / **Source B** references.
     - **Outside Clips[]**: array of outdoor ambient tracks.
     - **Inside Clips[]**: default indoor tracks.
     - **Playback Settings**: mode (`Random`/`Sequential`), loop interval.
     - **Volume Settings**: `masterVolume`, `outsideVolume`, `insideVolume`.
     - **Spatial Blend** slider (0–1).
     - **Fade Duration** and **Fade Curve**.
     - **Start Delay** and **Intro Clip** (optional).
     - **Enable Debug** and **Show Zone Gizmos** flags.

2. **Audio Zone Volumes**
   - For each room or interior area, place a collider object with **Is Trigger** enabled.
   - Attach `AudioZone.cs` to each collider.
   - In the zone’s Inspector, assign the **AmbientAudioManager** reference.

3. **Testing in Editor**
   - Enter Play Mode in Unity.
   - Move the player into and out of each zone to confirm cross-fades and clip behavior.

4. **Deployment**
   - Build and publish your world to VRChat.
   - Verify that ambient changes occur in the live environment.

---

## Customization Guide

| Setting              | Description                                                      |
|----------------------|------------------------------------------------------------------|
| **Playback Mode**    | `Random` or `Sequential` order of clips.                         |
| **Loop Interval**    | Delay (seconds) between end of one clip and start of next.      |
| **Master Volume**    | Global volume multiplier (0–1).                                  |
| **Outside Volume**   | Volume level for outdoor ambience (0–1).                        |
| **Inside Volume**    | Volume level for indoor ambience (0–1).                         |
| **Spatial Blend**    | 0 = stereo UI; 1 = fully 3D world audio.                        |
| **Fade Duration**    | Duration (seconds) of cross-fade.                               |
| **Fade Curve**       | AnimationCurve for non-linear fade shapes.                      |
| **Start Delay**      | Seconds to wait before initial playback.                        |
| **Intro Clip**       | One-time introductory clip before ambient loops.                |
| **Enable Debug**     | Toggle console logging of events and fade progress.             |
| **Show Zone Gizmos** | Display green wireframe boxes for each zone in the editor.       |

---

## Troubleshooting

- **No Sound**: Ensure `Play On Awake` is disabled and clips are assigned correctly.
- **Trigger Not Detected**: Verify the collider is set to **Is Trigger** and has `AudioZone` attached.
- **Spawn Inside Zone**: Zones perform a spawn check; ensure your player’s headAttachmentHandler is correctly referenced.
- **Stuttering Fade**: Increase the `FADE_STEPS` constant in `AmbientAudioManager.cs` for finer resolution.

---

## Support & Contributions

If you encounter issues or have feature suggestions, please open an issue or submit a pull request on the repository. Provide clear reproduction steps and configuration details.

---

**End of README**
