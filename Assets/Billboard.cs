using VRTK;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Camera camera;

    private void Awake()
    {
        VRTK_SDKManager.instance.LoadedSetupChanged += LoadedSetupChanged;
    }

    private void LoadedSetupChanged(VRTK_SDKManager sender, VRTK_SDKManager.LoadedSetupChangeEventArgs e)
    {
        if (!this || e.currentSetup == null)
            return;

        camera = e.currentSetup.headsetSDK.GetHeadsetCamera().GetComponentInChildren<Camera>();
    }

    //Orient the camera after all movement is completed this frame to avoid jittering
    void LateUpdate()
    {
        if (!camera)
            return;

        transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, Vector3.up);
    }
}
