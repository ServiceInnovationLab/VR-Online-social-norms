using UnityEngine;

public class AnimatedRotation : MonoBehaviour
{
    [SerializeField] float targetRotation;
    [SerializeField] float time;

    float startTime;

    private void OnEnable()
    {
        startTime = Time.time;
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
