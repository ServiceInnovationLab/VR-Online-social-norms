using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HighlightImage : MonoBehaviour
{
    [SerializeField] Color colour;
    [SerializeField] float fadeoutTime = 1.0f;
    [SerializeField] float initalDelay = 0.2f;

    Color originalColour;
    Image image;


    float time;

    private void Awake()
    {
        image = GetComponent<Image>();
        originalColour = image.color;
    }

    private void OnEnable()
    {
        time = -initalDelay;
        image.color = colour;
    }


    void Update()
    {
        time += Time.deltaTime;

        if (time >= fadeoutTime)
        {
            image.color = originalColour;
            enabled = false;
        }
        else
        {
            image.color = Color.Lerp(colour, originalColour, time / fadeoutTime);
        }
    }
}
