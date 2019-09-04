using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TextEnterer : MonoBehaviour
{
    [SerializeField] SocialMediaScenarioTextType textToEnterType;
    [SerializeField] FloatRange timeBetweenCharacters = new FloatRange() { min = 0.2f, max = 0.25f };
    [SerializeField] InputField input;
    [SerializeField] Button sendButton;
    [SerializeField] UnityEvent typingCompleted;
    [SerializeField] UnityEvent onSend;    
    [SerializeField] ScreenMessageFeedView feedView;

    bool started;
    string textToEnter;

    public void SendText()
    {
        feedView.StopFeed();
        feedView.SendMessageToFeed(input);
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

    private void Awake()
    {
        sendButton.interactable = false;

        if (!feedView)
        {
            feedView = GetComponent<ScreenMessageFeedView>();
        }
    }

    IEnumerator TypeText()
    {
        textToEnter = SocialMediaScenarioPicker.Instance.CurrentScenario.GetText(textToEnterType);

        for (int i = 0; i < textToEnter.Length; i++)
        {
            input.text = textToEnter.Substring(0, i + 1);
            EventManager.TriggerEvent(Events.KeyboardTextTyped);
            EventManager.TriggerEvent(Events.KeyboardTextTyped, new TextTypedEventArgs(input.text[input.text.Length - 1].ToString()));

            yield return new WaitForSeconds(timeBetweenCharacters.GetValue());
        }

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
}