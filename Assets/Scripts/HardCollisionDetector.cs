using UnityEngine;
using UnityEngine.Events;

public class HardCollisionDetector : MonoBehaviour
{
    public UnityEvent onHardCollision;

    [SerializeField] float minimumVelocity = 9;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > minimumVelocity)
        {
            onHardCollision?.Invoke();
        }
    }
}
