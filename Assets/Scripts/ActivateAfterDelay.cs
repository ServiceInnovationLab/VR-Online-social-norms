using System.Collections;
using UnityEngine;

public class ActivateAfterDelay : MonoBehaviour
{
    [SerializeField] GameObject objectToActivate;
    [SerializeField] float delayTime;

    public void Activate()
    {
        StartCoroutine(DoActivate());
    }

    IEnumerator DoActivate()
    {
        yield return new WaitForSeconds(delayTime);
        objectToActivate.SetActive(true);
    }
    
}
