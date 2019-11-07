using UnityEngine;

public class TrackedObjectChild : MonoBehaviour
{
    public bool onlyY = false;
    public bool placeBasedOnHeight;

    TrackedObject trackedParent;

    private void Awake()
    {
        trackedParent = GetComponentInParent<TrackedObject>();
    }

    private void LateUpdate()
    {
        if (onlyY)
        {
            var angles = trackedParent.transform.localRotation.eulerAngles;
            transform.localRotation = Quaternion.Euler(-angles.x, 0, -angles.z);
        }

        if (placeBasedOnHeight)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, -trackedParent.transform.localPosition.y, transform.localPosition.z);
        }
    }
}
