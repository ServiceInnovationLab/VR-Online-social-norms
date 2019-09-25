using UnityEngine;
using UnityEngine.UI;

public class TriggerTwitterMessage : MonoBehaviour
{
    [SerializeField] NotificationSender notificationSender;
    [SerializeField] float delay = 5;

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
        notificationSender.ShowNotification("Twitter: From Bashiir's friend", "Can you belive this?", 0);
        notificationSender.GetComponent<Button>().enabled = true;
    }

}
