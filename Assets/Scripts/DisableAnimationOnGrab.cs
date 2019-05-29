using System.Collections;
using UnityEngine;
using VRTK;

/// <summary>
/// This behaviour will disable the motion of an animated object when it is picked up
/// </summary>
[RequireComponent(typeof(Rigidbody), typeof(Animator), typeof(VRTK_InteractableObject))]
public class DisableAnimationOnGrab : MonoBehaviour
{
    public bool unlockRotation;

    Rigidbody rigidBody;
    Animator animator;
    VRTK_InteractableObject interactableObject;
    Coroutine onThrownRoutine;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        interactableObject = GetComponent<VRTK_InteractableObject>();
    }

    void OnEnable()
    {
        interactableObject.InteractableObjectGrabbed += InteractableObject_Grabbed;
        interactableObject.InteractableObjectUngrabbed += InteractableObject_Ungrabbed;
    }

    void OnDisable()
    {
        interactableObject.InteractableObjectGrabbed -= InteractableObject_Grabbed;
        interactableObject.InteractableObjectUngrabbed -= InteractableObject_Ungrabbed;
    }

    void InteractableObject_Ungrabbed(object sender, InteractableObjectEventArgs e)
    {
        if (onThrownRoutine != null)
        {
            StopCoroutine(onThrownRoutine);
        }
        onThrownRoutine = StartCoroutine(ResetOnSleep());
    }

    void InteractableObject_Grabbed(object sender, InteractableObjectEventArgs e)
    {
        if (onThrownRoutine != null)
        {
            StopCoroutine(onThrownRoutine);
        }
        animator.applyRootMotion = false;

        if (unlockRotation)
        {
            rigidBody.constraints = RigidbodyConstraints.None;
        }
    }

    IEnumerator ResetOnSleep()
    {
        while (!rigidBody.IsSleeping())
        {
            yield return new WaitForSeconds(0.2f);
        }
        
        animator.applyRootMotion = true;
        if (unlockRotation)
        {
            rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        }

        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
    }

}