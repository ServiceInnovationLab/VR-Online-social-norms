using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[RequireComponent(typeof(NotificationSender))]
public class SmsNotificationFeed : MonoBehaviour
{
    [SerializeField] UnityEvent onComplete;
    [SerializeField] Sprite appIcon;
    [SerializeField] float timeOnScreen;
    [SerializeField] FloatRange timeBetweenMessages;

    NotificationSender notificationSender;

    private void Awake()
    {
        notificationSender = GetComponent<NotificationSender>();
    }

    public void StartMessages()
    {
        StartCoroutine(DoShowMessages());
    }

    IEnumerator DoShowMessages()
    {
        var messageFeed = SocialMediaScenarioPicker.Instance.CurrentScenario.smsMessageFeed;

        foreach (var message in messageFeed.messages)
        {
            yield return new WaitForSeconds(timeBetweenMessages.GetValue());

            notificationSender.ShowNotification("Messages", message.profile.username, message.message, timeOnScreen, appIcon);

            yield return new WaitForSeconds(timeOnScreen);
        }

        onComplete?.Invoke();
    }
}
