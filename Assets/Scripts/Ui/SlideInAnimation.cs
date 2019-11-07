using UnityEngine;

public class SlideInAnimation : MonoBehaviour
{
    [SerializeField] float animationTime = 1.0f;
    [SerializeField] Vector2 animationDirection = new Vector2(-1, 0);
    [SerializeField] bool disableOnSlideOut;

    Vector2 startPosition;
    Vector2 endPosition;

    float startTime;
    int currentDirection = 0;
    RectTransform rectTansform;

    private void Awake()
    {
        rectTansform = (RectTransform)transform;

        endPosition = rectTansform.anchoredPosition;
        startPosition = endPosition + Vector2.Scale(-animationDirection, rectTansform.sizeDelta);

        ResetToStart();
    }

    void ResetToStart(bool active = false)
    {
        //rectTansform.gameObject.SetActive(active);
        if (rectTansform)
        {
            rectTansform.anchoredPosition = startPosition;
        }
    }

    public void SlideIn()
    {
        currentDirection = 1;
        startTime = Time.time;

        ResetToStart(true);
    }

    public void SlideOut()
    {
        currentDirection = -1;
        startTime = Time.time;
    }

    void Update()
    {
        if (currentDirection == 0)
            return;

        float animationProgress = (Time.time - startTime) / animationTime;

        // Animating in
        if (currentDirection > 0)
        {
            rectTansform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, animationProgress);
        }
        // Animating out
        else if (currentDirection < 0)
        {
            rectTansform.anchoredPosition = Vector2.Lerp(endPosition, startPosition, animationProgress);
        }

        if (animationProgress >= 1)
        {
            if (currentDirection < 0 && disableOnSlideOut)
            {
                gameObject.SetActive(false);
            }
            currentDirection = 0;
        }
    }
}
