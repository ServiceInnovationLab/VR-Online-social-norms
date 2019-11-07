using VRTK;
using UnityEngine;

public class Billboard : MonoBehaviour
{
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

        //transform.LookAt(transform.position + headset.transform.rotation * Vector3.forward, Vector3.up);
        transform.LookAt(headset);
    }
}
