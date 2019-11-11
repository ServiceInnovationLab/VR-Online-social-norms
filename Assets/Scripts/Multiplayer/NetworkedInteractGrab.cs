using VRTK;
using Mirror;

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

}
