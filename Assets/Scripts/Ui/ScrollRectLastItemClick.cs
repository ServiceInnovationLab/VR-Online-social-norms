using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ScrollRectLastItemClick : MonoBehaviour
{
    public UnityEvent onClicked;

    public ScrollRect rect;

    public RectTransform container;

    private void OnEnable()
    {
        
    }

    public bool IsLastItemShowing()
    {
        var position = rect.content.anchoredPosition.y + ((RectTransform)rect.transform).rect.height;

        var lastChild = (RectTransform)container.GetChild(container.childCount - 1);

        var lastPos = lastChild.anchoredPosition.y + lastChild.rect.center.y;

        if (Mathf.Abs(position) > Mathf.Abs(lastPos))
        {
            return true;
        }

        return false;
    }


    public void Check()
    {
        if (IsLastItemShowing())
        {
            onClicked?.Invoke();
        }
    }
}
