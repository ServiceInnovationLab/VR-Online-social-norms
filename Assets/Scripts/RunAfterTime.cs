using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class RunAfterTime : MonoBehaviour
{
    [SerializeField] float timeToWait;
    [SerializeField] UnityEvent onTimeUp;

    private void OnEnable()
    {
        Invoke("CallEvent", timeToWait);
    }

    void CallEvent()
    {
        onTimeUp?.Invoke();
    }
}
