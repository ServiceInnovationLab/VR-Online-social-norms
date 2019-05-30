using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TextEnterer : MonoBehaviour
{
    [SerializeField] string textToEnter;
    [SerializeField] float timeBetweenCharacters = 0.2f;
    [SerializeField] InputField input;
    [SerializeField] Button sendButton;
    [SerializeField] UnityEvent typingCompleted;

    public void Complete()
    {
        StopAllCoroutines();
        input.text = textToEnter;
    }

    public void StartTypeText()
    {
        StartCoroutine(TypeText());
    }

    private void Awake()
    {
        sendButton.interactable = false;
    }

    IEnumerator TypeText()
    {
        for (int i = 0; i < textToEnter.Length; i++)
        {
            input.text = textToEnter.Substring(0, i + 1);
            yield return new WaitForSeconds(timeBetweenCharacters);
        }

        typingCompleted?.Invoke();

        sendButton.interactable = true;
    }
}