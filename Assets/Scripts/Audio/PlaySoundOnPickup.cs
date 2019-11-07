using UnityEngine;
using VRTK;

public class PlaySoundOnPickup : MonoBehaviour
{
    [SerializeField] AudioClip soundToPlay;
    [SerializeField] float volume = 0.4f;

    // Use this for initialization
    void Start()
    {
        var interactableObject = GetComponent<VRTK_InteractableObject>();
        interactableObject.InteractableObjectGrabbed += InteractableObjectGrabbed;
    }

    private void InteractableObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        AudioSource.PlayClipAtPoint(soundToPlay, transform.position, volume);
    }
}
