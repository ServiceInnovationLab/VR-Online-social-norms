using UnityEngine;
using UnityEngine.Events;

public class HardCollisionDetector : MonoBehaviour
{
    public UnityEvent onHardCollision;

    [SerializeField] float minimumVelocity = 9;

    private void OnEnable()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (enabled && collision.relativeVelocity.magnitude > minimumVelocity)
        {
            onHardCollision?.Invoke();
        }
    }
}
