using System.Collections;
using UnityEngine;

/// <summary>
/// The VR simulation seems to stop running on my machine.
/// This will re-activate it after a certain amount of time to save having to manually do it
/// </summary>
public class VR_SDK_Fix : MonoBehaviour
{

    [SerializeField] GameObject[] expectedSDKs;
    [SerializeField] GameObject fallback;
    [SerializeField] float timeToWait = 2.0f;

    void Start()
    {
        StartCoroutine(checkSDKs());
    }

    IEnumerator checkSDKs()
    {
        yield return new WaitForSecondsRealtime(timeToWait);

        bool anyActive = false;
        foreach (var sdk in expectedSDKs)
        {
            anyActive |= sdk.activeInHierarchy;
        }

        if (!anyActive)
        {
            fallback.SetActive(true);
        }
    }
}
