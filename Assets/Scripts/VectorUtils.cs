using System;
using UnityEngine;


public static class VectorUtils
{

    /// <summary>
    /// Find some projected angle measure off some forward around some axis.
    /// </summary>
    /// <param name="v"></param>
    /// <param name="forward"></param>
    /// <param name="axis"></param>
    /// <returns>Angle in degrees</returns>
    public static float AngleOffAroundAxis(Vector3 v, Vector3 forward, Vector3 axis, bool clockwise = false)
    {
        Vector3 right;
        if (clockwise)
        {
            right = Vector3.Cross(forward, axis);
            forward = Vector3.Cross(axis, right);
        }
        else
        {
            right = Vector3.Cross(axis, forward);
            forward = Vector3.Cross(right, axis);
        }
        return Mathf.Atan2(Vector3.Dot(v, right), Vector3.Dot(v, forward)) * Mathf.Rad2Deg;
    }

    public static bool IsPointWithinCollider(Collider collider, Vector3 point)
    {
        var z = collider.ClosestPoint(point);
        return (collider.ClosestPoint(point) - point).sqrMagnitude < Mathf.Epsilon * Mathf.Epsilon;
    }

}

