using UnityEngine;
using VRTK;

public class PlayAreaLimitedTeleport : VRTK_HeightAdjustTeleport
{
    [SerializeField] VrPlayArea[] playAreas;

    protected override void Awake()
    {
        if (playAreas.Length == 0)
        {
            Debug.LogError("No play areas!", gameObject);
        }

        base.Awake();
    }

    public override bool ValidLocation(Transform target, Vector3 destinationPosition)
    {
        bool inPlayArea = false;

        foreach (var area in playAreas)
        {
            if (area.IsDestinationPointValid(destinationPosition))
            {
                inPlayArea = true;
                break;
            }
        }

        return inPlayArea && base.ValidLocation(target, destinationPosition);
    }

}

