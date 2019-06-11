using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ActivateAfterDelay : MonoBehaviour
{
    [SerializeField] GameObject objectToActivate;
    [SerializeField] float delayTime;
    [SerializeField] UnityEvent afterDelay;

    public void Activate()
    {
        StartCoroutine(DoActivate());
    }

    IEnumerator DoActivate()
    {
        yield return new WaitForSeconds(delayTime);
        objectToActivate.SetActive(true);
        afterDelay?.Invoke();
    }
    
}
