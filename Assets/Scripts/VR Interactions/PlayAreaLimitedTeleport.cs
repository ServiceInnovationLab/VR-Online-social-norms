using UnityEngine;
using VRTK;

public class PlayAreaLimitedTeleport : VRTK_HeightAdjustTeleport
{
    [SerializeField] bool onlyTeleportToPointers;
    [SerializeField] bool checkForCollisions = true;
    VrPlayArea[] playAreas;

    VRTK_BodyPhysics bodyPhysics;
    CapsuleCollider bodyCollider;

    Collider[] hitColliders;

    protected override void Awake()
    {
        bodyPhysics = FindObjectOfType<VRTK_BodyPhysics>();
        playAreas = FindObjectsOfType<VrPlayArea>();

        if (playAreas.Length == 0)
        {
            Debug.LogError("No play areas!", gameObject);
        }

        if (checkForCollisions)
        {
            hitColliders = new Collider[5];
        }

        base.Awake();
    }    

    public void SetOnlyToPointers(bool value)
    {
        onlyTeleportToPointers = value;
    }

    public bool GetOnlyTeleportToPointers()
    {
        return onlyTeleportToPointers;
    }

    public override bool ValidLocation(Transform target, Vector3 destinationPosition)
    {
        if (onlyTeleportToPointers && (!target.GetComponent<VRTK_DestinationPoint>() || !target.GetComponentInParent<VRTK_DestinationPoint>()))
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

        if (inPlayArea && checkForCollisions)
        {
            if (!bodyCollider)
            {
                bodyCollider = bodyPhysics.GetBodyColliderContainer().GetComponent<CapsuleCollider>();
            }

            if (bodyCollider)
            {
                var originalY = destinationPosition.y;
                destinationPosition.y = GetTeleportY(target, destinationPosition) + bodyCollider.radius * 2 + 0.05f;

                int count = Physics.OverlapCapsuleNonAlloc(destinationPosition, destinationPosition + Vector3.up * bodyCollider.height, bodyCollider.radius * 2, hitColliders, ~0, QueryTriggerInteraction.Ignore);

              /*  var newItem = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
                newItem.position = destinationPosition;
                newItem.localScale = new Vector3(bodyCollider.radius * 2, bodyCollider.radius * 2, bodyCollider.radius * 2);
                Destroy(newItem.GetComponent<Collider>());

                newItem = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
                newItem.position = destinationPosition + Vector3.up * bodyCollider.height;
                newItem.localScale = new Vector3(bodyCollider.radius * 2, bodyCollider.radius * 2, bodyCollider.radius * 2);
                Destroy(newItem.GetComponent<Collider>());*/

                if (count > 0)
                {
                    inPlayArea = false;

                    /*for (int i = 0; i < count; i++)
                    {
                        Debug.LogError("Can't teleport due to ", hitColliders[i]);
                    }*/
                }

                destinationPosition.y = originalY;
            }
        }

        return inPlayArea && base.ValidLocation(target, destinationPosition);
    }

}

