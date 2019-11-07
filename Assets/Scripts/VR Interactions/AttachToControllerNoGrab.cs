using UnityEngine;
using System.Collections;
using VRTK;

/// <summary>
/// Attaches the object to a controller without it being the 'grabbed' object (or being interactable)
/// </summary>
public class AttachToControllerNoGrab : MonoBehaviour
{
    public SDK_BaseController.ControllerHand controllerHand;
    public Vector3 offset = Vector3.zero;

    private void OnEnable()
    {
        StartCoroutine(DoAttach());  
    }

    IEnumerator DoAttach()
    {
        while (true)
        {
            var controller = VRTK_DeviceFinder.GetControllerReferenceForHand(controllerHand);

            if (controller == null || !controller.scriptAlias)
            {
                yield return new WaitForFixedUpdate();
                continue;
            }

            var grab = controller.scriptAlias.GetComponent<VRTK_InteractGrab>();

            if (!grab || !grab.controllerAttachPoint)
            {
                Debug.LogError("Couldn't find attachment point!");
                yield return new WaitForFixedUpdate();
                continue;
            }

            gameObject.transform.SetParent(grab.controllerAttachPoint.transform);
            gameObject.transform.localPosition = offset;
            gameObject.transform.localRotation = Quaternion.identity;


            var pointer = GetComponent<VRTK_UIPointer>();
            if (pointer)
            {
                pointer.controllerEvents = grab.GetComponent<VRTK_ControllerEvents>();
                pointer.enabled = true;
            }

            break;
        }
    }
}
