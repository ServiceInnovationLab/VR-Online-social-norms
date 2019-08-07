using UnityEngine;

public class VrPlayArea : MonoBehaviour
{
    public bool canUse = true;

    Collider[] colliders;

    private void Awake()
    {
        colliders = GetComponentsInChildren<Collider>();

#if UNITY_EDITOR
        if (colliders.Length == 0)
        {
            Debug.LogError("No colliders given in this play area!", gameObject);
        }
#endif
    }

    public void SetUsable(bool canUse)
    {
        this.canUse = canUse;
    }

    public bool IsDestinationPointValid(Vector3 position)
    {
        if (!canUse)
            return false;

        foreach (var collider in colliders)
        {
            position.y = collider.bounds.center.y;

            if (VectorUtils.IsPointWithinCollider(collider, position))
                return true;
        }

        return false;
    }
}
