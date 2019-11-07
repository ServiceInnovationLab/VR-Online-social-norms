using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class CollisionTriggerUnityEvent : MonoBehaviour
{
    public UnityEvent triggerEnter;

    private void OnTriggerEnter(Collider other)
    {
        triggerEnter?.Invoke();
    }

}
