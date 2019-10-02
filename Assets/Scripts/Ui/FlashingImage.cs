using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FlashingImage : MonoBehaviour
{
    [SerializeField] Color flashColour;
    [SerializeField] float flashTime = 2.0f;

    Color originalColour;
    Image image;

    Color toColor;
    Color fromColor;
    float animateTime;
    float time;

    private void Awake()
    {
        image = GetComponent<Image>();
        originalColour = image.color;

        animateTime = flashTime / 4.0f;

        toColor = flashColour;
        fromColor = originalColour;
    }

    private void OnDisable()
    {
        image.color = originalColour;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time >= flashTime)
        {
            image.color = toColor;

            toColor = fromColor;
            fromColor = image.color;
            time = 0;
        }
        else
        {
            float remaningTime = time - flashTime;
            image.color = Color.Lerp(fromColor, toColor, remaningTime / animateTime);
        }
    }
}
