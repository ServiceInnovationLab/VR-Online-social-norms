using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

public class ScreenMessageFeedView : MonoBehaviour
{
    public bool scrollToBottom = true;

    MessageFeed messageFeed;

    private MessageFeed MessageFeed
    {
        get
        {

            if (messageFeed == null)
                messageFeed = SocialMediaScenarioPicker.Instance.CurrentScenario.GetMessageFeed(MessageFeedType);

            return messageFeed;
        }
    }

    [SerializeField] UnityEvent OnComplete = new UnityEvent();

    [SerializeField] UnityEvent OnNewMessage = new UnityEvent();

    [SerializeField] bool loop = false;

    [SerializeField] SocialMediaScenatioMessageFeedType MessageFeedType = SocialMediaScenatioMessageFeedType.Default;

    [Tooltip("The time variances between messages showing up in the feed, in seconds")]
    [SerializeField] FloatRange timeBetweenMessages;

    [Tooltip("The message display element to be used for showing a message")]
    [SerializeField] RectTransform messagePrefab;

    [Tooltip("The object where the messages will be appended to")]
    [SerializeField] RectTransform messageContainer;

    [Tooltip("Set to start the feed automatically when the object is enabled")]
    [SerializeField] bool startOnEnable = true;

    [SerializeField] RectTransform scrollContainer;

    [SerializeField] bool adjustHeightToPrefab = false;

    [SerializeField] int startingMessage = 0;

    ScrollRect scrollRect;
    Vector2 position = Vector2.zero;
    bool forceComplete;

    public void SendMessageToFeed(TextObject text)
    {
        SendMessageToFeed(text.text);
    }

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

    public void SendSenderMessageToFeed()
    {
        DisplayMessage(new Message()
        {
            message = SocialMediaScenarioPicker.Instance.CurrentScenario.GetText(SocialMediaScenarioTextType.Sender),
            profile = SocialMediaScenarioPicker.Instance.CurrentScenario.GetProfile(SocialMediaScenarioTextType.Sender)
        });
    }

    public void SendReplyMessageToFeed()
    {
        DisplayMessage(new Message()
        {
            message = SocialMediaScenarioPicker.Instance.CurrentScenario.GetText(SocialMediaScenarioTextType.Receiver),
            profile = SocialMediaScenarioPicker.Instance.CurrentScenario.GetProfile(SocialMediaScenarioTextType.Receiver)
        });

        ScrollToBottom();
    }

    public void SendFriendMessageToFeed()
    {
        DisplayMessage(new Message()
        {
            message = SocialMediaScenarioPicker.Instance.CurrentScenario.GetText(SocialMediaScenarioTextType.Friend),
            profile = SocialMediaScenarioPicker.Instance.CurrentScenario.GetProfile(SocialMediaScenarioTextType.Friend)
        });
    }

    public void SendFriendAndReplyMessage()
    {
        SendFriendMessageToFeed();

        DisplayMessage(new Message()
        {
            message = SocialMediaScenarioPicker.Instance.CurrentScenario.GetText(SocialMediaScenarioTextType.Receiver),
            profile = SocialMediaScenarioPicker.Instance.CurrentScenario.GetProfile(SocialMediaScenarioTextType.Receiver)
        });
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

        messagePrefab.gameObject.SetActive(false);
    }

    private void Start()
    {
        messageFeed = SocialMediaScenarioPicker.Instance.CurrentScenario.GetMessageFeed(MessageFeedType);
    }

    private void OnEnable()
    {
        if (adjustHeightToPrefab)
        {
            position.y = messagePrefab.anchoredPosition.y;
        }

        if (startOnEnable)
        {
            StartFeed();
        }
    }

    IEnumerator DisplayMessages()
    {
        while (!messageFeed)
        {
            yield return null;
        }

        int lastMessageShown = startingMessage;

        while (lastMessageShown < messageFeed.messages.Count || loop)
        {
            DisplayMessage(messageFeed.messages[lastMessageShown]);
            lastMessageShown++;

            if (!forceComplete)
            {
                yield return new WaitForSeconds(timeBetweenMessages.GetValue());
            }

            if (loop && messageFeed.messages.Count <= lastMessageShown)
            {
                lastMessageShown = 0;
            }
        }

        OnComplete?.Invoke();
    }

