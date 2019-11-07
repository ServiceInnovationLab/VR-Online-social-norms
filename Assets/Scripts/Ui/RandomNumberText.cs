using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class RandomNumberText : MonoBehaviour
{
    [SerializeField] int numberOfCharacters;
    [SerializeField] string prefix;

    Text text;

    private void Awake()
    {
        text = GetComponent<Text>();

        string newText = prefix;

        for (int i = 0; i < numberOfCharacters; i++)
        {
            newText += Random.Range(i > 0 ? 0 : 1, 9).ToString();
        }

        text.text = newText;
    }
}
