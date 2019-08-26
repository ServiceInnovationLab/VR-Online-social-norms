using UnityEngine;
using VRTK;

public class WorldScale : MonoBehaviour
{
    public float scale = 1.0f;
    public VRTK_SDKManager sdkManager;

    private void Awake()
    {
        WorldScaleManager.Instance.ChangeWorldScale(this); ;
    }
}
