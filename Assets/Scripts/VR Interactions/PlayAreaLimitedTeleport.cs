using UnityEngine;
using VRTK;

public class PlayAreaLimitedTeleport : VRTK_HeightAdjustTeleport
{

    [SerializeField] Collider playBounds;

    public override bool ValidLocation(Transform target, Vector3 destinationPosition)
    {
        var max = playBounds.bounds.max;
        var min = playBounds.bounds.min;

        if (destinationPosition.x < min.x || destinationPosition.x > max.x
                || destinationPosition.z < min.z || destinationPosition.z > max.z)
            return false;

        return base.ValidLocation(target, destinationPosition);
    }

}

