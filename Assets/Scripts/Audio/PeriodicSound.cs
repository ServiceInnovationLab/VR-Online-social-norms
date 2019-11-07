using UnityEngine;
using System.Collections;

public class PeriodicSound : MonoBehaviour
{
    [SerializeField] float volume = 0.2f;
    [SerializeField] AudioClip[] sounds;
    [SerializeField] FloatRange timeBetweenSounds;

    private void OnEnable()
    {
        if (sounds != null && sounds.Length > 0)
        {
            StartCoroutine(PlaySounds());
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator PlaySounds()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenSounds.GetValue());

            AudioSource.PlayClipAtPoint(sounds.RandomItem(), transform.position, volume);
        }
    }

}
