using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class FlashingButton : MonoBehaviour
{
    [SerializeField] Color flashColour;
    [SerializeField] float flashTime = 2.0f;

    Color originalColour;
    Button button;

    Color toColor;
    Color fromColor;
    float animateTime;
    float time;

    private void Awake()
    {
        button = GetComponent<Button>();
        originalColour = button.colors.normalColor;

        animateTime = flashTime / 4.0f;

        toColor = flashColour;
        fromColor = originalColour;
    }

    void Update()
    {
        time += Time.deltaTime;

        if (time >= flashTime)
        {
            var colours = button.colors;
            colours.normalColor = toColor;
            button.colors = colours;

            toColor = fromColor;
            fromColor = colours.normalColor;
            time = 0;
        }
        else
        {
            float remaningTime = time - flashTime;
            remaningTime -= animateTime * 3;

            if (remaningTime >= 0)
            {
                var colours = button.colors;
                colours.normalColor = Color.Lerp(fromColor, toColor, remaningTime / animateTime);
                button.colors = colours;
            }
        }
    }
}