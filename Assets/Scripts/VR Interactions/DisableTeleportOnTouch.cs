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


    private void AddTouching(GameObject gameObject, int controllerIndex)
    {
        controllersTouching[controllerIndex].Add(gameObject);
        OnChange();

        var destroyHandler = gameObject.AddComponent<OnDestroyHandler>();
        destroyHandler.owner = gameObject;
        destroyHandler.OnDestroyed += OnDisablerDestroyed;
    }

    private void RemoveTouching(GameObject gameObject, int controllerIndex, bool raiseChange = true)
    {
        controllersTouching[controllerIndex].Remove(gameObject);
        if (raiseChange)
        {
            OnChange();
        }

        if (gameObject)
        {
            var destroyHandlers = gameObject.GetComponents<OnDestroyHandler>();
            foreach (var handler in destroyHandlers)
            {
                if (handler.owner == gameObject)
                {
                    Destroy(handler);
                }
            }
        }
    }

    private void OnDisablerDestroyed(GameObject gameObject)
    {
        for (int i = 0; i < controllersTouching.Length; i++)
        {
            RemoveTouching(gameObject, i, false);
        }
        OnChange();
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
                AddTouching(a.target, index);
            }
        };

        touch.ControllerNearUntouchInteractableObject += (s, a) =>
        {
            var touching = controllersTouching[index];

            if (touching.Contains(a.target))
            {
                RemoveTouching(a.target, index);                
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
                AddTouching(a.target, index);
            }
        };

        touch.ControllerStartUntouchInteractableObject += (s, a) =>
        {
            var touching = controllersTouching[index];

            if (touching.Contains(a.target))
            {
                RemoveTouching(a.target, index);
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
                pointer.enabled = pointer.IsPointerActive() || !anyTouches;
            }
        }
        else
        {
            for (int i = 0; i < controllers.Length; i++)
            {
                pointers[i].enabled = pointers[i].IsPointerActive() || controllersTouching[i].Count == 0;
            }
        }
    }

    public void AddDisabler(VRTK_ControllerEvents controller, GameObject gameObject)
    {
        if (!enabled)
            return;

        for (int i = 0; i < controllers.Length; i++)
        {
            if (controller == controllers[i])
            {
                AddTouching(gameObject, i);                
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
                RemoveTouching(gameObject, i);
                return;
            }
        }

        Debug.LogError("Invalid controller");
    }

}
