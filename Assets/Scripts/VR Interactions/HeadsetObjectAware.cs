using VRTK;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(VRTK_HeadsetControllerAware))]
public class HeadsetObjectAware : MonoBehaviour
{
    [SerializeField] Transform objectToTrack;

    public UnityEvent objectGlaced;

    VRTK_HeadsetControllerAware headesetAware;

    private void Awake()
    {
        if (!objectToTrack)
        {
            objectToTrack = transform;
        }

        headesetAware = GetComponent<VRTK_HeadsetControllerAware>();

        headesetAware.trackLeftController = true;
        headesetAware.trackRightController = false;

        headesetAware.customLeftControllerOrigin = objectToTrack;
    }

    private void OnEnable()
    {
        headesetAware.ControllerGlanceEnter += HeadesetAware_ControllerGlanceEnter;
    }

    private void OnDisable()
    {
        headesetAware.ControllerGlanceEnter -= HeadesetAware_ControllerGlanceEnter;
    }

    private void HeadesetAware_ControllerGlanceEnter(object sender, HeadsetControllerAwareEventArgs e)
    {
        objectGlaced?.Invoke();
    }
}