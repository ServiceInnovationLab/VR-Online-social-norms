using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TextEnterer : MonoBehaviour
{
    [SerializeField] string textToEnter;
    [SerializeField] float timeBetweenCharacters = 0.2f;
    [SerializeField] InputField input;

    public void Complete()
    {
        StopAllCoroutines();
        input.text = textToEnter;
    }

    private void Start()
    {
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        for (int i = 0; i < textToEnter.Length; i++)
        {
            input.text = textToEnter.Substring(0, i + 1);
            yield return new WaitForSeconds(timeBetweenCharacters);
        }
    }
}