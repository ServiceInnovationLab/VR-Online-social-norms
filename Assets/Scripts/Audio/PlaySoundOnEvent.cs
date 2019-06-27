using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySoundOnEvent : MonoBehaviour
{
    [SerializeField] AudioClip[] clips;
    [SerializeField] bool changePitch;
    [SerializeField] FloatRange pitch;
    public string eventName;

    AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

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

    void OnEventTriggered()
    {
        var clip = clips[Random.Range(0, clips.Length)];

        if (changePitch)
        {
            source.pitch = pitch.GetValue();
        }

        source.PlayOneShot(clip);
    }
}
