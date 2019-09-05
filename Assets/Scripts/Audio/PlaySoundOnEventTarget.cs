using UnityEngine;

public class PlaySoundOnEventTarget : MonoBehaviour
{
    [SerializeField] AudioClip[] clips;
    [SerializeField] float volume = 0.2f;
    public string eventName;


    private void OnEnable()
    {
        if (clips.Length > 0)
        {
            EventManager.StartListening(eventName, OnEventTriggered);
        }
    }

    private void OnDisable()
    {
        EventManager.StopListening(eventName, OnEventTriggered);
    }

    protected virtual void OnEventTriggered(IEventArgs args)
    {
        var senderDetails = args as ISenderEventArgs;

        if (senderDetails == null)
            return;

        AudioSource.PlayClipAtPoint(clips.RandomItem(), senderDetails.Sender.position, volume);
    }
}
