using UnityEngine;

[RequireComponent(typeof(PerspectiveChanger))]
public class PerspectiveChangeTrackingAdjust : MonoBehaviour
{
    [SerializeField] float scaleIfNoChairTracking;
    [SerializeField] GameObject[] toEnableIfNoChairTracking;
    [SerializeField] GameObject[] toDisableIfNoChairTracking;

    // Use this for initialization
    void Awake()
    {
        // If we want tracking, nothing to do
        if (TrackedChairOption.GetValue())
            return;

        var pov = GetComponent<PerspectiveChanger>();

        pov.doTeleport = true;
        pov.doRotate = true;
        pov.scaleCamera = true;

        var newScale = pov.transform.localScale;
        newScale.x = scaleIfNoChairTracking;
        newScale.z = scaleIfNoChairTracking;

        pov.transform.localScale = newScale;

        foreach (var item in toEnableIfNoChairTracking)
        {
            item.SetActive(true);
        }

        foreach (var item in toDisableIfNoChairTracking)
        {
            item.SetActive(false);
        }
    }
}
