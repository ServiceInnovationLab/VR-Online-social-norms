using UnityEngine;
using VRTK;

[RequireComponent(typeof(VRTK_InteractableObject), typeof(AudioSource))]
public class PlayAudioSourceOnUse : MonoBehaviour
{
    VRTK_InteractableObject interactableObject;
    AudioSource audioSource;

    private void Awake()
    {
        interactableObject = GetComponent<VRTK_InteractableObject>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        interactableObject.InteractableObjectUsed += InteractableObjectUsed;
    }

    private void OnDisable()
    {
        interactableObject.InteractableObjectUsed -= InteractableObjectUsed;
    }

    private void InteractableObjectUsed(object sender, InteractableObjectEventArgs e)
    {
        audioSource.Play();
    }

}
