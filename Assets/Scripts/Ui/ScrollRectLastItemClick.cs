using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class ScrollRectLastItemClick : MonoBehaviour
{

    public UnityEvent onClicked;

    public ScrollRect rect;

    public RectTransform container;


    public void Check()
    {
        var position = rect.content.anchoredPosition.y + ((RectTransform)rect.transform).rect.height;

        var lastPos = ((RectTransform)container.GetChild(container.childCount - 1)).anchoredPosition.y;

        if (Mathf.Abs(position) > Mathf.Abs(lastPos))
        {
            onClicked?.Invoke();
        }
    }
}
