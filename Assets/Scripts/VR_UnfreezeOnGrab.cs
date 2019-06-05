using UnityEngine;
using VRTK;

/// <summary>
/// This component will enhance the physics settings of the VR world. Physics bodies can
/// start with the position and rotation frozen to allow better placement.
/// This script unfreezes it when picked up.
/// </summary>
[RequireComponent(typeof(VRTK_InteractGrab))]
public class VR_UnfreezeOnGrab : MonoBehaviour
{
    VRTK_InteractGrab interactGrab;

    void Awake()
    {
        interactGrab = GetComponent<VRTK_InteractGrab>();
        interactGrab.ControllerStartGrabInteractableObject += ControllerStartGrabInteractableObject;
    }

    private void ControllerStartGrabInteractableObject(object sender, ObjectInteractEventArgs e)
    {
        var rigidBody = e.target.GetComponentInChildren<Rigidbody>();

        if (rigidBody.constraints == RigidbodyConstraints.FreezeAll)
        {
            rigidBody.constraints = RigidbodyConstraints.None;
        }
    }

}
