using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using VRTK;
using Valve.VR;

public class RotateBasedOnStartingSide : MonoBehaviour
{
    [SerializeField] UnityEvent beforeCheck;
    [SerializeField] UnityEvent afterCheck;
    [SerializeField] VRTK_SDKManager sdkManager;
    [SerializeField] VR_StartingPoint startingPoint;

    HeadsetEat headsetEat;

    private void Awake()
    {
        headsetEat = FindObjectOfType<HeadsetEat>();

        headsetEat.enabled = false;

        sdkManager.LoadedSetupChanged += SdkManager_LoadedSetupChanged;
    }

    private void SdkManager_LoadedSetupChanged(VRTK_SDKManager sender, VRTK_SDKManager.LoadedSetupChangeEventArgs e)
    {
        if (e.currentSetup == null)
            return;

        if (e.currentSetup.name.Contains("Steam"))
        {
            StartCoroutine(CorrectStartingAreaSteamVR());
        }
        else
        {
            headsetEat.enabled = true;

            if (startingPoint)
            {
                startingPoint.DoTeleport();
            }
        }
    }

    private IEnumerator CorrectStartingAreaSteamVR()
    {
        yield return null;
        var rect = new HmdQuad_t();        

        if (!SteamVR_PlayArea.GetBounds(SteamVR_PlayArea.Size.Calibrated, ref rect))
        {
            Debug.LogError("Could not get the bounds of the play area!");
            headsetEat.enabled = true;
            yield break;
        }

        float minX = Mathf.Min(rect.vCorners0.v0, rect.vCorners1.v0, rect.vCorners2.v0, rect.vCorners3.v0);
        float maxX = Mathf.Max(rect.vCorners0.v0, rect.vCorners1.v0, rect.vCorners2.v0, rect.vCorners3.v0);
        float minZ = Mathf.Min(rect.vCorners0.v2, rect.vCorners1.v2, rect.vCorners2.v2, rect.vCorners3.v2);
        float maxZ = Mathf.Max(rect.vCorners0.v2, rect.vCorners1.v2, rect.vCorners2.v2, rect.vCorners3.v2);

        float centreX = (maxX + minX) / 2;

        var playerLocation = VRTK_DeviceFinder.HeadsetTransform().transform.position - sdkManager.transform.position;

        beforeCheck?.Invoke();

        if (playerLocation.x < centreX)
        {
            sdkManager.transform.rotation = Quaternion.Euler(0, 180, 0);
            Debug.Log("Rotating play area!");
        }

        if (startingPoint)
        {
            startingPoint.DoTeleport();
        }

        afterCheck?.Invoke();
        headsetEat.enabled = true;
    }
}
