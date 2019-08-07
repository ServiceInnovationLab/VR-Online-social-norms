using UnityEngine;
using VRTK;

public class DisableTeleportOnValidPointer : MonoBehaviour
{
    DisableTeleportOnTouch touchTeleport;
    VRTK_Pointer pointer;

    bool valid = false;

    private void Awake()
    {
        touchTeleport = FindObjectOfType<DisableTeleportOnTouch>();
        pointer = GetComponent<VRTK_Pointer>();

        pointer.PointerStateValid += PointerStateValid;
        pointer.PointerStateInvalid += PointerStateInvalid;
    }

    private void PointerStateInvalid(object sender, DestinationMarkerEventArgs e)
    {
        if (valid)
        {
            touchTeleport.RemoveDisabler();
            valid = false;
        }
    }

    private void PointerStateValid(object sender, DestinationMarkerEventArgs e)
    {
        if (!valid)
        {
            touchTeleport.AddDisabler();
            valid = true;
        }        
    }
}
