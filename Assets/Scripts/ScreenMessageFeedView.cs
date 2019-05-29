using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenMessageFeedView : MonoBehaviour
{
    public bool scrollToBottom = true;

    [Tooltip("The messages to be displayed")]
    [SerializeField] MessageFeed messageFeed;

    [Tooltip("The time variances between messages showing up in the feed, in seconds")]
    [SerializeField] FloatRange timeBetweenMessages;

    [Tooltip("The message display element to be used for showing a message")]
    [SerializeField] RectTransform messagePrefab;

    [Tooltip("The object where the messages will be appended toa")]
    [SerializeField] RectTransform messageContainer;

    ScrollRect scrollRect;

    void Start()
    {
        scrollRect = messageContainer.GetComponentInParent<ScrollRect>();

        StartCoroutine(DisplayMessages());
    }

    IEnumerator DisplayMessages()
    {
        int lastMessageShown = 0;
        Vector2 position = Vector2.zero;

        while (lastMessageShown < messageFeed.messages.Count)
        {
            var messageDisplay = Instantiate(messagePrefab, messageContainer);
            messageDisplay.gameObject.SetActive(true);
            var messageText = messageDisplay.GetComponentInChildren<Text>();

            if (messageText)
            {
                messageText.text = messageFeed.messages[lastMessageShown];
            }
#if UNITY_EDITOR
            else
            {
                print("Message prefab has no text element!");
            }
#endif

            messageDisplay.anchoredPosition = position;

            position.y -= messageDisplay.rect.height;
            messageContainer.sizeDelta = new Vector2(0, -position.y);

            lastMessageShown++;

            if (scrollRect && scrollToBottom)
            {
                scrollRect.verticalNormalizedPosition = 0;
            }

            yield return new WaitForSeconds(timeBetweenMessages.GetValue());
        }
    }
}