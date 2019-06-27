using UnityEngine;
using UnityEngine.SceneManagement;

public class AddAdditionalScenes : MonoBehaviour
{
    [Tooltip("The scenes listed here will get additivly loaded to the current scene")]
    [SerializeField] SceneReference[] scenes;

    void Awake()
    {
        foreach (var scene in scenes)
        {
            if (!SceneManager.GetSceneByName(scene).isLoaded)
            {
                SceneManager.LoadScene(scene.ScenePath, LoadSceneMode.Additive);
            }
        }
    }
}