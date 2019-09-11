using UnityEngine;
using UnityEngine.Events;
using VRTK;
using Valve.VR;

public class VR_SittingDetection : MonoBehaviour
{
    int bufferSize = 90;
    [SerializeField] bool disableOnSittingDetected = true;
    [SerializeField] UnityEvent sittingDetected;
    [SerializeField] TrackedObject objectNeededForSitting;
    [SerializeField] int secondsToSettle = 1;
    Transform headsetTransform;

    SteamVR_RingBuffer<float> heights;

    int seen;
    int settledSeconds;

    float lastStandingHeight;

    bool detectSitting = false;

    private void Awake()
    {
        VRTK_SDKManager.AttemptAddBehaviourToToggleOnLoadedSetupChange(this);
    }

    private void OnDestroy()
    {
        VRTK_SDKManager.AttemptRemoveBehaviourToToggleOnLoadedSetupChange(this);
    }

    private void OnEnable()
    {
        headsetTransform = VRTK_DeviceFinder.HeadsetTransform();

        bufferSize = Mathf.CeilToInt(1 / Time.fixedUnscaledDeltaTime);
        heights = new SteamVR_RingBuffer<float>(bufferSize);
    }
    
    void FixedUpdate()
    {
        heights.Add(headsetTransform.position.y);

        seen++;

        if (seen < bufferSize)
        {
            seen = 0;

            float currentHeight = 0;

            float heading = 0;
            float last = heights.GetAtIndex(0);

            for (int i = 0; i < bufferSize; i++)
            {
                var height = heights.GetAtIndex(i);

                currentHeight += height;

                if (detectSitting)
                {
                    heading += height - last;
                    last = height;
                }
            }
            currentHeight /= bufferSize;

            if (!detectSitting)
            {
                lastStandingHeight = currentHeight;
            }
            else
            {
                if (currentHeight < lastStandingHeight && Mathf.Approximately(heading, 0))
                {
                    settledSeconds++;

                    if (settledSeconds >= secondsToSettle)
                    {
                        OnSittingDetected();
                    }
                }
                else
                {
                    settledSeconds = 0;
                }
            }
        }
    }

    private void OnSittingDetected()
    {
        sittingDetected?.Invoke();

        if (disableOnSittingDetected)
        {
            enabled = false;
        }
    }

    public void DetectSitting()
    {
        detectSitting = true;
        settledSeconds = 0;

        if (objectNeededForSitting && !objectNeededForSitting.IsTracked)
        {
            OnSittingDetected();
            return;
        }
    }
}
