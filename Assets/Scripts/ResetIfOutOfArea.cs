using UnityEngine;
using System.Collections;

public class ResetIfOutOfArea : MonoBehaviour
{
    [SerializeField] Transform resetPoint;
    [SerializeField] Collider area;

    Rigidbody body;

    private void Awake()
    {
        if (!area)
        {
            Debug.LogError("No area is set");
        }

        if (!resetPoint)
        {
            Debug.LogError("No reset point is set");
        }

        body = GetComponent<Rigidbody>();
    }

    private void LateUpdate()
    {
        if (VectorUtils.IsPointWithinCollider(area, transform.position.XZ()) && transform.position.y > area.bounds.max.y)
            return;

        if (body)
        {
            body.MovePosition(resetPoint.position);
            body.MoveRotation(resetPoint.rotation);
        }
        else
        {
            transform.position = resetPoint.position;
            transform.rotation = resetPoint.rotation;
        }
    }
}
