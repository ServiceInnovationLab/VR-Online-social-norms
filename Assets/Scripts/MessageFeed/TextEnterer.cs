using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenMessageFeedView))]
public class TextEnterer : MonoBehaviour
{
    [SerializeField] TextObject textToEnter;
    [SerializeField] FloatRange timeBetweenCharacters = new FloatRange() { min = 0.2f, max = 0.25f };
    [SerializeField] InputField input;
    [SerializeField] Button sendButton;
    [SerializeField] UnityEvent typingCompleted;
    [SerializeField] UnityEvent onSend;

    bool started;
    ScreenMessageFeedView feedView;

    public void SendText()
    {
        feedView.StopFeed();
        feedView.SendMessageToFeed(input);
        onSend?.Invoke();
    }

    public void Complete()
    {
        StopAllCoroutines();
        input.text = textToEnter.text;
    }

    public void StartTypeText()
    {
        if (!started)
        {
            StartCoroutine(TypeText());
            started = true;
        }
    }

    private void Awake()
    {
        sendButton.interactable = false;
        feedView = GetComponent<ScreenMessageFeedView>();
    }

    IEnumerator TypeText()
    {
        for (int i = 0; i < textToEnter.text.Length; i++)
        {
            input.text = textToEnter.text.Substring(0, i + 1);
            EventManager.TriggerEvent(Events.KeyboardTextTyped);
            EventManager.TriggerEvent(Events.KeyboardTextTyped, new TextTypedEventArgs(input.text[input.text.Length - 1].ToString()));

            yield return new WaitForSeconds(timeBetweenCharacters.GetValue());
        }

        typingCompleted?.Invoke();

        if (sendButton)
        {
            sendButton.interactable = true;
        }

        feedView.StopFeed();
    }
}