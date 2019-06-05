using System.Collections;
using UnityEngine;
using VRTK;

[RequireComponent(typeof(VRTK_SDKManager))]
public class FadeInScene : MonoBehaviour
{
    [Tooltip("The colour to fade to when making the scene switch.")]
    [SerializeField] Color blinkToColor = Color.black;

    [Tooltip("The duration of fading in from black")]
    [SerializeField] float fadeInTime = 1.0f;

    [Tooltip("The amount of time to keep the headset black while making the scene change")]
    [SerializeField] float blinkPause = 1.0f;

    VRTK_SDKManager manager;

    private void Awake()
    {
        manager = GetComponent<VRTK_SDKManager>();
        manager.LoadedSetupChanged += Manager_LoadedSetupChanged;
    }

    private void Manager_LoadedSetupChanged(VRTK_SDKManager sender, VRTK_SDKManager.LoadedSetupChangeEventArgs e)
    {
        if (e.currentSetup == null)
        {
            return;
        }

        VRTK_SDK_Bridge.HeadsetFade(blinkToColor, 0, true);
        StartCoroutine(DoFade());
    }

    IEnumerator DoFade()
    {
        yield return new WaitForSeconds(blinkPause);

        VRTK_SDK_Bridge.HeadsetFade(Color.clear, fadeInTime, true);
    }

}
