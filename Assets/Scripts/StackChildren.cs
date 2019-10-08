using UnityEngine;
using UnityEngine.UI;

public class StackChildren : MonoBehaviour
{
    [SerializeField] bool scrollToBottom = true;

    RectTransform rectTransform;
    ScrollRect scrollRect;

    private void Awake()
    {
        if (!(transform is RectTransform))
        {
            Debug.LogError("This is not on a UI element!");
            return;
        }

        scrollRect = GetComponentInParent<ScrollRect>();

        rectTransform = (RectTransform)transform;
        Resize();
    }

    public void Resize()
    {
        float height = 0;

        foreach (var child in rectTransform)
        {
            var rectTransform = child as RectTransform;

            if (!rectTransform || !rectTransform.gameObject.activeInHierarchy)
                continue;

            rectTransform.anchoredPosition = new Vector2(0, height);
            height -= rectTransform.sizeDelta.y;
        }

        rectTransform.sizeDelta = new Vector2(0, -height);

        if (scrollRect && scrollToBottom)
        {
            scrollRect.verticalNormalizedPosition = 0;
        }
    }
}
