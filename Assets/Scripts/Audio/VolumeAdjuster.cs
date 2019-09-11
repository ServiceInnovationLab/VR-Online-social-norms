using UnityEngine;
using UnityEngine.Audio;

public class VolumeAdjuster : MonoBehaviour
{
    [SerializeField] float startingVolume = 0;
    [SerializeField] float raisedVolume = 0;
    [SerializeField] AudioMixer audioMixer;

    private void Awake()
    {
        audioMixer.SetFloat("MasterVolume", startingVolume);
    }

    public void RaiseVolume()
    {
        audioMixer.SetFloat("MasterVolume", raisedVolume);
    }


}
