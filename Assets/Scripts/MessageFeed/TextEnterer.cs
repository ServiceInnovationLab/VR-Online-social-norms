using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TextEnterer : MonoBehaviour
{
    [SerializeField] SocialMediaScenarioTextType textToEnterType;
    [SerializeField] protected FloatRange timeBetweenCharacters = new FloatRange() { min = 0.2f, max = 0.25f };
    [SerializeField] InputField input;
    [SerializeField] Button sendButton;
    [SerializeField] UnityEvent typingCompleted;
    [SerializeField] UnityEvent onSend;
    [SerializeField] ScreenMessageFeedView feedView;

    protected bool started;
    protected string textToEnter;
    protected bool isTypingCompleted { get; private set; }

    public void SendText()
    {
        if (feedView)
        {
            feedView.StopFeed();
            feedView.SendMessageToFeed(input);
        }

        onSend?.Invoke();
    }

    public void Complete()
    {
        StopAllCoroutines();
        input.text = textToEnter;
    }

    public void StartTypeText()
    {
        if (!started)
        {
            StartCoroutine(TypeText());
            started = true;
        }
    }

    protected virtual void Awake()
    {
        if (sendButton)
        {
            sendButton.interactable = false;
        }

        if (!feedView)
        {
            feedView = GetComponent<ScreenMessageFeedView>();
        }
    }

    protected virtual void Start()
    {
        textToEnter = SocialMediaScenarioPicker.Instance.CurrentScenario.GetText(textToEnterType);
    }

    protected virtual IEnumerator TypeText()
    {
        if (textToEnter == null)
        {
            textToEnter = SocialMediaScenarioPicker.Instance.CurrentScenario.GetText(textToEnterType);
        }

        for (int i = 0; i < textToEnter.Length; i++)
        {
            TypeCharacter(i + 1);

            yield return new WaitForSeconds(timeBetweenCharacters.GetValue());
        }

        OnTypingFinished();
    }

    protected virtual void OnTypingFinished()
    {
        isTypingCompleted = true;
        typingCompleted?.Invoke();

        if (sendButton)
        {
            sendButton.interactable = true;
        }

        if (feedView)
        {
            feedView.StopFeed();
        }
    }

    /// <summary>
    /// Types the text to be entered up to the length specified.
    /// </summary>
    /// <param name="length"></param>
    protected void TypeCharacter(int length)
    {
        length = Mathf.Min(length, textToEnter.Length);

        input.text = textToEnter.Substring(0, length);
        EventManager.TriggerEvent(Events.KeyboardTextTyped);
        EventManager.TriggerEvent(Events.KeyboardTextTyped, new TextTypedEventArgs(input.text[input.text.Length - 1].ToString()));
    }
}