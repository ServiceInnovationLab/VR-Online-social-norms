using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageAppender : MonoBehaviour
{
    public FloatRange timeBetweenMessages;

    public Sprite[] images;

    public StackChildren stackChildren;

    public ScreenMessageFeedView view;

    public Image prefab;

    public bool placeOnTop;

    public float initialDleay;

    public ScrollRect rect;

    public ScrollRectDetector rectDetector;
    public bool scrollToBottom;

    float lastScrollTime = 0;

    private void OnEnable()
    {
       rect.onValueChanged.AddListener((v) =>
       {
           lastScrollTime = Time.time;
       });
        StartCoroutine(DisplayMessages());
    }

    IEnumerator DisplayMessages()
    {
        yield return new WaitForSeconds(initialDleay);

        int lastMessageShown = 0;

        Vector2 position = Vector2.zero;

        if (view)
        {
            position = view.GetPosition();
        }

        while (lastMessageShown < images.Length)
        {
            yield return new WaitForSeconds(timeBetweenMessages.GetValue());

            int index = lastMessageShown;

            var item = stackChildren ? Instantiate(prefab, stackChildren.transform) : Instantiate(prefab, view.transform);
            item.gameObject.SetActive(true);
            item.transform.GetChild(0).GetComponent<Image>().sprite = images[index];

            item.rectTransform.sizeDelta = new Vector2(item.rectTransform.sizeDelta.x, images[index].textureRect.height);

            if (placeOnTop)
            {
                item.transform.SetSiblingIndex(1);
            }

            if (stackChildren)
            {
                stackChildren.Resize();
            }

            if (view)
            {

            }

            if (scrollToBottom && rect && (Time.time - lastScrollTime > timeBetweenMessages.min * 0.75f))
            {
                rect.verticalNormalizedPosition = placeOnTop ? 1 : 0;

                if (rectDetector)
                {
                    rectDetector.Restart();
                }
            }

            lastMessageShown++;
        }

    }
}
