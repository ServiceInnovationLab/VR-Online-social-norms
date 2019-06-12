using System.Collections;
using UnityEngine;
using VRTK.Highlighters;

public class FlashingObject : MonoBehaviour
{
    [SerializeField] protected Color highlightColour = Color.clear;
    [SerializeField] float time = 0.5f;

    VRTK_MaterialColorSwapHighlighter highlighter;

    protected virtual void OnEnable()
    {
        highlighter = new VRTK_MaterialColorSwapHighlighter();
        highlighter.Initialise(highlightColour, gameObject);

        StartCoroutine(DoFlashing());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        if (highlighter)
        {
            highlighter.Unhighlight();
        }
    }

    IEnumerator DoFlashing()
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            highlighter.Highlight(highlightColour);
            yield return new WaitForSeconds(time);
            highlighter.Unhighlight();
        }
    }
}
