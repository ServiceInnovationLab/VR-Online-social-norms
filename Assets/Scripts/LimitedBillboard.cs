using UnityEngine;
using VRTK;

public class LimitedBillboard : MonoBehaviour
{
    [SerializeField] float limit = 5;
    Transform headset;

    private void Awake()
    {
        VRTK_SDKManager.instance.LoadedSetupChanged += LoadedSetupChanged;
    }

    private void LoadedSetupChanged(VRTK_SDKManager sender, VRTK_SDKManager.LoadedSetupChangeEventArgs e)
    {
        if (!this || e.currentSetup == null)
            return;

        headset = VRTK_DeviceFinder.HeadsetTransform();
    }

    //Orient the camera after all movement is completed this frame to avoid jittering
    void LateUpdate()
    {
        if (!headset)
            return;

        var direction = transform.position - headset.transform.position;
        var rotation = Quaternion.LookRotation(direction);

        rotation = Quaternion.Euler(Mathf.Clamp(rotation.eulerAngles.x, -limit, limit), Mathf.Clamp(rotation.eulerAngles.y, -limit, limit), Mathf.Clamp(rotation.eulerAngles.z, -limit, limit));
        transform.localRotation = rotation;
    }
}
