using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ScreenMessageFeedView : MonoBehaviour
{
    public float timeScaleAfterPause = 1.0f;

    public float afterPauseDelay = 2.0f;

    public Color lastItemHighlight;
    public bool highlightLastItem = false;

    public bool scrollToBottom = true;

    public bool stopScrollingAfterHighlighed = false;

    MessageFeed messageFeed;

    public float headerHeight = 0;

    private MessageFeed MessageFeed
    {
        get
        {
            if (messageFeed == null)
                messageFeed = SocialMediaScenarioPicker.Instance.CurrentScenario.GetMessageFeed(MessageFeedType);

            return messageFeed;
        }
    }

    [SerializeField] UnityEvent OnEnabled = new UnityEvent();

    [SerializeField] UnityEvent OnComplete = new UnityEvent();

    [SerializeField] UnityEvent OnNewMessage = new UnityEvent();

    [SerializeField] bool showAll = false;

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

    [SerializeField] bool randomiseOrder = false;

    [SerializeField] int enableDelay = 0;

    [SerializeField] bool skipResizeIfEnoughSpace = false;

    [SerializeField] UnityEvent onPaused;

    public bool IsDone { get; private set; }

    ScrollRect scrollRect;
    Vector2 position = Vector2.zero;
    bool forceComplete;
    ScrollRectDetector detector;

    int[] messageOrder;
    bool needsToWaitAFrame;

    bool paused;

    float timeScale = 1.0f;

    public void Continue()
    {
        paused = false;
    }

    public Vector2 GetPosition()
    {
        return position;
    }

    public void SendMessageToFeed(TextObject text)
    {
        SendMessageToFeed(text.text);
    }

    public void SendMessageToFeed(string text)
    {
        if (!string.IsNullOrWhiteSpace(text))
        {
            StartCoroutine(DisplayMessage(new Message() { message = text }));
        }
    }

    public void SendMessageToFeed(InputField input)
    {
        SendMessageToFeed(input.text);
        input.text = "";
    }

    public void SendSenderMessageToFeed()
    {
        StartCoroutine(DisplayMessage(new Message()
        {
            message = SocialMediaScenarioPicker.Instance.CurrentScenario.GetText(SocialMediaScenarioTextType.Sender),
            profile = SocialMediaScenarioPicker.Instance.CurrentScenario.GetProfile(SocialMediaScenarioTextType.Sender),
            highlight = true
        }));
    }

    public void SendReplyMessageToFeed()
    {
        scrollToBottom = true;
        StartCoroutine(DisplayMessage(new Message()
        {
            message = SocialMediaScenarioPicker.Instance.CurrentScenario.GetText(SocialMediaScenarioTextType.Receiver),
            profile = SocialMediaScenarioPicker.Instance.CurrentScenario.GetProfile(SocialMediaScenarioTextType.Receiver)
        }));

        ScrollToBottom();
    }

    public void SendFriendMessageToFeed()
    {
        StartCoroutine(
            DisplayMessage(new Message()
            {
                message = SocialMediaScenarioPicker.Instance.CurrentScenario.GetText(SocialMediaScenarioTextType.Friend),
                profile = SocialMediaScenarioPicker.Instance.CurrentScenario.GetProfile(SocialMediaScenarioTextType.Friend),
                flash = true
            },
            true,
            SocialMediaScenarioPicker.Instance.CurrentScenario.receiverMessageFeed.messages[0]
            )
        );
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
        if (scrollRect)
        {
            detector = scrollRect.GetComponent<ScrollRectDetector>();
        }

        messagePrefab.gameObject.SetActive(false);
        needsToWaitAFrame = messagePrefab.GetComponent<ScreenMessage>().NeedsAFrame;
    }

    private void Start()
    {
        messageFeed = SocialMediaScenarioPicker.Instance.CurrentScenario.GetMessageFeed(MessageFeedType);
    }

    private void OnEnable()
    {
        Invoke(nameof(RaiseOnEnable), 0.02f);

        if (adjustHeightToPrefab)
        {
            position.y = messagePrefab.anchoredPosition.y;
        }

        if (startOnEnable)
        {
            if (scrollRect)
            {
                scrollRect.verticalNormalizedPosition = 1;
            }
            StartFeed();
        }
    }

    void RaiseOnEnable()
    {
        OnEnabled?.Invoke();
    }


    IEnumerator DisplayMessages()
    {
        while (!messageFeed)
        {
            yield return null;
        }

        if (randomiseOrder)
        {
            messageOrder = new int[messageFeed.messages.Count];
            for (int i = 0; i < messageOrder.Length; i++)
            {
                messageOrder[i] = i;
            }
            RandomiseItems();
        }

        int lastMessageShown = startingMessage;

        bool stopScrolling = false;

        while (lastMessageShown < messageFeed.messages.Count || loop)
        {
            int index = lastMessageShown;

            if (randomiseOrder)
            {
                index = messageOrder[index];
            }

            yield return DisplayMessage(messageFeed.messages[index]);
            lastMessageShown++;

            if (stopScrolling)
            {
                scrollToBottom = false;
                stopScrolling = false;
                ScrollToHighlightedMessage();
            }

            if (stopScrollingAfterHighlighed && messageFeed.messages[index].highlight)
            {
                stopScrolling = true;
            }

            if (messageFeed.messages[index].pauseHere)
            {
                paused = true;
                //forceComplete = false;
                onPaused?.Invoke();
                timeScale = timeScaleAfterPause;

                yield return new WaitUntil(() => !paused);
                yield return new WaitForSeconds(afterPauseDelay);
            }

            if (highlightLastItem && lastMessageShown == messageFeed.messages.Count)
            {
                HighlightLastMessage();
            }

            float timeToWait = timeBetweenMessages.GetValue() * timeScale / 10;

            // Split it up into groups of 10, in case force complete is set so it doesn't have to wait the whole time
            for (int i = 0; i < 10; i++)
            {
                if (!forceComplete)
                {
                    yield return new WaitForSeconds(timeToWait);
                }
            }

            if (loop && messageFeed.messages.Count <= lastMessageShown)
            {
                lastMessageShown = 0;

                if (randomiseOrder)
                {
                    RandomiseItems();
                }
            }
        }

        IsDone = true;
        OnComplete?.Invoke();
    }

    private void RandomiseItems()
    {
        for (var i = 0; i < messageOrder.Length - 1; i++)
        {
            var temp = messageOrder[i];
            int newIndex = Random.Range(i, messageOrder.Length);

            messageOrder[i] = messageOrder[newIndex];
            messageOrder[newIndex] = temp;
        }
    }

    public IEnumerator DisplayMessage(Message theMessage, bool triggerEvent = true, Message subMessage = null)
    {
        var messageDisplay = Instantiate(messagePrefab, messageContainer);

        var message = messageDisplay.GetComponent<ScreenMessage>();

        if (!message)
        {
            Debug.LogError("No message!");
            yield break;
        }

        message.message = theMessage.message;
        message.highlight = theMessage.highlight;
        message.flash = theMessage.flash;
        message.animatedImage = theMessage.animatedImage;
        message.retweetedBy = theMessage.retweetedBy;

        if (theMessage.senderSubMessage)
        {
            subMessage = new Message()
            {
                message = SocialMediaScenarioPicker.Instance.CurrentScenario.GetText(SocialMediaScenarioTextType.Receiver),
                profile = SocialMediaScenarioPicker.Instance.CurrentScenario.GetProfile(SocialMediaScenarioTextType.Receiver),
            };
        }

        if (!string.IsNullOrEmpty(theMessage.profile?.tag))
        {
            message.fromTag = theMessage.profile.tag;
        }

        message.image = theMessage.image;

        if (theMessage.profile?.picture != null)
        {
            message.profilePicture = theMessage.profile.picture;
        }

        if (!string.IsNullOrEmpty(theMessage.profile?.username))
        {
            message.from = theMessage.profile.username;
        }

        messageDisplay.gameObject.SetActive(true);

        if (needsToWaitAFrame)
        {
            yield return new WaitForEndOfFrame();
        }

        if (message.MessageTextField)
        {
            IncreaseHeightToFitText(messageDisplay, message.MessageTextField, theMessage.message, messageDisplay, message.TextBackground);
        }
        else if (message.MessageTextFieldPro)
        {
            IncreaseHeightToFitText(message.MessageTextFieldPro, theMessage.message, messageDisplay, message.TextBackground);
        }


        SetWidthBasedOnText(message.UsernameTextField, message.from, message.moveFromTime ? message.TagAndTimeTextField : null);


        // Sub Message
        if (subMessage != null && message.subMessage)
        {
            message.subMessage.message = subMessage.message;
            message.subMessage.profilePicture = subMessage.profile.picture;
            message.subMessage.message = subMessage.message;
            message.subMessage.from = subMessage.profile.username;
            message.subMessage.fromTag = subMessage.profile.tag;

            RectTransform subMessageRect = message.subMessage.transform as RectTransform;
            messageDisplay.sizeDelta += subMessageRect.sizeDelta.Y(+20);

            if (message.subMessage.MessageTextField)
            {
                IncreaseHeightToFitText(messageDisplay, message.subMessage.MessageTextField, subMessage.message, subMessageRect, message.TextBackground, messageDisplay);
            }
            else if (message.subMessage.MessageTextFieldPro)
            {
                IncreaseHeightToFitText(message.subMessage.MessageTextFieldPro, subMessage.message, subMessageRect, message.subMessage.TextBackground, messageDisplay);
            }

            SetWidthBasedOnText(message.subMessage.UsernameTextField, message.subMessage.from, message.subMessage.moveFromTime ? message.subMessage.TagAndTimeTextField : null);

            subMessageRect.gameObject.SetActive(true);
        }

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

    private float IncreaseHeightToFitText(RectTransform screenMessage, Text textField, string newText, params RectTransform[] containers)
    {
        var currentTextHeight = textField.rectTransform.sizeDelta.y;
        var perferredHeight = textField.cachedTextGeneratorForLayout.GetPreferredHeight(newText, textField.GetGenerationSettings(textField.rectTransform.rect.size));

        if (perferredHeight > currentTextHeight)
        {
            var sizeDifference = new Vector2(0, perferredHeight);

            bool needsToResizeContainer = !skipResizeIfEnoughSpace || textField.rectTransform.sizeDelta.y + sizeDifference.y + Mathf.Abs(textField.rectTransform.anchoredPosition.y) * 1.5f >= screenMessage.rect.height;

            textField.rectTransform.sizeDelta += sizeDifference;

            foreach (var container in containers)
            {
                if (!container)
                    continue;

                if (!needsToResizeContainer && container == screenMessage)
                {
                    perferredHeight = 0;
                    continue;
                }

                container.sizeDelta += sizeDifference - (currentTextHeight / 2 * Vector2.up);
            }

            return perferredHeight;
        }

        return 0;
    }

    private float IncreaseHeightToFitText(TextMeshProUGUI textField, string newText, params RectTransform[] containers)
    {
        var currentTextHeight = textField.rectTransform.sizeDelta.y;
        var perferredHeight = textField.GetPreferredValues(newText, textField.rectTransform.rect.width, 0).y;

        if (perferredHeight > currentTextHeight)
        {
            var sizeDifference = new Vector2(0, perferredHeight - currentTextHeight);
            textField.rectTransform.sizeDelta += sizeDifference;

            foreach (var container in containers)
            {
                if (!container)
                    continue;

                container.sizeDelta += sizeDifference;
            }

            return perferredHeight;
        }

        return 0;
    }

    private void SetWidthBasedOnText(Text textField, string newText, params RectTransform[] toTheRightOf)
    {
        if (!textField)
            return;

        // Based on being top, centre pivot
        var perferredWidth = textField.cachedTextGeneratorForLayout.GetPreferredWidth(newText, textField.GetGenerationSettings(textField.rectTransform.sizeDelta));

        var difference = new Vector2(perferredWidth - textField.rectTransform.sizeDelta.x, 0);

        if (textField.rectTransform.anchorMin.x > 0)
        {
            textField.rectTransform.sizeDelta += difference;
        }

        textField.rectTransform.anchoredPosition += difference / 2;

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

        if (detector)
        {
            detector.Restart();
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
        StartCoroutine(DoPopulate(feed));
    }

    private IEnumerator DoPopulate(MessageFeed feed)
    {
        foreach (var message in feed.messages)
        {
            yield return DisplayMessage(message, false);
        }
    }

    public void PopulateFirstMessage(RectTransform messageDisplay)
    {
        var message = messageDisplay.GetComponent<ScreenMessage>();


        message.enabled = false;

        var theMessage = MessageFeed.messages[0];

        message.message = theMessage.message;

        float height = 0;

        if (message.MessageTextField)
        {
            height = IncreaseHeightToFitText(messageDisplay, message.MessageTextField, theMessage.message, messageDisplay, message.TextBackground);
        }
        else if (message.MessageTextFieldPro)
        {
            height = IncreaseHeightToFitText(message.MessageTextFieldPro, theMessage.message, messageDisplay, message.TextBackground);
        }

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

        if (showAll)
        {
            if (adjustHeightToPrefab)
            {
                position.y = messagePrefab.anchoredPosition.y;
            }

            forceComplete = true;
            StartFeed();
        }
        else
        {
            OnEnabled.AddListener(() =>
            {
                messageContainer.sizeDelta = new Vector2(0, -position.y);

                if (scrollContainer)
                {
                    scrollContainer.sizeDelta = messageContainer.sizeDelta;
                }

                if (scrollRect)
                {
                    scrollRect.verticalNormalizedPosition = 1;
                }
            });
        }
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

    public void HighlightLastMessage()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            var last = transform.GetChild(i);
            var highlight = last.GetComponent<HighlightImage>();

            if (highlight && last.gameObject.activeInHierarchy)
            {
                highlight.enabled = false;
                highlight.GetComponent<Image>().color = lastItemHighlight;
                return;
            }
        }
    }

    public void ScrollToHighlightedMessage()
    {
        for (int i = 0; i < messageContainer.transform.childCount; i++)
        {
            var child = messageContainer.transform.GetChild(i);
            var message = child.GetComponent<ScreenMessage>();

            if (message && message.highlight)
            {
                Canvas.ForceUpdateCanvases();
                Vector2 viewportLocalPosition = scrollRect.viewport.localPosition;
                Vector2 childLocalPosition = child.localPosition;
                Vector2 result = new Vector2(
                    0 - (viewportLocalPosition.x + childLocalPosition.x),
                    0 - (viewportLocalPosition.y + childLocalPosition.y) + 150
                );

                scrollRect.content.localPosition = result;

                if (detector)
                {
                    detector.Restart();
                }

                break;
            }
        }
    }

    public void ScrollToFirstUnpausedMessage()
    {
        int index = 0;

        foreach (var message in MessageFeed.messages)
        {
            index++;
            if (message.pauseHere)
            {
                break;
            }
        }

        int z = 0;

        for (int i = 0; i < messageContainer.transform.childCount; i++)
        {
            var child1 = messageContainer.transform.GetChild(i);
            var message = child1.GetComponent<ScreenMessage>();

            if (message && message.gameObject.activeInHierarchy)
            {
                if (z++ == index)
                {
                    index = i;
                    break;
                }
            }
        }

        var child = messageContainer.transform.GetChild(index);
        var screenMessage = child.GetComponent<ScreenMessage>();

        var rectChild = (RectTransform)child;

        if (screenMessage)
        {
            Canvas.ForceUpdateCanvases();
            Vector2 viewportLocalPosition = scrollRect.viewport.localPosition;
            Vector2 childLocalPosition = child.localPosition;
            Vector2 result = new Vector2(
                0 - (viewportLocalPosition.x + childLocalPosition.x),
                0 - (viewportLocalPosition.y + childLocalPosition.y) + (((RectTransform)scrollRect.transform).rect.height - rectChild.rect.height) - headerHeight
            );

            //scrollRect.content.localPosition = result;
            StartCoroutine(SmoothScroll(result));

            if (detector)
            {
                detector.Restart();
            }

            FindObjectOfType<StartAfterScrolled>().SecondGo();
        }
    }

    IEnumerator SmoothScroll(Vector2 toPos)
    {
        Vector2 fromPos = scrollRect.content.localPosition;

        float time = Time.time;

        while (true)
        {
            yield return new WaitForEndOfFrame();

            float diff = (Time.time - time) / 1;

            if (diff > 1)
            {
                scrollRect.content.localPosition = toPos;
                break;
            }

            scrollRect.content.localPosition = Vector2.Lerp(fromPos, toPos, diff);
        }
    }

}