using UnityEngine;
using VRTK;

[RequireComponent(typeof(VRTK_InteractableObject))]
public class DestroyOnUngrab : MonoBehaviour
{
    VRTK_InteractableObject interactableObject;

    private void Awake()
    {
        interactableObject = GetComponent<VRTK_InteractableObject>();

        interactableObject.InteractableObjectUngrabbed += InteractableObjectUngrabbed;
    }

    private void InteractableObjectUngrabbed(object sender, InteractableObjectEventArgs e)
    {
        Destroy(gameObject);
    }

}
