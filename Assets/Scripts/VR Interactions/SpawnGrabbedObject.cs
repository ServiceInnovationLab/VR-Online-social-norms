using UnityEngine;
using VRTK;

/// <summary>
/// This behaviour is to be called by a Unity Event.
/// It will spawn a new object an have the controller automatically pick it up
/// </summary>
[RequireComponent(typeof(VRTK_ObjectAutoGrab))]
public class SpawnGrabbedObject : MonoBehaviour
{
    VRTK_ObjectAutoGrab autoGrab;

    private void Awake()
    {
        autoGrab = GetComponent<VRTK_ObjectAutoGrab>();
    }

    public void SpawnObject(VRTK_InteractableObject prefab)
    {
        autoGrab.enabled = false;
        var instance = Instantiate(prefab);
        autoGrab.objectToGrab = instance;
        autoGrab.enabled = true;
    }
}
