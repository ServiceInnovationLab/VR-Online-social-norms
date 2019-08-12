using UnityEngine;
using System.Collections;

public class RotateOnScroll : Scrollable
{
    protected override void OnScroll(Vector2 velocity)
    {
        transform.localRotation *= Quaternion.Euler(velocity.x, velocity.y, 0);
    }
}
