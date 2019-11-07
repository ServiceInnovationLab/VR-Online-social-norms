using UnityEngine;
using Valve.VR;
using VRTK;
using System.Collections;

public class SteamControllerColour : MonoBehaviour
{
    [SerializeField] VRTK_SDKManager sdkManager;
    [SerializeField] SteamVR_RenderModel controllerLeft;
    [SerializeField] SteamVR_RenderModel controllerRight;

    [SerializeField] VRTK_Pointer leftPointer;
    [SerializeField] VRTK_Pointer rightPointer;

    [SerializeField] Color triggerButtonColour = Color.yellow;
    [SerializeField] Color touchpadButtonColour = Color.blue;

    EmissionGlow touchpad1Renderer;
    EmissionGlow trigger1Renderer;

    EmissionGlow touchpad2Renderer;
    EmissionGlow trigger2Renderer;

    bool steamLoaded = false;
    bool gettingControllers1 = false;
    bool gettingControllers2 = false;
    [SerializeField] bool animateTouchpad = false;
    [SerializeField]  bool animateTrigger = false;
    [SerializeField] bool showTriggers = true;

    public void SetGlowTouchpad(bool glow)
    {
        animateTouchpad = glow;
    }

    public void SetGlowTrigger(bool glow)
    {
        animateTrigger = glow;
    }

    public void SetShowTrigger(bool show)
    {
        showTriggers = show;
    }

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
        if (!steamLoaded)
            return;

        if (hasModels1())
        {
            touchpad1Renderer.showEmission = leftPointer.enabled && leftPointer.enableTeleport;
            touchpad1Renderer.animate = animateTouchpad;
            trigger1Renderer.animate = animateTrigger;
            trigger1Renderer.showEmission = showTriggers;
        }
        else if (!gettingControllers1)
        {
            StartCoroutine(GetController1());
        }


        if (hasModels2())
        {
            touchpad2Renderer.showEmission = rightPointer.enabled && rightPointer.enableTeleport;
            touchpad2Renderer.animate = animateTouchpad;
            trigger2Renderer.animate = animateTrigger;
            trigger2Renderer.showEmission = showTriggers;
        }
        else if(!gettingControllers2)
        {
            StartCoroutine(GetController2());
        }
    }

    private bool hasModels1()
    {
        return touchpad1Renderer && trigger1Renderer;
    }

    private bool hasModels2()
    {
        return touchpad2Renderer && trigger2Renderer;
    }

    IEnumerator GetController1()
    {
        gettingControllers1 = true;
        while (true)
        {
            yield return new WaitForSeconds(0.25f);

            touchpad1Renderer = controllerLeft.transform.Find("trackpad")?.gameObject?.GetOrCreateComponent<EmissionGlow>();
            trigger1Renderer = controllerLeft.transform.Find("trigger")?.gameObject?.GetOrCreateComponent<EmissionGlow>();

            if (hasModels1())
                break;
        }
        gettingControllers1 = false;

        touchpad1Renderer.emissionColour = touchpadButtonColour;
        touchpad1Renderer.maxGlow = touchpadButtonColour.a;
        trigger1Renderer.emissionColour = triggerButtonColour;
        trigger1Renderer.maxGlow = triggerButtonColour.a;
    }

    IEnumerator GetController2()
    {
        gettingControllers2 = true;
        while (true)
        {
            yield return new WaitForSeconds(0.25f);

            touchpad2Renderer = controllerRight.transform.Find("trackpad")?.gameObject?.GetOrCreateComponent<EmissionGlow>();
            trigger2Renderer = controllerRight.transform.Find("trigger")?.gameObject?.GetOrCreateComponent<EmissionGlow>();

            if (hasModels2())
                break;
        }
        gettingControllers2 = false;

        touchpad2Renderer.emissionColour = touchpadButtonColour;
        touchpad2Renderer.maxGlow = touchpadButtonColour.a;
        trigger2Renderer.emissionColour = triggerButtonColour;
        trigger2Renderer.maxGlow = triggerButtonColour.a;
    }

}
