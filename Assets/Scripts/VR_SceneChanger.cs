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

    bool switched;

    public void SwitchScenes()
    {
        if (!switched && scene != null && !string.IsNullOrEmpty(scene.ScenePath))
        {
            DisableTeleporters();
            StartCoroutine(DoSwitchScene());
            switched = true;
        }
    }

    void DisableTeleporters()
    {
        foreach (var teleporter in FindObjectsOfType<VRTK_BasicTeleport>())
        {
            teleporter.enabled = false;
        }
    }

    IEnumerator DoSwitchScene()
    {
        VRTK_SDK_Bridge.HeadsetFade(blinkToColor, 0, true);

        yield return SceneManager.LoadSceneAsync(scene.ScenePath, LoadSceneMode.Single);
    }
}