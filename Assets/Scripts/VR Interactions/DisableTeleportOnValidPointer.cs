using UnityEngine;
using VRTK;

public class DisableTeleportOnValidPointer : MonoBehaviour
{
    DisableTeleportOnTouch touchTeleport;
    VRTK_Pointer pointer;
    VRTK_UIPointer uiPointer;
    VRTK_ControllerEvents events;

    bool valid = false;

    private void Awake()
    {
        touchTeleport = FindObjectOfType<DisableTeleportOnTouch>();
        pointer = GetComponent<VRTK_Pointer>();
        uiPointer = GetComponent<VRTK_UIPointer>();

        pointer.PointerStateValid += PointerStateValid;
        pointer.PointerStateInvalid += PointerStateInvalid;
    }

    private void PointerStateInvalid(object sender, DestinationMarkerEventArgs e)
    {
        if (valid)
        {
            touchTeleport.RemoveDisabler(events, gameObject);
            valid = false;
        }
    }

    private void PointerStateValid(object sender, DestinationMarkerEventArgs e)
    {
        if (!valid)
        {
            events = uiPointer.controllerEvents;
            touchTeleport.AddDisabler(events, gameObject);
            valid = true;
        }        
    }
}
