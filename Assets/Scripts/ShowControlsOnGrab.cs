using UnityEngine;
using VRTK;

[RequireComponent(typeof(VRTK_InteractableObject))]
public class ShowControlsOnGrab : MonoBehaviour
{
    [SerializeField] string controlHelpName;

    VRTK_InteractableObject interactableObject;
    HelpTipsManager manager;
    string previousHelp;

    void Awake()
    {
        interactableObject = GetComponent<VRTK_InteractableObject>();
        manager = FindObjectOfType<HelpTipsManager>();
    }

    void OnEnable()
    {
        interactableObject.InteractableObjectGrabbed += InteractableObject_Grabbed;
        interactableObject.InteractableObjectUngrabbed += InteractableObject_Ungrabbed;
    }

    void OnDisable()
    {
        interactableObject.InteractableObjectGrabbed -= InteractableObject_Grabbed;
        interactableObject.InteractableObjectUngrabbed -= InteractableObject_Ungrabbed;
    }

    void InteractableObject_Ungrabbed(object sender, InteractableObjectEventArgs e)
    {
        manager.ShowHelp(previousHelp);
    }

    void InteractableObject_Grabbed(object sender, InteractableObjectEventArgs e)
    {
        previousHelp = manager.GetCurrentHelp();
        manager.ShowHelp(controlHelpName);
        manager.ShowHelp(true);
    }
}