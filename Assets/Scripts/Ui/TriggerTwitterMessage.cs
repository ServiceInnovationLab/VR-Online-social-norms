using UnityEngine;
using UnityEngine.UI;

public class TriggerTwitterMessage : MonoBehaviour
{
    [SerializeField] NotificationSender notificationSender;
    [SerializeField] float delay = 5;
    [SerializeField] Sprite twitterIcon;

    bool triggered = false;

    public void Trigger()
    {
        if (triggered)
            return;

        triggered = true;

        Invoke(nameof(DoTrigger), delay);
    }

    void DoTrigger()
    {
        notificationSender.ShowNotification("Twitter", "New message", "Tap to go to Twitter", 0, twitterIcon);
        GetComponentInParent<PeriodicHapticFeedback>().StartFeedback();
        notificationSender.GetComponent<Button>().enabled = true;
    }

}
