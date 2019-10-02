using UnityEngine;
using UnityEngine.Events;

public class SenderRoomInteraction : MonoBehaviour
{
    public UnityEvent OnEnterPov;

    public UnityEvent[] interactions;

    public void OnPovSwitch()
    {
        OnEnterPov?.Invoke();
    }

    public void TriggerInteraction(int index)
    {
        interactions[index]?.Invoke();
    }

}
