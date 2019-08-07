using UnityEngine;
using VRTK;

public class DisableTeleportOnTouch : MonoBehaviour
{
    VRTK_Pointer[] pointers;
    VRTK_InteractTouch[] touches;

    int touchCount = 0;

    private void Awake()
    {
        touches = GetComponentsInChildren<VRTK_InteractTouch>();
        pointers = GetComponentsInChildren<VRTK_Pointer>();

        foreach (var touch in touches)
        {
            touch.ControllerStartTouchInteractableObject += ControllerStartTouchInteractableObject;
            touch.ControllerStartUntouchInteractableObject += ControllerStartUntouchInteractableObject;
        }
    }

    private void ControllerStartUntouchInteractableObject(object sender, ObjectInteractEventArgs e)
    {
        RemoveDisabler();
    }

    private void ControllerStartTouchInteractableObject(object sender, ObjectInteractEventArgs e)
    {
        AddDisabler();
    }

    public void AddDisabler()
    {
        touchCount++;

        foreach (var pointer in pointers)
        {
            pointer.enabled = false;
        }
    }

    public void RemoveDisabler()
    {
        if (--touchCount == 0)
        {
            foreach (var pointer in pointers)
            {
                pointer.enabled = true;
            }
        }
    }

}
