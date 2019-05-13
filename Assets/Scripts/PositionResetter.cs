using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This behaviour allows resetting the position of an object to this location 
/// </summary>
public class PositionResetter : MonoBehaviour
{
    [SerializeField] Transform objectToReset;

    private void Start()
    {
        ResetObjectPosition();
    }

    public void ResetObjectPosition()
    {
        var rigidBody = transform.GetComponent<Rigidbody>();

        if (rigidBody)
        {
            rigidBody.velocity = Vector3.zero;
            rigidBody.angularVelocity = Vector3.zero;
        }

        objectToReset.position = transform.position;
    }
}
