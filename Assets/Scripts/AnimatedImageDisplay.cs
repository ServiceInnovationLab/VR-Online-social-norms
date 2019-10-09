using UnityEngine;
using UnityEngine.UI;

public class AnimatedImageDisplay : MonoBehaviour
{
    public AnimatedImage animatedImage;

    [SerializeField] Image image;

    int currentIndex = 0;

    float time;

    private void OnEnable()
    {
        time = 0;

        if (!image || !animatedImage)
        {
            enabled = false;
        }
    }

    void Update()
    {
        time += Time.deltaTime;

        if (time >= animatedImage.frameTime)
        {
            time -= animatedImage.frameTime;

            currentIndex = (currentIndex + 1) % animatedImage.images.Length;

            image.sprite = animatedImage.images[currentIndex];
        }
    }
}
