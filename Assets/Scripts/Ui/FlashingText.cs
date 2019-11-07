using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class FlashingText : MonoBehaviour
{
    [SerializeField] Color flashColour;
    [SerializeField] float flashTime = 2.0f;

    Color originalColour;
    Text text;

    Color toColor;
    Color fromColor;
    float time;

    private void Awake()
    {
        text = GetComponent<Text>();
        originalColour = text.color;

        toColor = flashColour;
        fromColor = originalColour;
    }

    private void OnDisable()
    {
        text.color = originalColour;
    }

    void Update()
    {
        time += Time.deltaTime;

        if (time >= flashTime)
        {
            text.color = toColor;

            toColor = fromColor;
            fromColor = text.color;
            time = 0;
        }
        else
        {
            float t = time / flashTime;
            text.color = Color.Lerp(fromColor, toColor, t * t);
        }
    }
}
