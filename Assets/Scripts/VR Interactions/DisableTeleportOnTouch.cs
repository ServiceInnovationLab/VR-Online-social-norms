using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class DisableTeleportOnTouch : MonoBehaviour
{
    public Closeness closeness = Closeness.Touched;
    public bool affectBoth = false;

    VRTK_Pointer[] pointers;
    VRTK_ControllerEvents[] controllers;
    int[] counts;

    HashSet<GameObject>[] controllersTouching;

    int touchCount = 0;

    private void Awake()
    {
        controllers = GetComponentsInChildren<VRTK_ControllerEvents>();
        counts = new int[controllers.Length];
        pointers = new VRTK_Pointer[controllers.Length];
        controllersTouching = new HashSet<GameObject>[controllers.Length];

        for (int i = 0; i < controllers.Length; i++)
        {
            controllersTouching[i] = new HashSet<GameObject>();

            VRTK_Pointer pointer = controllers[i].GetComponent<VRTK_Pointer>();

            pointers[i] = pointer;

            if (closeness == Closeness.Touched)
            {
                SetupTouch(controllers[i].GetComponent<VRTK_InteractTouch>(), i);
            }
            else
            {
                SetupNearTouch(controllers[i].GetComponent<VRTK_InteractNearTouch>(), i);
            }
        }
    }

    private void SetupNearTouch(VRTK_InteractNearTouch touch, int index)
    {
        if (!touch)
        {
            Debug.LogError("No VRTK_InteractNearTouch given");
            return;
        }

        touch.ControllerNearTouchInteractableObject += (s, a) =>
        {
            var touching = controllersTouching[index];

            if (!touching.Contains(a.target) && !a.target.name.Contains("[NearTouch][CollidersContainer]"))
            {
                touching.Add(a.target);
                AddDisabler(index);
            }
        };

        touch.ControllerNearUntouchInteractableObject += (s, a) =>
        {
            var touching = controllersTouching[index];

            if (touching.Contains(a.target))
            {
                touching.Remove(a.target);
                RemoveDisabler(index);
            }
        };
    }

    private void SetupTouch(VRTK_InteractTouch touch, int index)
    {
        if (!touch)
        {
            Debug.LogError("No VRTK_InteractTouch given");
            return;
        }

        touch.ControllerStartTouchInteractableObject += (s, a) =>
        {
            AddDisabler(index);
        };

        touch.ControllerStartUntouchInteractableObject += (s, a) =>
        {
            RemoveDisabler(index);
        };
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
