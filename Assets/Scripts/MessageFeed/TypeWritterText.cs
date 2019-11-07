using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

[RequireComponent(typeof(Text))]
public class TypeWritterText : MonoBehaviour
{
    [SerializeField] UnityEvent typingCompleted;
    [SerializeField] FloatRange timeBetweenCharacters = new FloatRange() { min = 0.2f, max = 0.25f };

    Text textObject;

    string textToShow;

    private void Awake()
    {
        textObject = GetComponent<Text>();
        textToShow = textObject.text;

        textObject.text = "";
    }

    private void OnEnable()
    {
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        for (int i = 0; i < textToShow.Length; i++)
        {
            textObject.text = textToShow.Substring(0, i + 1);
            EventManager.TriggerEvent(Events.KeyboardTextTyped);
            EventManager.TriggerEvent(Events.KeyboardTextTyped, new TextTypedEventArgs(textObject.text[textObject.text.Length - 1].ToString()));

            yield return new WaitForSeconds(timeBetweenCharacters.GetValue());
        }

        typingCompleted?.Invoke();
    }

}
