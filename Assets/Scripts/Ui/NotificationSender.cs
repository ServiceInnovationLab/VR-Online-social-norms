using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.UI;

public class NotificationSender : MonoBehaviour
{
    [SerializeField] UnityEvent notificationReceived;
    [SerializeField] UnityEvent notificationDismissed;

    [SerializeField] float animationTime = 1.0f;
    [SerializeField] Vector2 animationDirection = new Vector2(-1, 0);
    [SerializeField] RectTransform background;
    [SerializeField] Text fromText;
    [SerializeField] Text messageText;
    [SerializeField] Text applicationNameText;
    [SerializeField] Image applicationIcon;
    [SerializeField] Vector2 padding;
    [SerializeField] RectTransform sentTime;

    Vector2 startPosition;
    Vector2 endPosition;

    float startTime;
    int currentDirection = 0;

    int currentMessage;
   
    public bool IsNotificationShowing { get; private set; }

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

        SetPositionBasedOnText(fromText, from, sentTime);

        ResetToStart(true);

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

    private void SetPositionBasedOnText(Text textField, string newText, params RectTransform[] toTheRightOf)
    {
        var position = new Vector2(textField.cachedTextGeneratorForLayout.GetPreferredWidth(newText, textField.GetGenerationSettings(textField.rectTransform.sizeDelta)), 0);

        foreach (var control in toTheRightOf)
        {
            control.anchoredPosition = textField.rectTransform.anchoredPosition + position + padding;
        }
    }
}
