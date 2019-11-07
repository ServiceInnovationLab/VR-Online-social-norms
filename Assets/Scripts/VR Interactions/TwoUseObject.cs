using UnityEngine;
using VRTK;
using UnityEngine.Events;

[RequireComponent(typeof(VRTK_InteractableObject))]
public class TwoUseObject : MonoBehaviour
{
    public bool forceUse;
    public bool canUse;
    public UnityEvent onFirstUse;
    public UnityEvent onSecondUseUse;

    bool canUseSecondAction;
    bool firstUse = true;

    VRTK_InteractableObject interactableObject;
    VRTK_InteractObjectHighlighter highlighter;

    FlashUntilNear flashUntilNear;

    public void AllowFirstUse()
    {
        if (highlighter)
        {
            highlighter.Unhighlight();
            highlighter.enabled = true;
        }
        if (flashUntilNear)
        {
            flashUntilNear.enabled = true;
        }

        interactableObject.isUsable = true;
    }

    public void AllowSecondUse()
    {
        if (forceUse)
        {
            enabled = true;
        }

        if (enabled)
        {
            canUseSecondAction = true;
            highlighter.enabled = true;
            interactableObject.isUsable = true;
        }
        else
        {
            highlighter.Unhighlight();
            highlighter.enabled = false;
            interactableObject.isUsable = false;
        }
    }

    public void DisallowUse()
    {
        if (highlighter)
        {
            highlighter.Unhighlight();
            highlighter.enabled = false;
        }
        if (flashUntilNear)
        {
            flashUntilNear.enabled = false;
        }

        interactableObject.isUsable = false;
    }

    private void Awake()
    {
        interactableObject = GetComponent<VRTK_InteractableObject>();        
        highlighter = GetComponent<VRTK_InteractObjectHighlighter>();
        flashUntilNear = GetComponent<FlashUntilNear>();

        if (!canUse)
        {
            DisallowUse();
        }
    }

    private void OnEnable()
    {
        interactableObject.InteractableObjectUsed += InteractableObjectUsed;
    }

    private void InteractableObjectUsed(object sender, InteractableObjectEventArgs e)
    {
        if (firstUse)
        {
            onFirstUse?.Invoke();
            firstUse = false;
        }
        else if (canUseSecondAction)
        {
            onSecondUseUse?.Invoke();
        }
        highlighter.Unhighlight();
        highlighter.enabled = false;
        interactableObject.isUsable = false;
    }
}
