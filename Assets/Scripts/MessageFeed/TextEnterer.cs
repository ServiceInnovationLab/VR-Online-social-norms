using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenMessageFeedView))]
public class TextEnterer : MonoBehaviour
{
    [SerializeField] TextObject textToEnter;
    [SerializeField] float timeBetweenCharacters = 0.2f;
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
            yield return new WaitForSeconds(timeBetweenCharacters);
        }

        typingCompleted?.Invoke();

        if (sendButton)
        {
            sendButton.interactable = true;
        }

        feedView.StopFeed();
    }
}