using UnityEngine;
using VRTK;

/// <summary>
/// Provides some basic functions to be used for UnityEvents via the inspector
/// </summary>
[CreateAssetMenu(menuName = "Utilities/BasicFunctions")]
public class BasicFunctions : ScriptableObject
{
    /// <summary>
    /// Toggles the active state of the given game object.
    /// </summary>
    /// <param name="gameObject">The <see cref="GameObject"/> to change</param>
    public void ToggleObject(GameObject gameObject)
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }

    /// <summary>
    /// Destroys the given game object
    /// </summary>
    /// <param name="gameObject">The <see cref="GameObject"/> to destroy</param>
    public void DestroyObject(GameObject gameObject)
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Forces grabbing of the given item
    /// </summary>
    /// <param name="gameObject"></param>
    public void ForceGrabObject(VRTK_InteractableObject gameObject)
    {
        var autoGrab = FindObjectOfType<VRTK_ObjectAutoGrab>();

        if (!autoGrab)
        {
            Debug.LogError("ForceGrabObject - There is no VRTK_ObjectAutoGrab in the scene! It should be attached to one of the controllers");
            return;
        }

        autoGrab.enabled = false;
        gameObject.isGrabbable = true;
        autoGrab.objectToGrab = gameObject;
        autoGrab.enabled = true;
    }

    /// <summary>
    /// Disables teleporting around
    /// </summary>
    public void DisableTeleporting()
    {        
        foreach (var teleporter in FindObjectsOfType<VRTK_Pointer>())
        {
            teleporter.enableTeleport = false;
        }             
    }

    /// <summary>
    /// Enables teleporting around
    /// </summary>
    public void EnableTeleporting()
    {
        foreach (var teleporter in FindObjectsOfType<VRTK_Pointer>())
        {
            teleporter.enableTeleport = true;
        }
    }

    public void AlwaysShowStraightPointer()
    {
        var pointer = FindObjectOfType<VRTK_StraightPointerRenderer>();

        pointer.tracerVisibility = VRTK_BasePointerRenderer.VisibilityStates.AlwaysOn;
        pointer.cursorVisibility = VRTK_BasePointerRenderer.VisibilityStates.AlwaysOn;
        pointer.cursorScaleMultiplier = 20;
    }

    public void NormalStraightPointer()
    {
        var pointer = FindObjectOfType<VRTK_StraightPointerRenderer>();

        pointer.tracerVisibility = VRTK_BasePointerRenderer.VisibilityStates.OnWhenActive;
        pointer.cursorVisibility = VRTK_BasePointerRenderer.VisibilityStates.OnWhenActive;
        pointer.cursorScaleMultiplier = 25;
    }
}