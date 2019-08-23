using UnityEngine;
using VRTK;

public class SnapToFloor : MonoBehaviour
{
    [SerializeField] VRTK_BasicTeleport teleport;
    [SerializeField] Transform floor;
    [SerializeField] Vector3 offset = Vector3.up * 0.5f;
    [SerializeField] bool disableOnUse = false;
    [SerializeField] bool keepTeleportToPointerValue = true;
    [SerializeField] bool snapManagerHere = false;

    private void Awake()
    {
        if (!floor)
        {
            floor = transform;
        }

        if (snapManagerHere)
        {
            var manager = FindObjectOfType<VRTK_SDKManager>();
            manager.transform.position = transform.position.XZ();
        }
    }

    private void OnEnable()
    {
        
    }

    public void DoSnapToFloor()
    {
        if (!enabled)
            return;

        var player = VRTK_DeviceFinder.HeadsetTransform();

        var playAreaTeleport = teleport as PlayAreaLimitedTeleport;
        bool originalValue = false;

        if (playAreaTeleport)
        {
            originalValue = playAreaTeleport.GetOnlyTeleportToPointers();
            playAreaTeleport.SetOnlyToPointers(false);
        }

        teleport.Teleport(floor, player.position + offset);

        enabled = !disableOnUse;

        if (keepTeleportToPointerValue && playAreaTeleport)
        {
            playAreaTeleport.SetOnlyToPointers(originalValue);
        }
    }
}
