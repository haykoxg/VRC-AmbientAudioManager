using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[RequireComponent(typeof(Collider))]
public class AudioZone : UdonSharpBehaviour
{
    [Tooltip("Drag your AmbientAudioManager here.")]
    [SerializeField] private AmbientAudioManager manager;

    private Collider _col;

    void Start()
    {
        _col = GetComponent<Collider>();
        SendCustomEventDelayedSeconds(nameof(CheckSpawnInside), 0.2f);
    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (player.isLocal) manager.NotifyEnter();
    }

    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        if (player.isLocal) manager.NotifyExit();
    }

    public void CheckSpawnInside()
    {
        var player = Networking.LocalPlayer;
        if (player == null) return;
        if (_col.ClosestPoint(player.GetPosition()) == player.GetPosition())
            manager.NotifyEnter();
    }

    void OnDrawGizmosSelected()
    {
        if (manager != null && manager.ShowGizmos())
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(_col.bounds.center, _col.bounds.size);
        }
    }
}
