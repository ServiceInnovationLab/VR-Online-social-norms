using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class DisableTeleportOnTouch : MonoBehaviour
{
    public Closeness closeness = Closeness.Touched;
    public bool affectBoth = false;

    VRTK_Pointer[] pointers;
    VRTK_ControllerEvents[] controllers;
    HashSet<GameObject>[] controllersTouching;

    private void Awake()
    {
        controllers = GetComponentsInChildren<VRTK_ControllerEvents>();
        pointers = new VRTK_Pointer[controllers.Length];
        controllersTouching = new HashSet<GameObject>[controllers.Length];
    }

    private void OnEnable()
    {
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
                OnChange();
            }
        };

        touch.ControllerNearUntouchInteractableObject += (s, a) =>
        {
            var touching = controllersTouching[index];

            if (touching.Contains(a.target))
            {
                touching.Remove(a.target);
                OnChange();
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
            var touching = controllersTouching[index];

            if (!touching.Contains(a.target) && !a.target.name.Contains("[NearTouch][CollidersContainer]"))
            {
                touching.Add(a.target);
                OnChange();
            }
        };

        touch.ControllerStartUntouchInteractableObject += (s, a) =>
        {
            var touching = controllersTouching[index];

            if (touching.Contains(a.target))
            {
                touching.Remove(a.target);
                OnChange();
            }
        };
    }

    private void OnChange()
    {
        if (affectBoth)
        {
            bool anyTouches = false;

            for (int i = 0; i < controllers.Length; i++)
            {
                if (controllersTouching[i].Count > 0)
                {
                    anyTouches = true;
                    break;
                }
            }

            foreach (var pointer in pointers)
            {
                pointer.enabled = !anyTouches;
            }
        }
        else
        {
            for (int i = 0; i < controllers.Length; i++)
            {
                pointers[i].enabled = controllersTouching[i].Count == 0;
            }
        }
    }

    public void RemoveDeleted()
    {
        StartCoroutine(DoRemove());
    }

    IEnumerator DoRemove()
    {
        yield return new WaitForSeconds(0.5f);
        yield return new WaitForEndOfFrame();

        foreach (var touching in controllersTouching)
        {
            var toRemove = new HashSet<GameObject>();

            foreach (var obj in touching)
            {
                if (!obj)
                {
                    toRemove.Add(obj);
                }
            }

            foreach (var remove in toRemove)
            {
                touching.Remove(remove);
            }
        }

        OnChange();
    }

    public void AddDisabler(VRTK_ControllerEvents controller, GameObject gameObject)
    {
        if (!enabled)
            return;

        for (int i = 0; i < controllers.Length; i++)
        {
            if (controller == controllers[i])
            {
                controllersTouching[i].Add(gameObject);
                OnChange();
                return;
            }
        }

        Debug.LogError("Invalid controller");
    }

    public void RemoveDisabler(VRTK_ControllerEvents controller, GameObject gameObject)
    {
        if (!enabled)
            return;

        for (int i = 0; i < controllers.Length; i++)
        {
            if (controller == controllers[i])
            {
                controllersTouching[i].Remove(gameObject);
                OnChange();
                return;
            }
        }

        Debug.LogError("Invalid controller");
    }

}
