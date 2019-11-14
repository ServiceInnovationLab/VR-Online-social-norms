using UnityEngine;

public class RemoteMicrophonePlayer : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    int lastSamplePlayed;

    private void Awake()
    {
        if (!audioSource)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void ReceiveAudio(MicrophoneData data)
    {
        if (!audioSource.clip)
        {
            SetupAudioSource();
        }

        audioSource.clip.SetData(data.GetFloats(), lastSamplePlayed);

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }

        lastSamplePlayed = (lastSamplePlayed + data.Samples.Length) % MicrophoneTransfer.MaxAudioClipSamples;
    }

    void SetupAudioSource()
    {
        lastSamplePlayed = 0;
        audioSource.clip = AudioClip.Create("ReceivedAudio", MicrophoneTransfer.MaxAudioClipSamples,
            MicrophoneTransfer.AudioTransmissionChannels, MicrophoneTransfer.Frequency, false);
    }
}
