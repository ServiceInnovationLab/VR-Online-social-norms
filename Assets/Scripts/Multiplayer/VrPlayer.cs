using Mirror;
using UnityEngine;
using VRTK;

public class VrPlayer : NetworkBehaviour
{
    public static VrPlayer MyPlayer;

    public override void OnStartLocalPlayer()    
    {
        MyPlayer = this;
        foreach (var renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = false;
        }
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        var headset = VRTK_DeviceFinder.HeadsetTransform();

        transform.position = headset.position;
        transform.rotation = headset.rotation;
    }

    [Command]
    public void CmdPickUp(NetworkIdentity toPickUp, NetworkIdentity player)
    {
        toPickUp.RemoveClientAuthority();
        toPickUp.AssignClientAuthority(player.connectionToClient);
        RpcDisablePhysics(toPickUp);
    }

    [Command]
    public void CmdDrop(NetworkIdentity toPickUp)
    {
       // toPickUp.RemoveClientAuthority();
      //  RpcEnablePhysics(toPickUp);
    }

    [ClientRpc]
    void RpcDisablePhysics(NetworkIdentity id)
    {
        id.GetComponent<NetworkTransform>().enabled = true;

        var body = id.GetComponent<Rigidbody>();

        if (body)
        {
            body.isKinematic = !isLocalPlayer;
        }
    }

    [ClientRpc]
    void RpcEnablePhysics(NetworkIdentity id)
    {
        id.GetComponent<NetworkTransform>().enabled = false;

        var body = id.GetComponent<Rigidbody>();

        if (body)
        {
            body.isKinematic = false;
            body.WakeUp();
        }
    }
}
