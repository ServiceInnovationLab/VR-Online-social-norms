using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

public class ImageAppender : MonoBehaviour
{
    public Color colour;

    public UnityEvent onComplete;

    public FloatRange timeBetweenMessages;

    public Sprite[] images;

    public StackChildren stackChildren;

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
        if (initialDleay > 0)
        {
            yield return new WaitForSeconds(initialDleay);
        }

        int lastMessageShown = 0;

        while (lastMessageShown < images.Length)
        {
            if (timeBetweenMessages.max > 0)
            {
                yield return new WaitForSeconds(timeBetweenMessages.GetValue());
            }

            int index = lastMessageShown;

            var item = Instantiate(prefab, stackChildren.transform);
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

            if (scrollToBottom && rect && (Time.time - lastScrollTime > timeBetweenMessages.min * 0.75f))
            {
                rect.verticalNormalizedPosition = placeOnTop ? 1 : 0;

                if (rectDetector)
                {
                    rectDetector.Restart();
                }
            }

            lastMessageShown++;

            if (lastMessageShown >= images.Length)
            {
                item.GetComponent<HighlightImage>().enabled = false;
                item.GetComponent<Image>().color = colour;
            }
        }

        onComplete?.Invoke();
    }
}
