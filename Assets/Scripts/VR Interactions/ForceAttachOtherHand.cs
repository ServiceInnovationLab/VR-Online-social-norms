using UnityEngine;
using VRTK;

[RequireComponent(typeof(VRTK_InteractableObject))]
public class ForceAttachOtherHand : MonoBehaviour
{
    [SerializeField] AttachToControllerNoGrab objectToKeepInOtherHand;

    VRTK_InteractableObject interactableObject;

    private void Awake()
    {
        interactableObject = GetComponent<VRTK_InteractableObject>();
        interactableObject.InteractableObjectGrabbed += InteractableObjectGrabbed;
        interactableObject.InteractableObjectUngrabbed += InteractableObjectUngrabbed;
    }

    private void InteractableObjectUngrabbed(object sender, InteractableObjectEventArgs e)
    {
        objectToKeepInOtherHand.gameObject.SetActive(false);
    }

    private void InteractableObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        var thisController = e.interactingObject.name;

        if (!objectToKeepInOtherHand.gameObject.activeInHierarchy)
        {
            objectToKeepInOtherHand.gameObject.SetActive(true);
        }

        objectToKeepInOtherHand.enabled = false;
        objectToKeepInOtherHand.controllerHand = thisController.Contains("Left") ? SDK_BaseController.ControllerHand.Right : SDK_BaseController.ControllerHand.Left;
        objectToKeepInOtherHand.enabled = true;
    }
}
