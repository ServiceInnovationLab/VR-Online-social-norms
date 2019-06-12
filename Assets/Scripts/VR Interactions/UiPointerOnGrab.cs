using UnityEngine;
using VRTK;

[RequireComponent(typeof(VRTK_InteractableObject))]
public class UiPointerOnGrab : MonoBehaviour
{
    VRTK_InteractableObject interactableObject;

    private void Awake()
    {
        interactableObject = GetComponent<VRTK_InteractableObject>();
    }

    private void OnEnable()
    {
        interactableObject.InteractableObjectGrabbed += InteractableObjectGrabbed;
    }

    private void InteractableObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        VRTK_UIPointer pointer = GetComponent<VRTK_UIPointer>();
        if (!pointer)
        {
            pointer = gameObject.AddComponent<VRTK_UIPointer>();
        }
        pointer.enabled = false;

        pointer.activationMode = VRTK_UIPointer.ActivationMethods.AlwaysOn;
        pointer.controllerEvents = e.interactingObject.GetComponent<VRTK_ControllerEvents>();
        pointer.enabled = true;
    }

}
