using UnityEngine;
using VRTK;
using Valve.VR;
using System.Collections;

public class OrientWorldForPlayer : MonoBehaviour
{
    [SerializeField] VRTK_SDKManager sdkManager;

    private void Awake()
    {
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
    }

    private IEnumerator CorrectStartingAreaSteamVR()
    {
        yield return null;
        var rect = new HmdQuad_t();

        if (!SteamVR_PlayArea.GetBounds(SteamVR_PlayArea.Size.Calibrated, ref rect))
        {
            Debug.LogError("Could not get the bounds of the play area!");
            yield break;
        }

        float minX = Mathf.Min(rect.vCorners0.v0, rect.vCorners1.v0, rect.vCorners2.v0, rect.vCorners3.v0);
        float maxX = Mathf.Max(rect.vCorners0.v0, rect.vCorners1.v0, rect.vCorners2.v0, rect.vCorners3.v0);
        float minZ = Mathf.Min(rect.vCorners0.v2, rect.vCorners1.v2, rect.vCorners2.v2, rect.vCorners3.v2);
        float maxZ = Mathf.Max(rect.vCorners0.v2, rect.vCorners1.v2, rect.vCorners2.v2, rect.vCorners3.v2);

        var playerLocation = VRTK_DeviceFinder.HeadsetTransform().transform.position - sdkManager.transform.position;
        var sdkPosition = sdkManager.transform.position;

        if (playerLocation.XZ().magnitude < 0.5f)
        {
            sdkPosition += new Vector3(Mathf.Abs(playerLocation.x), 0, -Mathf.Abs(playerLocation.z));
        }

        if (playerLocation.x >= 0)
        {
            if (playerLocation.z > 0)
            {
                sdkPosition.z -= playerLocation.z;
            }
            else
            {
                // Totally fine!
            }
        }
        else
        {
            sdkPosition.x -= playerLocation.x;

            if (playerLocation.z > 0)
            {
                sdkPosition.z -= playerLocation.z;
            }
            else
            {
                // Totally fine!
            }
        }

        sdkManager.transform.position = sdkPosition;
    }
}
