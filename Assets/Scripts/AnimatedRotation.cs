using UnityEngine;

public class AnimatedRotation : MonoBehaviour
{
    [SerializeField] float targetRotation;
    [SerializeField] float time;
    [SerializeField] AudioClip[] soundOnAnimate;
    [SerializeField] float volume = 0.2f;

    float startTime = -1;

    private void OnEnable()
    {
        if (startTime > 0)
        {
            enabled = false;
        }

        startTime = Time.time;

        if (soundOnAnimate != null && soundOnAnimate.Length > 0)
        {
            AudioSource.PlayClipAtPoint(soundOnAnimate.RandomItem(), transform.position, volume);
        }
    }

    private void FixedUpdate()
    {
        float newAngle = Mathf.LerpAngle(0, targetRotation, (Time.time - startTime) / time);
        transform.rotation = Quaternion.Euler(0, newAngle, 0);

        if (newAngle == targetRotation)
        {
            enabled = false;
        }
    }

}
