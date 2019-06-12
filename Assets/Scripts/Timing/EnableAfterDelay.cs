using System.Collections;
using UnityEngine;

public class EnableAfterDelay : MonoBehaviour
{
    [SerializeField] GameObject objectToActivate;
    [SerializeField] string scriptToEnable;
    [SerializeField] float delayTime;

    public void Activate()
    {
        StartCoroutine(DoActivate());
    }

    IEnumerator DoActivate()
    {
        yield return new WaitForSeconds(delayTime);
        MonoBehaviour script = objectToActivate.GetComponent(scriptToEnable) as MonoBehaviour;

        if (script)
        {
            script.enabled = true;
        }
        else
        {
            Debug.LogError("Could not find script");
        }
    }
}
