using UnityEngine;
using VRTK;
using UnityEngine.Events;

[RequireComponent(typeof(VRTK_InteractableObject))]
public class TwoUseObject : MonoBehaviour
{
    public UnityEvent onFirstUse;
    public UnityEvent onSecondUseUse;

    bool canUseSecondAction;
    bool firstUse = true;

    VRTK_InteractableObject interactableObject;
    VRTK_InteractObjectHighlighter highlighter;

    public void AllowSecondUse()
    {
        canUseSecondAction = true;
        highlighter.enabled = true;
    }

    private void Awake()
    {
        interactableObject = GetComponent<VRTK_InteractableObject>();        
        highlighter = GetComponent<VRTK_InteractObjectHighlighter>();        
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
    }
}
