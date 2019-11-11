using UnityEngine;
using Mirror;

public class SpawnPoint : NetworkBehaviour
{
    public GameObject prefab;

    public override void OnStartServer()
    {
        GameObject go = Instantiate(prefab, transform.position, Quaternion.identity);
        NetworkServer.Spawn(go);
    }

}
