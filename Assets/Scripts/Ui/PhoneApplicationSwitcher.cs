using UnityEngine;
using UnityEngine.Events;

public class PhoneApplicationSwitcher : MonoBehaviour
{
    [SerializeField] UnityEvent onApplicationSwitched;
    [SerializeField] Transform[] app1;
    [SerializeField] Transform[] app2;

    private void Awake()
    {
        // Switch to app 1
        foreach (var transform in app1)
        {
            transform.gameObject.SetActive(true);
        }

        foreach (var transform in app2)
        {
            transform.gameObject.SetActive(false);
        }
    }

    public void SwitchToApp2()
    {
        foreach (var transform in app1)
        {
            transform.gameObject.SetActive(false);
        }

        foreach (var transform in app2)
        {
            transform.gameObject.SetActive(true);
        }

        onApplicationSwitched?.Invoke();
    }

}
