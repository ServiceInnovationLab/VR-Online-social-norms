using UnityEngine;
using VRTK;

public class ShowIfCloseToPlayer : MonoBehaviour
{
    [SerializeField] float showDistance = 1.0f;
    [SerializeField] Transform objectToCompareDistance;
    [SerializeField] Transform objectToToggle;    

    Transform player;

    private void Awake()
    {
        VRTK_SDKManager.AttemptAddBehaviourToToggleOnLoadedSetupChange(this);

        if (!objectToCompareDistance)
        {
            objectToCompareDistance = transform;
        }
    }

    private void OnDestroy()
    {
        VRTK_SDKManager.AttemptRemoveBehaviourToToggleOnLoadedSetupChange(this);
    }

    private void OnEnable()
    {
        player = VRTK_DeviceFinder.HeadsetTransform();
    }

    private void FixedUpdate()
    {
        if (!player || !objectToToggle || !objectToCompareDistance)
            return;

        var distance = Vector3.Distance(player.position.XZ(), objectToCompareDistance.position.XZ());

        objectToToggle.gameObject.SetActive(distance <= showDistance);
    }
}
