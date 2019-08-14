using UnityEngine;
using VRTK;

public class PlayAreaLimitedTeleport : VRTK_HeightAdjustTeleport
{
    public bool onlyTeleportToPointers;
    VrPlayArea[] playAreas;

    protected override void Awake()
    {
        playAreas = FindObjectsOfType<VrPlayArea>();

        if (playAreas.Length == 0)
        {
            Debug.LogError("No play areas!", gameObject);
        }

        base.Awake();
    }    

    public void SetOnlyToPointers(bool value)
    {
        onlyTeleportToPointers = value;
    }

    public override bool ValidLocation(Transform target, Vector3 destinationPosition)
    {
        if (onlyTeleportToPointers && !target.GetComponent<VRTK_DestinationPoint>())
        {
            return false;
        }

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

