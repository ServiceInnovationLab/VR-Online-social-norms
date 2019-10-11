using UnityEngine;
using VRTK;

public class DisableUseOnValidPointer : MonoBehaviour
{
    VRTK_Pointer pointer;
    VRTK_UIPointer uiPointer;
    VRTK_ControllerEvents events;
    VRTK_InteractUse interactUse;

    bool valid = false;

    private void Awake()
    {
        pointer = GetComponent<VRTK_Pointer>();
        uiPointer = GetComponent<VRTK_UIPointer>();

        pointer.PointerStateValid += PointerStateValid;
        pointer.PointerStateInvalid += PointerStateInvalid;
    }

    private void PointerStateInvalid(object sender, DestinationMarkerEventArgs e)
    {
        if (valid)
        {
            if (interactUse)
            {
                interactUse.enabled = true;
            }
            valid = false;
        }
    }

    private void PointerStateValid(object sender, DestinationMarkerEventArgs e)
    {
        if (!valid)
        {
            events = uiPointer.controllerEvents;
            interactUse = events.GetComponent<VRTK_InteractUse>();
            if (interactUse)
            {
                interactUse.enabled = false;
            }
            valid = true;
        }
    }
}
