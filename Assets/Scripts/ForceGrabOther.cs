using UnityEngine;
using VRTK;

public class ForceGrabOther : MonoBehaviour
{
    [SerializeField] VRTK_InteractableObject objectToKeepInOtherHand;

    VRTK_InteractableObject interactableObject;
    VRTK_ObjectAutoGrab[] controllers;

    private void Awake()
    {
        controllers = FindObjectsOfType<VRTK_ObjectAutoGrab>();

        interactableObject = GetComponent<VRTK_InteractableObject>();

        interactableObject.InteractableObjectGrabbed += InteractableObjectGrabbed;
    }

    private void InteractableObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        var thisController = e.interactingObject.GetComponent<VRTK_ObjectAutoGrab>();

        if (!objectToKeepInOtherHand.gameObject.activeInHierarchy)
        {
            objectToKeepInOtherHand.gameObject.SetActive(true);
        }

        foreach (var grab in controllers)
        {
            if (grab == thisController)
                continue;

            grab.enabled = false;
            grab.objectToGrab = objectToKeepInOtherHand;
            grab.enabled = true;
            break;
        }
    }
}
