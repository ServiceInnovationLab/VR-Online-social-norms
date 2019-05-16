using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Rock : MonoBehaviour
{

    [SerializeField, Range(0.0f, 1.0f)] float bounceScale = 0.6f;
    [SerializeField, Range(0.0f, 1.0f)] float straightnessLimit = 0.2f;

    Rigidbody body;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Water")
            return;

        float staightness = 1.0f - Mathf.Abs(Vector3.Dot(Vector3.up, body.velocity.normalized));
        // Mathf.Abs(body.velocity.x) + Mathf.Abs(body.velocity.z) > Mathf.Abs(body.velocity.y) *  (1.0f -straightnessLimit)

        if (staightness < straightnessLimit)
            return;

        Vector3 newVelocity = body.velocity;
        newVelocity.y = Mathf.Abs(newVelocity.y) * bounceScale;

        body.velocity = newVelocity;
    }
}

