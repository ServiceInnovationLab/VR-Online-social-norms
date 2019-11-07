using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class NotificationSender : MonoBehaviour
{
    [SerializeField] UnityEvent notificationReceived;
    [SerializeField] UnityEvent notificationDismissed;

    [SerializeField] float animationTime = 1.0f;
    [SerializeField] Vector2 animationDirection = new Vector2(-1, 0);
    [SerializeField] RectTransform background;
    [SerializeField] TextMeshProUGUI fromText;
    [SerializeField] TextMeshProUGUI messageText;
    [SerializeField] TextMeshProUGUI applicationNameText;
    [SerializeField] Image applicationIcon;
    [SerializeField] Vector2 padding;
    [SerializeField] RectTransform sentTime;

    Vector2 startPosition;
    Vector2 endPosition;

    float startTime;
    int currentDirection = 0;

    int currentMessage;

    Vector2 initialSize;
    Vector2 initalTextSize;
    Vector2 initialstartPosition;
    RectTransform rectTansform;

    public bool IsNotificationShowing { get; private set; }

    private void Awake()
    {
        endPosition = background.anchoredPosition;
        initialstartPosition = endPosition + Vector2.Scale(-animationDirection, background.sizeDelta);

        rectTansform = (RectTransform)transform;

        initialSize = rectTansform.sizeDelta;
        initalTextSize = messageText.rectTransform.sizeDelta;

        ResetToStart();
    }

    void ResetToStart(bool active = false)
    {
        background.gameObject.SetActive(active);
        background.anchoredPosition = startPosition;
        startPosition = initialstartPosition;

        rectTansform.sizeDelta = initialSize;
        background.sizeDelta = initialSize;
        messageText.rectTransform.sizeDelta = initalTextSize;
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

            if (!IsNotificationShowing)
            {
                ResetToStart();
            }
        }
    }

    /// <summary>
    /// Animates the current notification going away
    /// </summary>
    public void DismissNotification()
    {
        if (!IsNotificationShowing)
            return;

        currentMessage++;
        currentDirection = -1;
        startTime = Time.time;

        IsNotificationShowing = false;

        notificationDismissed?.Invoke();
    }

    /// <summary>
    /// Shows a notification 
    /// </summary>
    /// <param name="from"></param>
    /// <param name="message"></param>
    /// <param name="duration"></param>
    public void ShowNotification(string appName, string from, string message, float duration, Sprite icon = null)
    {
        IsNotificationShowing = true;
        currentMessage++;
        currentDirection = 1;
        startTime = Time.time;

        fromText.text = from;
        messageText.text = message;
        applicationIcon.sprite = icon;
        applicationNameText.text = appName.ToUpper();

        ResetToStart(true);

        SetPositionBasedOnText(fromText, from, sentTime);
        FitMessage(messageText, message, background, rectTansform);

        if (duration > 0)
        {
            StartCoroutine(DismissNotificationAfterTime(currentMessage, duration));
        }

        notificationReceived?.Invoke();
    }

    IEnumerator DismissNotificationAfterTime(int message, float duration)
    {
        yield return new WaitForSeconds(duration + animationTime);

        if (currentMessage == message)
        {
            DismissNotification();
        }
    }

    private void SetPositionBasedOnText(TextMeshProUGUI textField, string newText, params RectTransform[] toTheRightOf)
    {
        var position = textField.GetPreferredValues(newText, textField.rectTransform.sizeDelta.x, textField.rectTransform.sizeDelta.y);

        foreach (var control in toTheRightOf)
        {
            control.anchoredPosition = textField.rectTransform.anchoredPosition + position.X() + padding;
        }
    }

    private void FitMessage(TextMeshProUGUI textField, string newText, params RectTransform[] alsoIncrease)
    {
        var newHeight = textField.GetPreferredValues(newText, textField.rectTransform.sizeDelta.x, 0).y;

        if (newHeight > textField.rectTransform.sizeDelta.y)
        {
            var sizeDifference = new Vector2(0, newHeight - textField.rectTransform.sizeDelta.y);

            textField.rectTransform.sizeDelta += sizeDifference;

            foreach (var rect in alsoIncrease)
            {
                rect.sizeDelta += sizeDifference;
            }

            startPosition += Vector2.Scale(-animationDirection, sizeDifference);
        }
    }
}
