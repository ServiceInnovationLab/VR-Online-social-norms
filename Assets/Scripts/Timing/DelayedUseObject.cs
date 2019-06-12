﻿using UnityEngine;
using VRTK;
using UnityEngine.Events;

[RequireComponent(typeof(VRTK_InteractableObject))]
public class DelayedUseObject : MonoBehaviour
{
    public UnityEvent onUse;
    [SerializeField] bool disallowUseAfterUse;
    [SerializeField] bool canUse = false;

    VRTK_InteractableObject interactableObject;
    VRTK_InteractObjectHighlighter highlighter;

    public void AllowUse()
    {
        if (highlighter)
        {
            highlighter.enabled = true;
        }
        interactableObject.isUsable = true;
        canUse = true;
    }

    public void DisallowUse()
    {
        if (highlighter)
        {
            highlighter.Unhighlight();
            highlighter.enabled = false;
        }
        canUse = false;
        interactableObject.isUsable = false;
    }

    private void Awake()
    {
        interactableObject = GetComponent<VRTK_InteractableObject>();
        highlighter = GetComponent<VRTK_InteractObjectHighlighter>();

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
        if (!canUse)
            return;

        onUse?.Invoke();

        if (disallowUseAfterUse)
        {
            DisallowUse();
        }
    }
}
