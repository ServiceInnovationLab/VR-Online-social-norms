using UnityEngine;

public static class VectorExtensions
{

    /// <summary>
    /// Returns the vector with just the X and Z components
    /// </summary>
    public static Vector3 XZ(this Vector3 vector)
    {
        return new Vector3(vector.x, 0, vector.z);
    }

    /// <summary>
    /// Returns a vector with just the absolute value of each component
    /// </summary>
    public static Vector3 Abs(this Vector3 vector)
    {
        return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
    }
}