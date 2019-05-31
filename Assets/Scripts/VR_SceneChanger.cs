using System.Collections;
using UnityEngine;
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
                        // Teleport here
                        var teleporter = FindObjectOfType<VRTK_BasicTeleport>();
                        teleporter.Teleport(location.transform, location.transform.position);

                        return;
                    }
                }
            }

            if (!string.IsNullOrEmpty(scene.ScenePath))
            {
                DisableTeleporters();
                StartCoroutine(DoSwitchSceneLoad());
                switched = true;
            }
        }
    }

    IEnumerator DoSwitchSceneLoad()
    {
        VRTK_SDK_Bridge.HeadsetFade(blinkToColor, 0, true);

        yield return SceneManager.LoadSceneAsync(scene.ScenePath, LoadSceneMode.Single);
    }
}