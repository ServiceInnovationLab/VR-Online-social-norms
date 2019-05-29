using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This component is for allowing something to happen when an object is
/// moved into the headset to simulate being eaten.
/// </summary>
public class Edible : MonoBehaviour
{
    [SerializeField] UnityEvent onEaten;

    public void OnEaten()
    {
        onEaten?.Invoke();
    }
}
