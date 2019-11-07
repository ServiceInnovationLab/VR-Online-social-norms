using UnityEngine;
using UnityEngine.Events;

public class OnDestroyHandler : MonoBehaviour
{
    public delegate void OnDestroyEventHandler(GameObject gameObject);

    public UnityEvent destroyed = new UnityEvent();

    public event OnDestroyEventHandler OnDestroyed;

    public GameObject owner;

    private void OnDestroy()
    {
        destroyed?.Invoke();
        OnDestroyed?.Invoke(gameObject);
    }

}
