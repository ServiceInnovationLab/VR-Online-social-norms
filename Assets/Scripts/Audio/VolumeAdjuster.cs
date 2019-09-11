using UnityEngine;
using UnityEngine.Audio;

public class VolumeAdjuster : MonoBehaviour
{
    [SerializeField] float startingVolume = 0;
    [SerializeField] string volumeType = "MasterVolume";
    [SerializeField] float raisedVolume = 0;
    [SerializeField] AudioMixer audioMixer;

    private void Start()
    {
        var result = audioMixer.SetFloat(volumeType, startingVolume);
    }

    public void RaiseVolume()
    {
        audioMixer.SetFloat(volumeType, raisedVolume);
    }


}
