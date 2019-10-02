using UnityEngine;
using UnityEngine.Events;

public class Enabled_UnityEvents : MonoBehaviour
{
    [SerializeField] UnityEvent onEnabled;
    [SerializeField] UnityEvent onDisabled;

    private void OnEnable()
    {
        onEnabled?.Invoke();
    }

    private void OnDisable()
    {
        onDisabled?.Invoke();
    }
}
