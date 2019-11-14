using Mirror;
using UnityEngine;
using VRTK;

public class VrPlayer : NetworkBehaviour
{
    public static VrPlayer MyPlayer;

    [SerializeField] Transform rightHand;
    [SerializeField] Transform leftHand;

    public override void OnStartLocalPlayer()    
    {
        MyPlayer = this;
        foreach (var renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = false;
        }

        /*if (!Valve.VR.SteamVR.active)
        {
            QualitySettings.vSyncCount = 1;
        }*/
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        var headset = VRTK_DeviceFinder.HeadsetTransform();
        var rightController = VRTK_DeviceFinder.GetControllerRightHand(true);
        var leftController = VRTK_DeviceFinder.GetControllerLeftHand(true);

        transform.position = headset.position;
        transform.rotation = headset.rotation;

        rightHand.position = rightController.transform.position;
        leftHand.position = leftController.transform.position;
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
        // Don't do this, otherwise the server doesn't have the physics of the throw


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
