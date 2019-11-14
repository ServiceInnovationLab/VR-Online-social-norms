using UnityEngine;
using Mirror;

public class MicrophoneTransfer : NetworkBehaviour
{
    public const int Frequency = 44100;
    public const int MaxAudioClipSamples = 100 * Frequency;
    public const int SecondsToRecord = 10;
    public const int AudioTransmissionChannels = 1;

    string microphone = null;
    int lastSentPosition;
    AudioClip microphoneAudio;

    RemoteMicrophonePlayer playback;

    private void Awake()
    {
        playback = GetComponent<RemoteMicrophonePlayer>();
    }

    public override void OnStartLocalPlayer()
    {
        foreach (var device in Microphone.devices)
        {
            if (device.ToLower().Contains("vive"))
            {
                microphone = device;
                break;
            }
        }

        microphoneAudio = Microphone.Start(microphone, true, SecondsToRecord, Frequency);
    }

    private void OnDisable()
    {
        if (!isLocalPlayer)
            return;

        Microphone.End(microphone);
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;

        var currentPosition = Microphone.GetPosition(microphone);
        int sampleCount = GetNewSampleCount(currentPosition);

        if (sampleCount > 0)
        {
            SendAudio(sampleCount);
            lastSentPosition = currentPosition;
        }
    }

    int GetNewSampleCount(int currentPosition)
    {
        var count = currentPosition - lastSentPosition;

        if (count < 0)
        {
            count = microphoneAudio.samples - lastSentPosition + currentPosition;
        }
        
        return count;
    }

    void SendAudio(int sampleCount)
    {
        float[] samples = new float[sampleCount * microphoneAudio.channels];
        microphoneAudio.GetData(samples, lastSentPosition);
        CmdSendAudio(new MicrophoneData(samples, microphoneAudio.channels));
    }

    [Command]
    void CmdSendAudio(MicrophoneData data)
    {
        Debug.Log(data.Samples.Length);
        foreach (var connection in NetworkServer.connections)
        {
           // if (connection.Value != connectionToClient)
            {
                TargetPlayAudio(connection.Value, data);
            }
        }
    }

    [TargetRpc]
    public void TargetPlayAudio(NetworkConnection target, MicrophoneData data)
    {
        playback.ReceiveAudio(data);
    }
}
