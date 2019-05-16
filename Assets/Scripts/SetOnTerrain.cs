using UnityEngine;

public class SetOnTerrain : MonoBehaviour
{
    void Start()
    {
        var pos = transform.position;
        pos.y = Terrain.activeTerrain.SampleHeight(pos);
        transform.localPosition = pos;

        Destroy(this);
    }
}

