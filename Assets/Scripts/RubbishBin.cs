using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class RubbishBin : MonoBehaviour
{

    public UnityEvent itemAdded;
    public UnityEvent itemRemoved;

    HashSet<Rigidbody> objectsInside = new HashSet<Rigidbody>();

    private void OnTriggerStay(Collider other)
    {
        if (!other.attachedRigidbody || objectsInside.Contains(other.attachedRigidbody))
            return;

        if (Mathf.Abs(other.attachedRigidbody.velocity.y) < Mathf.Epsilon * 2)
        {
            other.attachedRigidbody.Sleep();
        }


        if (!other.attachedRigidbody.isKinematic && other.attachedRigidbody.IsSleeping())
        {
            Debug.Log("In the bin!");
            objectsInside.Add(other.attachedRigidbody);

            itemAdded?.Invoke();
            other.gameObject.tag = "Untagged";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.attachedRigidbody)
            return;

        objectsInside.Remove(other.attachedRigidbody);
    }

}