    public void DisplayMessage(Message theMessage, bool triggerEvent = true)
    {
        var messageDisplay = Instantiate(messagePrefab, messageContainer);

        var message = messageDisplay.GetComponent<ScreenMessage>();

        if (!message)
        {
            Debug.LogError("No message!");
            return;
        }

        message.message = theMessage.message;

        IncreaseHeightToFitText(message.MessageTextField, theMessage.message, messageDisplay, message.TextBackground);

        if (theMessage.profile?.picture != null)
        {
            message.profilePicture = theMessage.profile.picture;
        }

        if (!string.IsNullOrEmpty(theMessage.profile?.username))
        {
            message.from = theMessage.profile.username;
        }
        SetWidthBasedOnText(message.UsernameTextField, message.from, message.moveFromTime ? message.TagAndTimeTextField.rectTransform : null);

        if (!string.IsNullOrEmpty(theMessage.profile?.tag))
        {
            message.fromTag = theMessage.profile.tag;
        }

        message.image = theMessage.image;

        messageDisplay.gameObject.SetActive(true);

        messageDisplay.anchoredPosition = position;

        position.y -= messageDisplay.rect.height;
        messageContainer.sizeDelta = new Vector2(0, -position.y);

        if (scrollContainer)
        {
            scrollContainer.sizeDelta = messageContainer.sizeDelta;
        }

        if (scrollToBottom)
        {
            ScrollToBottom();
        }

        if (triggerEvent)
        {
            OnNewMessage?.Invoke();
        }
    }

    private float IncreaseHeightToFitText(Text textField, string newText, params RectTransform[] containers)
    {
        var currentTextHeight = textField.rectTransform.sizeDelta.y;
        var perferredHeight = textField.cachedTextGeneratorForLayout.GetPreferredHeight(newText, textField.GetGenerationSettings(textField.rectTransform.rect.size));

        if (perferredHeight > currentTextHeight)
        {
            var sizeDifference = new Vector2(0, perferredHeight);
            textField.rectTransform.sizeDelta += sizeDifference;

            foreach (var container in containers)
            {
                if (!container)
                    continue;

                container.sizeDelta += sizeDifference - (currentTextHeight / 2 * Vector2.up);
            }

            return perferredHeight;
        }

        return 0;
    }

    private void SetWidthBasedOnText(Text textField, string newText, params RectTransform[] toTheRightOf)
    {
        // Based on being top, centre pivot
        var perferredWidth = textField.cachedTextGeneratorForLayout.GetPreferredWidth(newText, textField.GetGenerationSettings(textField.rectTransform.sizeDelta));

        var difference = new Vector2(perferredWidth - textField.rectTransform.sizeDelta.x, 0);

        if (textField.rectTransform.anchorMin.x > 0)
        {
            textField.rectTransform.sizeDelta += difference;
        }

        textField.rectTransform.anchoredPosition += difference / 2;

        if (difference.x > 0)
        {
            //difference += difference / 2;
        }

        foreach (var control in toTheRightOf)
        {
            if (!control)
                return;

            control.anchoredPosition += difference;
        }
    }

    public void SendToBottom(RectTransform transform)
    {
        transform.gameObject.SetActive(true);
        transform.anchoredPosition = position;

        position.y -= transform.rect.height;
        messageContainer.sizeDelta = new Vector2(0, -position.y);

        if (scrollContainer)
        {
            scrollContainer.sizeDelta = messageContainer.sizeDelta;
        }

        if (scrollToBottom)
        {
            ScrollToBottom();
        }
    }

    public void ScrollToBottom()
    {
        if (scrollRect)
        {
            scrollRect.verticalNormalizedPosition = 0;
        }
    }

    public void FlashLastOne()
    {
        var last = transform.GetChild(transform.childCount - 1);
        var highlight = last.GetComponent<HighlightImage>();

        if (highlight)
        {
            highlight.enabled = true;
        }
    }

    public void Populate(int timeOffset, MessageFeed feed)
    {
        foreach (var message in feed.messages)
        {
            DisplayMessage(message, false);
        }
    }

    public void PopulateFirstMessage(RectTransform messageDisplay)
    {
        var message = messageDisplay.GetComponent<ScreenMessage>();


        message.enabled = false;

        var theMessage = MessageFeed.messages[0];

        message.message = theMessage.message;

        float height = IncreaseHeightToFitText(message.MessageTextField, theMessage.message, messageDisplay, message.TextBackground);

        if (theMessage.profile?.picture != null)
        {
            message.profilePicture = theMessage.profile.picture;
        }

        if (!string.IsNullOrEmpty(theMessage.profile?.username))
        {
            message.from = theMessage.profile.username;
        }
        SetWidthBasedOnText(message.UsernameTextField, message.from);

        if (!string.IsNullOrEmpty(theMessage.profile?.tag))
        {
            message.fromTag = theMessage.profile.tag;
        }

        message.image = theMessage.image;

        message.enabled = true;

        messagePrefab.anchoredPosition -= new Vector2(0, height);
    }

    public void HighlightFirstMessage()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var last = transform.GetChild(i);
            var highlight = last.GetComponent<HighlightImage>();

            if (highlight && last.gameObject.activeInHierarchy)
            {
                highlight.enabled = true;
                return;
            }
        }
    }
}