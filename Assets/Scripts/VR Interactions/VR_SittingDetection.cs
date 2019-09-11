using UnityEngine;
using VRTK;
using Valve.VR;

public class VR_SittingDetection : MonoBehaviour
{
    [SerializeField] int bufferSize = 90;
    Transform headsetTransform;

    SteamVR_RingBuffer<float> heights;

    int seen;

    private void Awake()
    {
        VRTK_SDKManager.AttemptAddBehaviourToToggleOnLoadedSetupChange(this);

        heights = new SteamVR_RingBuffer<float>(bufferSize);
    }

    private void OnDestroy()
    {
        VRTK_SDKManager.AttemptRemoveBehaviourToToggleOnLoadedSetupChange(this);
    }


    private void OnEnable()
    {
        headsetTransform = VRTK_DeviceFinder.HeadsetTransform();
    }

    void Update()
    {
        heights.Add(headsetTransform.position.y);

        seen++;

        if (seen < bufferSize)
        {
            seen = 0;

            float sum = 0;
            for (int i = 0; i < bufferSize; i++)
            {
                sum += heights.GetAtIndex(i);
            }
            sum /= bufferSize;
        }
    }
}
