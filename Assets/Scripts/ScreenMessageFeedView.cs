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

    [Tooltip("The object where the messages will be appended to")]
    [SerializeField] RectTransform messageContainer;

    [Tooltip("Set to start the feed automatically when the object is enabled")]
    [SerializeField] bool startOnEnable = true;

    ScrollRect scrollRect;
    Vector2 position = Vector2.zero;
    bool forceComplete;

    public void SendMessageToFeed(string text)
    {
        if (!string.IsNullOrWhiteSpace(text))
        {
            DisplayMessage(new Message() { message = text });
        }
    }

    public void SendMessageToFeed(InputField input)
    {
        SendMessageToFeed(input.text);
        input.text = "";
    }

    public void CompleteFeed()
    {
        forceComplete = true;
    }

    public void StopFeed()
    {
        StopAllCoroutines();
    }

    public void StartFeed()
    {
        StartCoroutine(DisplayMessages());
    }

    void Awake()
    {
        scrollRect = messageContainer.GetComponentInParent<ScrollRect>();
    }

    private void OnEnable()
    {
        if (startOnEnable)
        {
            StartFeed();
        }
    }

    IEnumerator DisplayMessages()
    {
        int lastMessageShown = 0;

        while (lastMessageShown < messageFeed.messages.Count)
        {
            DisplayMessage(messageFeed.messages[lastMessageShown]);
            lastMessageShown++;

            if (!forceComplete)
            {
                yield return new WaitForSeconds(timeBetweenMessages.GetValue());
            }
        }
    }

    void DisplayMessage(Message theMessage)
    {
        var messageDisplay = Instantiate(messagePrefab, messageContainer);

        var message = messageDisplay.GetComponent<ScreenMessage>();

        if (!message)
        {
            Debug.LogError("No message!");
            return;
        }

        message.message = theMessage.message;

        if (theMessage.profilePicture)
        {
            message.profilePicture = theMessage.profilePicture;
        }

        if (!string.IsNullOrEmpty(theMessage.fromProfile))
        {
            message.from = theMessage.fromProfile;
        }

        if (!string.IsNullOrEmpty(theMessage.fromTag))
        {
            message.fromTag = theMessage.fromTag;
        }

        messageDisplay.gameObject.SetActive(true);

        messageDisplay.anchoredPosition = position;

        position.y -= messageDisplay.rect.height;
        messageContainer.sizeDelta = new Vector2(0, -position.y);

        if (scrollRect && scrollToBottom)
        {
            scrollRect.verticalNormalizedPosition = 0;
        }
    }
}