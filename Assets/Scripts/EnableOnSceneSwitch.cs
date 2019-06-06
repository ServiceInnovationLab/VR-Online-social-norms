using UnityEngine;

/// <summary>
/// Enables an object on 'scene switch' - when there are multiple scenes loaded
/// this wait until the scene is switched too
/// </summary>
public class EnableOnSceneSwitch : MonoBehaviour
{
    [SerializeField] GameObject toEnable;
    [SerializeField] GameObject teleportTo;

    private void Awake()
    {
        if (!FindObjectOfType<AddAdditionalScenes>())
        {
            DoEnable(new SceneChangeEventArgs(teleportTo.name));
        }
    }

    private void OnEnable()
    {
        EventManager.StartListening(Events.SceneSwitched, DoEnable);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.SceneSwitched, DoEnable);
    }

    void DoEnable(IEventArgs args)
    {
        if (args == null || !(args is SceneChangeEventArgs))
        {
            Debug.LogError("Wrong argument given to DoEnable!");
            return;
        }

        var eventArgs = (SceneChangeEventArgs)args;

        if (eventArgs.SceneTeleportName == teleportTo.name)
        {
            toEnable.SetActive(true);
            Destroy(this);
        }
    }
}