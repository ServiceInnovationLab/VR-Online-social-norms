using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldScaleManager : MonoBehaviour
{
    Vector2 originalGravity;

    private static WorldScaleManager worldScaleManager;

    public static WorldScaleManager Instance
    {
        get
        {
            if (!worldScaleManager)
            {
                worldScaleManager = FindObjectOfType<WorldScaleManager>();

                if (!worldScaleManager)
                {
                    var obj = new GameObject("WorldScaleManager");
                    worldScaleManager = obj.AddComponent<WorldScaleManager>();
                    Debug.Log("WorldScaleManager automatically created.");
                }
            }

            return worldScaleManager;
        }
    }

    private void Awake()
    {
        originalGravity = Physics.gravity;

        DontDestroyOnLoad(this);
        SceneManager.activeSceneChanged += ActiveSceneChanged;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.playModeStateChanged += (a) =>
        {
            Physics.gravity = originalGravity;
        };
#endif
    }

    private void ActiveSceneChanged(Scene current, Scene next)
    {
        Physics.gravity = originalGravity;
    }

    public void ChangeWorldScale(WorldScale worldScale)
    {
        Physics.gravity = originalGravity * worldScale.scale;

        if (worldScale.sdkManager)
        {
            worldScale.sdkManager.LoadedSetupChanged += (s, e) =>
            {
                if (e.currentSetup == null)
                    return;

                VRTK.VRTK_DeviceFinder.HeadsetCamera().localScale = new Vector3(worldScale.scale, worldScale.scale, worldScale.scale);
            };
          //  worldScale.sdkManager.transform.localScale = new Vector3(worldScale.scale, worldScale.scale, worldScale.scale);
        }
    }
}
