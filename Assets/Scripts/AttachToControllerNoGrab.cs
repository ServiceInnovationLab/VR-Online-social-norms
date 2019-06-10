using UnityEngine;
using VRTK;

/// <summary>
/// Attaches the object to a controller without it being the 'grabbed' object (or being interactable)
/// </summary>
public class AttachToControllerNoGrab : MonoBehaviour
{
    public SDK_BaseController.ControllerHand controllerHand;

    private void OnEnable()
    {
        var controller = VRTK_DeviceFinder.GetControllerReferenceForHand(controllerHand);

        var grab = controller.scriptAlias.GetComponent<VRTK_InteractGrab>();

        if (!grab || !grab.controllerAttachPoint)
        {
            Debug.LogError("Couldn't find attachment point!");
            return;
        }

        gameObject.transform.SetParent(grab.controllerAttachPoint.transform);
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localRotation = Quaternion.identity;


        var pointer = GetComponent<VRTK_UIPointer>();
        if (pointer)
        {
            pointer.controllerEvents = grab.GetComponent<VRTK_ControllerEvents>();
            pointer.enabled = true;
        }
    }
}
