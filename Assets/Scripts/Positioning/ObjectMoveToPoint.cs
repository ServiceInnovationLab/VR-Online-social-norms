using UnityEngine;
using System.Collections;

public class ObjectMoveToPoint : MonoBehaviour
{
    [SerializeField] Vector3 rotation = new Vector3(78.967f, -25.264f, -24.854f);
    
    public void MoveObjectHere(Transform transformToMove)
    {
        MoveObjectHere(transformToMove, transformToMove.localRotation);
    }

    public void MoveObjectHereRotateToSetRotation(Transform transformToMove)
    {
        MoveObjectHere(transformToMove, Quaternion.Euler(rotation));
    }

    public void MoveObjectHere(Transform transformToMove, Quaternion rotation)
    {
        var rigidBody = transformToMove.GetComponent<Rigidbody>();

        if (rigidBody)
        {
            rigidBody.constraints = RigidbodyConstraints.None;
            rigidBody.position = transform.position;
            rigidBody.rotation = rotation;
            StartCoroutine(FixRigidBody(rigidBody));
        }
        else
        {
            transformToMove.position = transform.position;
            transformToMove.localRotation = rotation;
        }
    }

    IEnumerator FixRigidBody(Rigidbody rigidbody)
    {
        yield return new WaitForSeconds(0.1f);
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        rigidbody.isKinematic = false;
        rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }
}