using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
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
        var positionToCheck = transform.position.XZ();
        positionToCheck.y = area.bounds.center.y;

        var inBounds = VectorUtils.IsPointWithinCollider(area, positionToCheck);
        var isAbove = transform.position.y > area.bounds.max.y;

        if (inBounds || (isAbove && !body.IsSleeping()))
            return;

        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
        body.Sleep();

        body.position = resetPoint.position;
        body.rotation = resetPoint.rotation;
    }
}
