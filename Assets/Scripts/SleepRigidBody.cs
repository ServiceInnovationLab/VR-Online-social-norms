using UnityEngine;

public class SleepRigidBody : MonoBehaviour
{

    private void Awake()
    {
        var rigidBody = GetComponent<Rigidbody>();
        if (rigidBody)
        {
            rigidBody.Sleep();
        }
        Destroy(this);
    }
}
