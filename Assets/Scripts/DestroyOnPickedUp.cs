using UnityEngine;
using VRTK;

public class DestroyOnPickedUp : MonoBehaviour
{

    VRTK_InteractableObject interactableObject;

    private void Awake()
    {
        interactableObject = GetComponentInParent<VRTK_InteractableObject>();

        if (!interactableObject)
        {
            Debug.LogError("No interactable object parent!", gameObject);
            return;
        }

        interactableObject.InteractableObjectGrabbed += InteractableObjectGrabbed;
    }

    private void InteractableObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        interactableObject.InteractableObjectGrabbed -= InteractableObjectGrabbed;
    }
}
