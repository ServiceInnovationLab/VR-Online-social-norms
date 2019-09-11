using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class VolumeAdjuster : MonoBehaviour
{
    [SerializeField] float startingVolume = 0;
    [SerializeField] string volumeType = "MasterVolume";
    [SerializeField] float raisedVolume = 0;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] float fadeTime = 0;

    private void Start()
    {
        var result = audioMixer.SetFloat(volumeType, startingVolume);
    }

    public void RaiseVolume()
    {
        if (fadeTime <= 0)
        {
            audioMixer.SetFloat(volumeType, raisedVolume);
        }
        else
        {
            StartCoroutine(FadeRaiseVolume());
        }
    }

    IEnumerator FadeRaiseVolume()
    {
        float delta = (raisedVolume - startingVolume) / (fadeTime / Time.fixedDeltaTime);

        if (delta == 0)
        {
            yield break;
        }

        float current = startingVolume;

        while (current < raisedVolume)
        {
            current += delta;

            if (current > raisedVolume)
            {
                current = raisedVolume;
            }
        }

        audioMixer.SetFloat(volumeType, current);
        yield return new WaitForFixedUpdate();
    }


}
