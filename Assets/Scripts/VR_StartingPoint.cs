using UnityEngine;
using VRTK;
using System.Linq;

public class VR_StartingPoint : MonoBehaviour
{
    [SerializeField] VRTK_SDKManager manager;
    [SerializeField] VRTK_BasicTeleport teleport;

    private void Awake()
    {
        manager.LoadedSetupChanged += LoadedSetupChanged;
    }

    private void LoadedSetupChanged(VRTK_SDKManager sender, VRTK_SDKManager.LoadedSetupChangeEventArgs e)
    {
        if (e.currentSetup == null)
            return;       

        teleport.skipBlink = true;
        teleport.Teleport(transform, transform.position);
        teleport.skipBlink = false;

        // TODO: Move the play area to be better...

        var areaVeritices = e.currentSetup.boundariesSDK.GetPlayAreaVertices();
        
        var allZValues = areaVeritices.Select(v => v.z).ToArray();
        var allXValues = areaVeritices.Select(v => v.x).ToArray();

        float minX = Mathf.Min(allXValues);
        float maxX = Mathf.Max(allXValues);
        float minZ = Mathf.Min(allZValues);
        float maxZ = Mathf.Max(allZValues);

        float centreX = (maxX + minX) / 2;
        float centreZ = (maxZ + minZ) / 2;        


        Destroy(gameObject);
    }
}
