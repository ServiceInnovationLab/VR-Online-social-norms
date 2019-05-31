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
    Vector2 position = Vector2.zero;

    public void SendMessageToFeed(string text)
    {
        DisplayMessage(text);
    }

    public void SendMessageToFeed(InputField input)
    {
        DisplayMessage(input.text);
        input.text = "";
    }

    public void StopFeed()
    {
        StopAllCoroutines();
    }    

    void Start()
    {
        scrollRect = messageContainer.GetComponentInParent<ScrollRect>();

        StartCoroutine(DisplayMessages());
    }

    IEnumerator DisplayMessages()
    {
        int lastMessageShown = 0;

        while (lastMessageShown < messageFeed.messages.Count)
        {
            DisplayMessage(messageFeed.messages[lastMessageShown]);
            lastMessageShown++;

            yield return new WaitForSeconds(timeBetweenMessages.GetValue());
        }
    }

    void DisplayMessage(string text)
    {
        var messageDisplay = Instantiate(messagePrefab, messageContainer);
        messageDisplay.gameObject.SetActive(true);
        var messageText = messageDisplay.GetComponentInChildren<Text>();

        if (messageText)
        {
            messageText.text = text;
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

        if (scrollRect && scrollToBottom)
        {
            scrollRect.verticalNormalizedPosition = 0;
        }
    }
}