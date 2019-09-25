using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.UI;

public class NotificationSender : MonoBehaviour
{
    [SerializeField] UnityEvent notificationReceived;

    [SerializeField] float animationTime = 1.0f;
    [SerializeField] Vector2 animationDirection = new Vector2(-1, 0);
    [SerializeField] RectTransform background;
    [SerializeField] Text fromText;
    [SerializeField] Text messageText;

    Vector2 startPosition;
    Vector2 endPosition;

    float startTime;
    int currentDirection = 0;

    private void Awake()
    {
        endPosition = background.anchoredPosition;
        startPosition = endPosition + Vector2.Scale(-animationDirection, background.sizeDelta);

        ResetToStart();
    }

    void ResetToStart(bool active = false)
    {
        background.gameObject.SetActive(active);
        background.anchoredPosition = startPosition;
    }

    void Update()
    {
        if (currentDirection == 0)
            return;

        float animationProgress = (Time.time - startTime) / animationTime;

        // Animating in
        if (currentDirection > 0)
        {
            background.anchoredPosition = Vector2.Lerp(startPosition, endPosition, animationProgress);
        }
        // Animating out
        else if (currentDirection < 0)
        {
            background.anchoredPosition = Vector2.Lerp(endPosition, startPosition, animationProgress);
        }

        if (animationProgress >= 1)
        {
            currentDirection = 0;
        }
    }

    /// <summary>
    /// Animates the current notification going away
    /// </summary>
    public void DismissNotification()
    {
        currentDirection = -1;
        startTime = Time.time;
    }

    /// <summary>
    /// Shows a notification 
    /// </summary>
    /// <param name="from"></param>
    /// <param name="message"></param>
    /// <param name="duration"></param>
    public void ShowNotification(string from, string message, float duration)
    {
        currentDirection = 1;
        startTime = Time.time;

        fromText.text = from;
        messageText.text = message;
        ResetToStart(true);

        if (duration > 0)
        {
            Invoke(nameof(DismissNotification), duration + animationTime);
        }

        notificationReceived?.Invoke();
    }
}
