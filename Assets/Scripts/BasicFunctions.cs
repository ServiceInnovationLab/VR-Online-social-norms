using UnityEngine;

/// <summary>
/// Provides some basic functions to be used for UnityEvents via the inspector
/// </summary>
[CreateAssetMenu(menuName = "Utilities/BasicFunctions")]
public class BasicFunctions : ScriptableObject
{
    /// <summary>
    /// Toggles the active state of the given game object.
    /// </summary>
    /// <param name="gameObject">The <see cref="GameObject"/> to change</param>
    public void ToggleObject(GameObject gameObject)
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }

    /// <summary>
    /// Destroys the given game object
    /// </summary>
    /// <param name="gameObject">The <see cref="GameObject"/> to destroy</param>
    public void DestroyObject(GameObject gameObject)
    {
        Destroy(gameObject);
    }
}