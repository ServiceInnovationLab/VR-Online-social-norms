using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[RequireComponent(typeof(NotificationSender))]
public class SmsNotificationFeed : MonoBehaviour
{
    [SerializeField] SocialMediaScenarioSMStype smsFeed = SocialMediaScenarioSMStype.Initial;
    [SerializeField] UnityEvent onComplete;    
    [SerializeField] float timeOnScreen;
    [SerializeField] FloatRange timeBetweenMessages;
    [SerializeField] Sprite[] appIcons;
    [SerializeField] float initialDelay;
    [SerializeField] LocalisedText[] appNames ;

    NotificationSender notificationSender;
    bool started = false;

    private void Awake()
    {
        notificationSender = GetComponent<NotificationSender>();

        if (appIcons.Length != appNames.Length)
        {
            Debug.LogError("App icon count doesn't match up with the app names");
        }
    }

    public void StartMessages()
    {
        if (started)
            return;
        
        started = true;
        StartCoroutine(DoShowMessages());
    }

    IEnumerator DoShowMessages()
    {
        var messageFeed = SocialMediaScenarioPicker.Instance.CurrentScenario.GetSMSMessageFeed(smsFeed);

        yield return new WaitForSeconds(initialDelay);

        foreach (var message in messageFeed.messages)
        {
            yield return new WaitForSeconds(timeBetweenMessages.GetValue());

            var iconIndex = 0;
            var messageText = message.message;

            if (messageText.StartsWith("["))
            {
                int.TryParse(message.message.Substring(1, 1), out iconIndex);

                messageText = messageText.Substring(3);
            }

            iconIndex = Mathf.Clamp(iconIndex, 0, appIcons.Length);

            notificationSender.ShowNotification(appNames[iconIndex].CurrentValue, message.profile.username, messageText, timeOnScreen, appIcons[iconIndex]);

            yield return new WaitWhile(() => notificationSender.IsNotificationShowing);
        }

        onComplete?.Invoke();
    }
}
