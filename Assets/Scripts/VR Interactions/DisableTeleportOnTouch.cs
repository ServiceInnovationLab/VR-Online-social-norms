using UnityEngine;
using VRTK;

public class DisableTeleportOnTouch : MonoBehaviour
{
    public bool affectBoth = false;

    VRTK_Pointer[] pointers;
    VRTK_ControllerEvents[] controllers;
    int[] counts;

    int touchCount = 0;

    private void Awake()
    {
        controllers = GetComponentsInChildren<VRTK_ControllerEvents>();
        counts = new int[controllers.Length];
        pointers = new VRTK_Pointer[controllers.Length];

        for (int i = 0; i < controllers.Length; i++)
        {
            VRTK_InteractTouch touch = controllers[i].GetComponent<VRTK_InteractTouch>();
            VRTK_Pointer pointer = controllers[i].GetComponent<VRTK_Pointer>();

            pointers[i] = pointer;

            int copy = i;

            touch.ControllerStartTouchInteractableObject += (s, a) =>
            {
                AddDisabler(copy);
            };

            touch.ControllerStartUntouchInteractableObject += (s, a) =>
            {
                RemoveDisabler(copy);
            };
        }
    }

    private void AddDisabler(int index)
    {
        if (affectBoth)
        {
            touchCount++;

            foreach (var pointer in pointers)
            {
                pointer.enabled = false;
            }
        }
        else
        {
            counts[index]++;
            pointers[index].enabled = false;
        }
    }

    private void RemoveDisabler(int index)
    {
        if (affectBoth)
        {
            if (--touchCount <= 0)
            {
                touchCount = 0;
                foreach (var pointer in pointers)
                {
                    pointer.enabled = true;
                }
            }
        }
        else
        {
            counts[index]--;
            if (counts[index] <= 0)
            {
                counts[index] = 0;
                pointers[index].enabled = true;
            }
        }
    }

    public void AddDisabler(VRTK_ControllerEvents controller)
    {
        for (int i = 0; i < controllers.Length; i++)
        {
            if (controller == controllers[i])
            {
                AddDisabler(i);
                return;
            }
        }

        Debug.LogError("Invalid controller");
    }

    public void RemoveDisabler(VRTK_ControllerEvents controller)
    {
        for (int i = 0; i < controllers.Length; i++)
        {
            if (controller == controllers[i])
            {
                RemoveDisabler(i);
                return;
            }
        }

        Debug.LogError("Invalid controller");
    }

}
