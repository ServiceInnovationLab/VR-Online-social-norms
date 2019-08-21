using UnityEngine;
using Valve.VR;
using VRTK;
using System.Collections;

public class SteamControllerColour : MonoBehaviour
{
    [SerializeField] VRTK_SDKManager sdkManager;
    [SerializeField] SteamVR_RenderModel controller1;
    [SerializeField] SteamVR_RenderModel controller2;

    MeshRenderer touchpad1Renderer;
    MeshRenderer trigger1Renderer;

    MeshRenderer touchpad2Renderer;
    MeshRenderer trigger2Renderer;

    bool steamLoaded = false;
    bool gettingControllers = false;

    private void Awake()
    {
        sdkManager.LoadedSetupChanged += SdkManager_LoadedSetupChanged;
    }

    private void SdkManager_LoadedSetupChanged(VRTK_SDKManager sender, VRTK_SDKManager.LoadedSetupChangeEventArgs e)
    {
        if (e.currentSetup == null)
            return;

        steamLoaded = e.currentSetup.name.Contains("Steam");
    }

    private void FixedUpdate()
    {
        if (!steamLoaded || gettingControllers)
            return;

        if (!hasModels())
        {
            StartCoroutine(GetControllers());
        }
    }

    private bool hasModels()
    {
        return touchpad1Renderer && trigger1Renderer
                    && touchpad2Renderer && trigger2Renderer;
    }

    IEnumerator GetControllers()
    {
        gettingControllers = true;
        while (true)
        {
            yield return new WaitForSeconds(0.25f);

            touchpad1Renderer = controller1.transform.Find("trackpad")?.GetComponent<MeshRenderer>();
            trigger1Renderer = controller1.transform.Find("trigger")?.GetComponent<MeshRenderer>();

            touchpad2Renderer = controller2.transform.Find("trackpad")?.GetComponent<MeshRenderer>();
            trigger2Renderer = controller2.transform.Find("trigger")?.GetComponent<MeshRenderer>();

            if (hasModels())
            {
                break;
            }
        }
        gettingControllers = false;
    }

}
