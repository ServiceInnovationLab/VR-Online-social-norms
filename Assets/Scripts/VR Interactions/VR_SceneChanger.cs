using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using VRTK;

public class VR_SceneChanger : MonoBehaviour
{
    [Tooltip("The scene that that is desired to change to")]
    [SerializeField] SceneReference scene;

    [Tooltip("The colour to fade to when making the scene switch.")]
    [SerializeField] Color blinkToColor = Color.black;

    [Tooltip("The delay before doing the teleport")]
    [SerializeField] float delay = 0;

    [SerializeField] string teleportToName;

    [Tooltip("The custom blink distance to use for the teleport to allow for a longer blink when teleporting to a nearby location")]
    [SerializeField, Range(0f, 32.9f)] float blinkDistance = 32.1f;

    [Tooltip("The custom blink transition time to use for the teleport")]
    [SerializeField, Range(0, 10)] float blinkTransition = 1;

    [SerializeField] UnityEvent onTeleport;

    bool switched;

    public void SwitchScenes()
    {
        if (delay == 0f)
        {
            DoSwitchScene();
        }
        else
        {
            Invoke(nameof(DoSwitchScene), delay);
        }
    }

    void DisableTeleporters()
    {
        foreach (var teleporter in FindObjectsOfType<VRTK_BasicTeleport>())
        {
            teleporter.enabled = false;
        }
    }

    void DoSwitchScene()
    {
        if (!switched)
        {
            if (!string.IsNullOrWhiteSpace(teleportToName))
            {
                foreach (var location in FindObjectsOfType<VR_SceneTeleportTo>())
                {
                    if (location.gameObject.name == teleportToName)
                    {
                        var teleporter = FindObjectOfType<VRTK_BasicTeleport>();

                        var originalBlinkDelay = teleporter.distanceBlinkDelay;
                        var originalBlinkTransition = teleporter.blinkTransitionSpeed;

                        teleporter.distanceBlinkDelay = blinkDistance;
                        teleporter.blinkTransitionSpeed = blinkTransition;

                        teleporter.Teleport(location.transform, location.transform.position);
                        EventManager.TriggerEvent(Events.SceneSwitched, new SceneChangeEventArgs(teleportToName));
                        onTeleport?.Invoke();

                        teleporter.distanceBlinkDelay = originalBlinkDelay;
                        teleporter.blinkTransitionSpeed = originalBlinkTransition;

                        return;
                    }
                }
            }

            if (!string.IsNullOrEmpty(scene.ScenePath))
            {
                switched = true;

                var steam = VRTK_SDKManager.instance.transform.Find("[VRTK_SDKSetups]/SteamVR");
                if (false && steam && steam.gameObject.activeInHierarchy)
                {
                    var loader = gameObject.AddComponent<Valve.VR.SteamVR_LoadLevel>();
                    loader.levelName = scene.ScenePath;
                    loader.loadingScreenWidthInMeters = 0;
                    loader.Trigger();
                }
                else
                {
                    DisableTeleporters();
                    StartCoroutine(DoSwitchSceneLoad());
                }
            }
        }
    }

    IEnumerator DoSwitchSceneLoad()
    {
        VRTK_SDK_Bridge.HeadsetFade(blinkToColor, 0, true);

        yield return SceneManager.LoadSceneAsync(scene.ScenePath, LoadSceneMode.Single);
    }
}