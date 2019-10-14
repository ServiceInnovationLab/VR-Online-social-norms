using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class ScrollRectLastItemClick : MonoBehaviour
{
    public UnityEvent onClicked;

    public ScrollRect rect;

    public RectTransform container;

    private void OnEnable()
    {
        
    }


    public void Check()
    {
        var position = rect.content.anchoredPosition.y + ((RectTransform)rect.transform).rect.height;

        var lastChild = (RectTransform)container.GetChild(container.childCount - 1);

        var lastPos = lastChild.anchoredPosition.y + lastChild.rect.center.y;

        if (Mathf.Abs(position) > Mathf.Abs(lastPos))
        {
            onClicked?.Invoke();
        }
    }
}
