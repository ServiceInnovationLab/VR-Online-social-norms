using UnityEngine;

public class AnimatedRotation : MonoBehaviour
{
    [SerializeField] float targetRotation;
    [SerializeField] float time;

    float startTime = -1;

    private void OnEnable()
    {
        if (startTime > 0)
        {
            enabled = false;
        }

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
