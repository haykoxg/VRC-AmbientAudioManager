using UdonSharp;
using UnityEngine;
using VRC.Udon;
using VRC.SDKBase;

public enum PlayMode { Random, Sequential }

public class AmbientAudioManager : UdonSharpBehaviour
{
    [Header("Audio Sources (cross-fade between these)")]
    [SerializeField] private AudioSource sourceA;
    [SerializeField] private AudioSource sourceB;

    [Header("Clips")]
    [Tooltip("Ambient clips to play when the player is outside all zones.")]
    [SerializeField] private AudioClip[] outsideClips;
    [Tooltip("Ambient clips to play when the player is inside any zone.")]
    [SerializeField] private AudioClip[] insideClips;

    [Header("Playback Settings")]
    [Tooltip("Random: choose clips randomly. Sequential: play in order.")]
    [SerializeField] private PlayMode playOrder = PlayMode.Random;
    [Tooltip("Seconds to wait between end of one clip and start of next (if >0, disables auto-loop).")]
    [SerializeField] private float clipLoopInterval = 0f;

    [Header("Volume Settings")]
    [Range(0f,1f)][SerializeField] private float masterVolume  = 1f;
    [Range(0f,1f)][SerializeField] private float outsideVolume = 1f;
    [Range(0f,1f)][SerializeField] private float insideVolume  = 1f;

    [Header("Spatial")]
    [Range(0f,1f), Tooltip("0 = 2D, 1 = fully 3D")]  
    [SerializeField] private float spatialBlend = 0f;

    [Header("Fade Settings")]
    [Tooltip("Time (seconds) to cross-fade between outside ↔ inside ambience.")]
    [SerializeField] private float fadeDuration = 1f;
    [Tooltip("Animation curve for fade (0=t0,1=end)")]
    [SerializeField] private AnimationCurve fadeCurve = AnimationCurve.Linear(0,0,1,1);

    [Header("Startup / Intro")]
    [Tooltip("Seconds to wait after scene load before playing intro or outside ambience.")]
    [SerializeField] private float startDelay = 0f;
    [Tooltip("One-off intro clip before ambient loops.")]
    [SerializeField] private AudioClip introClip;

    [Header("Debug & Visualization")]
    [SerializeField] private bool enableDebug     = false;
    [SerializeField] private bool showZoneGizmos  = false;

    // internal state
    private int   insideCount     = 0;
    private bool  useSourceA      = true;
    private float targetVolume    = 0f;
    private float previousVolume  = 0f;      // holds volume of outgoing source
    private float fadeElapsed     = 0f;
    private float fadeStepTime    = 0f;
    private const int FADE_STEPS  = 20;

    private AudioClip[] lastClips;
    private bool        lastStateInside;

    // sequence indices
    private int outsideIndex = 0;
    private int insideIndex  = 0;

    void Start()
    {
        if (startDelay > 0f)
            SendCustomEventDelayedSeconds(nameof(StartIntro), startDelay);
        else
            StartIntro();
    }

    public void StartIntro()
    {
        if (enableDebug) Debug.Log("[AmbientAudioManager] StartIntro");

        if (introClip != null)
        {
            previousVolume = outsideVolume * masterVolume;
            sourceA.clip         = introClip;
            sourceA.loop         = false;
            sourceA.volume       = previousVolume;
            sourceA.spatialBlend = spatialBlend;
            sourceA.Play();

            float delay = introClip.length + clipLoopInterval;
            SendCustomEventDelayedSeconds(nameof(NotifyOutside), delay);
        }
        else
        {
            NotifyOutside();
        }
    }

    public void NotifyEnter()
    {
        insideCount++;
        if (enableDebug) Debug.Log($"[AmbientAudioManager] NotifyEnter: count={insideCount}");
        if (insideCount == 1)
            NotifyInside();
    }

    public void NotifyExit()
    {
        insideCount = Mathf.Max(insideCount - 1, 0);
        if (enableDebug) Debug.Log($"[AmbientAudioManager] NotifyExit: count={insideCount}");
        if (insideCount == 0)
            NotifyOutside();
    }

    private void NotifyInside()
    {
        if (enableDebug) Debug.Log("[AmbientAudioManager] NotifyInside");
        lastStateInside = true;
        CrossFadeTo(insideClips, insideVolume);
    }

    private void NotifyOutside()
    {
        if (enableDebug) Debug.Log("[AmbientAudioManager] NotifyOutside");
        lastStateInside = false;
        CrossFadeTo(outsideClips, outsideVolume);
    }

    private void CrossFadeTo(AudioClip[] clips, float newVol)
    {
        // store old outgoing volume
        previousVolume = targetVolume;

        // pick sources
        AudioSource incoming = useSourceA ? sourceA : sourceB;
        AudioSource outgoing = useSourceA ? sourceB : sourceA;
        useSourceA = !useSourceA;

        // choose clip
        AudioClip chosen = null;
        if (clips != null && clips.Length > 0)
        {
            if (playOrder == PlayMode.Random)
                chosen = clips[Random.Range(0, clips.Length)];
            else
            {
                int idx = lastStateInside ? insideIndex : outsideIndex;
                chosen = clips[idx % clips.Length];
                if (lastStateInside) insideIndex++; else outsideIndex++;
            }
        }

        if (chosen != null)
        {
            incoming.clip         = chosen;
            incoming.loop         = (clipLoopInterval <= 0f);
            incoming.volume       = 0f;
            incoming.spatialBlend = spatialBlend;
            incoming.Play();
        }
        else outgoing.Stop();

        // set new target volume
        targetVolume   = newVol * masterVolume;
        fadeElapsed    = 0f;
        fadeStepTime   = fadeDuration / FADE_STEPS;
        SendCustomEventDelayedSeconds(nameof(FadeStep), fadeStepTime);

        // manual loop scheduling
        if (clipLoopInterval > 0f && chosen != null)
        {
            float delay = chosen.length + clipLoopInterval;
            SendCustomEventDelayedSeconds(nameof(LoopAmbient), delay);
        }
    }

    public void FadeStep()
    {
        fadeElapsed += fadeStepTime;
        float t = fadeCurve.Evaluate(fadeElapsed / fadeDuration);

        AudioSource fadingIn  = useSourceA ? sourceB : sourceA;
        AudioSource fadingOut = useSourceA ? sourceA : sourceB;

        // fade in to new target, fade out from previousVolume
        fadingIn.volume  = t * targetVolume;
        fadingOut.volume = (1f - t) * previousVolume;

        if (enableDebug) Debug.Log($"[AmbientAudioManager] Fade t={t}");

        if (fadeElapsed < fadeDuration)
            SendCustomEventDelayedSeconds(nameof(FadeStep), fadeStepTime);
        else
            fadingOut.Stop();
    }

    public void LoopAmbient()
    {
        if (enableDebug) Debug.Log("[AmbientAudioManager] LoopAmbient");
        if ((lastStateInside && insideCount > 0) || (!lastStateInside && insideCount == 0))
            CrossFadeTo(lastClips, lastStateInside ? insideVolume : outsideVolume);
    }

    public bool ShowGizmos() => showZoneGizmos;
}
