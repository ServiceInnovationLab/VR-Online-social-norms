using UnityEngine;
using System.Collections;
using VRTK;
using Valve.VR;

public class TrackedObject : MonoBehaviour
{
    public uint trackerNumber;
    public Transform origin;

    bool isTracked;
    SteamVR_TrackedObject trackedSteamVR;

    private void Awake()
    {
        VRTK_SDKManager.instance.LoadedSetupChanged += LoadedSetupChanged;
    }

    private void LoadedSetupChanged(VRTK_SDKManager sender, VRTK_SDKManager.LoadedSetupChangeEventArgs e)
    {
        StopAllCoroutines();

        if (trackedSteamVR)
        {
            Destroy(trackedSteamVR);
            trackedSteamVR = null;
        }

        if (e.currentSetup == null)
            return;

        if (e.currentSetup.name.Contains("Steam"))
        {
            StartCoroutine(TryAndTrack());
        }
    }

    IEnumerator TryAndTrack()
    {
        while (true)
        {
            yield return null;

            uint index = 0;
            var error = ETrackedPropertyError.TrackedProp_Success;
            int foundCount = 0;
            bool found = false;

            for (uint i = 0; i < 16; i++)
            {
                var result = new System.Text.StringBuilder(64);
                OpenVR.System.GetStringTrackedDeviceProperty(i, ETrackedDeviceProperty.Prop_RenderModelName_String, result, 64, ref error);
                if (result.ToString().Contains("tracker"))
                {
                    if (foundCount != trackerNumber)
                    {
                        foundCount++;
                        continue;
                    }

                    found = true;
                    index = i;
                    break;
                }
            }

            if (found)
            {
                trackedSteamVR = gameObject.AddComponent<SteamVR_TrackedObject>();
                trackedSteamVR.index = (SteamVR_TrackedObject.EIndex)index;

                if (origin)
                {
                    trackedSteamVR.origin = origin;
                }

                yield break;
            }
        }
    }
}
