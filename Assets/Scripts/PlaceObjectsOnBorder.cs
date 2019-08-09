using UnityEngine;
using System.Collections;

public class PlaceObjectsOnBorder : MonoBehaviour
{
    public GameObject left, right, top, bottom;

    private void Awake()
    {
        var bounds = GetComponent<Collider>().bounds;

        if (left)
        {
            left.transform.position = new Vector3(bounds.min.x, left.transform.position.y, bounds.center.z);
        }

        if (right)
        {
            right.transform.position = new Vector3(bounds.max.x, right.transform.position.y, bounds.center.z);
        }

        if (top)
        {
            top.transform.position = new Vector3(bounds.center.x, top.transform.position.y, bounds.min.z);
        }

        if (bottom)
        {
            bottom.transform.position = new Vector3(bounds.center.x, bottom.transform.position.y, bounds.max.z);
        }

    }
}
