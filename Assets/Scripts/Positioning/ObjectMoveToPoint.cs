using System;
using UnityEngine;

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
            rigidBody.MovePosition(transform.position);
            rigidBody.rotation = rotation;
        }
        else
        {
            transformToMove.position = transform.position;
            transformToMove.localRotation = rotation;
        }
    }
}