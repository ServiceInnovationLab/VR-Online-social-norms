using System.Collections;
using UnityEngine;
using VRTK;
using VRTK.Highlighters;

public enum Closeness
{
    NearTouch,
    Touched
}

[RequireComponent(typeof(VRTK_InteractableObject))]
public class FlashUntilNear : MonoBehaviour
{
    VRTK_InteractableObject interactableObject;

    [SerializeField] Closeness closeness = Closeness.NearTouch;
    [SerializeField] protected Color highlightColour = Color.clear;
    [SerializeField] float time = 0.5f;
    [SerializeField] float maxTime = 0;

    VRTK_MaterialColorSwapHighlighter highlighter;
    VRTK_InteractObjectHighlighter highlighterObject;

    void OnEnable()
    {
        if (highlighterObject)
        {
            highlighterObject.InteractObjectHighlighterHighlighted += InteractObjectHighlighterHighlighted;
        }
        else
        {
            highlighter = new VRTK_MaterialColorSwapHighlighter();
            highlighter.Initialise(highlightColour, gameObject);
        }

        StartCoroutine(DoFlashing());

        if (closeness == Closeness.NearTouch)
        {
            interactableObject.InteractableObjectNearTouched += InteractableObjectNearTouched;
        }
        else if (closeness == Closeness.Touched)
        {
            interactableObject.InteractableObjectTouched += InteractableObjectNearTouched;
        }
        else
        {
            Debug.LogError("Unknown closeness value");
        }
    }

    private void InteractObjectHighlighterHighlighted(object sender, InteractObjectHighlighterEventArgs e)
    {
        StopAllCoroutines();
        enabled = false;
    }

    private void OnDisable()
    {
        StopAllCoroutines();

        if (highlighterObject)
        {
            highlighterObject.Unhighlight();
        }

        if (highlighter)
        {
            highlighter.Unhighlight();
        }
    }

    IEnumerator DoFlashing()
    {
        int maxTimes = maxTime > 0 ? Mathf.CeilToInt(maxTime / time) + 1 : -1;

        while (maxTimes == -1 || maxTimes-- > 0)
        {
            yield return new WaitForSeconds(time);

            if (highlighterObject)
            {
                highlighterObject.Highlight(highlightColour);
            }
            else if (highlighter)
            {
                highlighter.Highlight(highlightColour);
            }

            yield return new WaitForSeconds(time);

            if (highlighterObject)
            {
                highlighterObject.Unhighlight();
            }

            if (highlighter)
            {
                highlighter.Unhighlight();
            }
        }

        enabled = false;
    }

    private void Awake()
    {
        interactableObject = GetComponent<VRTK_InteractableObject>();
        highlighterObject = GetComponent<VRTK_InteractObjectHighlighter>();
    }

    private void InteractableObjectNearTouched(object sender, InteractableObjectEventArgs e)
    {
        StopAllCoroutines();

        if (highlighterObject && closeness != Closeness.Touched)
        {
            highlighterObject.Unhighlight();
        }

        if (highlighter)
        {
            highlighter.Unhighlight();
            DestroyImmediate(highlighter);
        }

        enabled = false;
    }
}
