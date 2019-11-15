using VRTK;
using Mirror;
using UnityEngine;


/// <summary>
/// Helps with objects that are picked up and thrown.
/// 
/// To make things easier, items that are picked up become owned by the client to make every other client match up with what's happening here. 
/// As this is just trying things out this is fine.
/// </summary>
public class NetworkedInteractGrab : NetworkBehaviour
{

    private void Awake()
    {
        GetComponent<NetworkTransform>().enabled = false;
    }

    void Start()
    {
        var interactableObject = GetComponent<VRTK_InteractableObject>();
        interactableObject.InteractableObjectGrabbed += NetworkedInteractGrab_InteractableObjectGrabbed;

        interactableObject.InteractableObjectUngrabbed += InteractableObject_InteractableObjectUngrabbed;
    }

    private void InteractableObject_InteractableObjectUngrabbed(object sender, InteractableObjectEventArgs e)
    {
        var id = GetComponent<NetworkIdentity>();

        VrPlayer.MyPlayer.CmdDrop(id);
    }

    private void NetworkedInteractGrab_InteractableObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        var id = GetComponent<NetworkIdentity>();

        VrPlayer.MyPlayer.CmdPickUp(id, VrPlayer.MyPlayer.netIdentity);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If someone throws this object and it touches something, make those objects follow what happened here
        if (!hasAuthority)
            return;

        var id = collision.gameObject.GetComponent<NetworkIdentity>();

        if (id)
        {
            VrPlayer.MyPlayer.CmdPickUp(id, VrPlayer.MyPlayer.netIdentity);
        }
    }

}
