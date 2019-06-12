using UnityEngine;
using VRTK;

/// <summary>
/// Desribes what direction the controls are specified for
/// </summary>
public enum ControlDirection
{
    /// <summary>
    /// Control order doesn't matter
    /// </summary>
    Either,

    /// <summary>
    /// Controls are setup for this object in the left hand
    /// </summary>
    Left,

    /// <summary>
    /// Controls are setup for this object in the right hand
    /// </summary>
    Right
}

[RequireComponent(typeof(VRTK_InteractableObject))]
public class ShowControlsOnGrab : MonoBehaviour
{
    [SerializeField] string controlHelpName;
    [SerializeField] ControlDirection controlsDirection = ControlDirection.Either;


    VRTK_InteractableObject interactableObject;
    HelpTipsManager manager;
    string previousHelp;

    void Awake()
    {
        interactableObject = GetComponent<VRTK_InteractableObject>();
        GetHelpManager();
    }

    private void GetHelpManager()
    {
        if (!manager)
        {
            manager = FindObjectOfType<HelpTipsManager>();
        }
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
        GetHelpManager();
        manager.ShowHelp(previousHelp);
    }

    void InteractableObject_Grabbed(object sender, InteractableObjectEventArgs e)
    {
        GetHelpManager();
        previousHelp = manager.GetCurrentHelp();

        bool swapLeftAndRight = false;

        if (controlsDirection != ControlDirection.Either)
        {
            bool pickedUpWithLeft = e.interactingObject.gameObject.name.Contains("LeftController");

            if (pickedUpWithLeft && controlsDirection == ControlDirection.Right ||
                !pickedUpWithLeft && controlsDirection == ControlDirection.Left)
            {
                swapLeftAndRight = true;
            }
        }

        manager.ShowHelp(controlHelpName, swapLeftAndRight);
        manager.ShowHelp(true);
    }
}