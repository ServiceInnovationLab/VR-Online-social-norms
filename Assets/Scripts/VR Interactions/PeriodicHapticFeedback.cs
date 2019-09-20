using System;
using UnityEngine;
using VRTK;

[Flags]
public enum VR_Controller
{
    Left = 1,
    Right = 2,
    Grabbed = 4
}

public class PeriodicHapticFeedback : MonoBehaviour
{
    [SerializeField, EnumFlag] VR_Controller controller;
    [SerializeField] float duration;
    [SerializeField, Range(0, 1)] float stength;
    [SerializeField] float pulseInterval = 0.05f;
    [SerializeField] float timeBetween;

    VRTK_ControllerReference leftController;
    VRTK_ControllerReference rightController;

    VRTK_ControllerReference grabbedController;

    float time;
    bool isEnabled;

    private void Awake()
    {
        var interactableObject = GetComponent<VRTK_InteractableObject>();

        if (interactableObject)
        {
            interactableObject.InteractableObjectGrabbed += InteractableObjectGrabbed;
            interactableObject.InteractableObjectUngrabbed += InteractableObjectUngrabbed;
        }
    }

    private void InteractableObjectUngrabbed(object sender, InteractableObjectEventArgs e)
    {
        if (VRTK_ControllerReference.IsValid(grabbedController))
        {
            StopFeedback();
        }
        grabbedController = null;
    }

    private void InteractableObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        grabbedController = VRTK_ControllerReference.GetControllerReference(e.interactingObject);
    }

    public void StartFeedback()
    {
        time = timeBetween;
        isEnabled = true;
    }

    public void StopFeedback()
    {
        isEnabled = false;

        if (controller.HasFlag(VR_Controller.Left))
        {
            VRTK_ControllerHaptics.CancelHapticPulse(leftController);
        }

        if (controller.HasFlag(VR_Controller.Right))
        {
            VRTK_ControllerHaptics.CancelHapticPulse(rightController);
        }

        if (VRTK_ControllerReference.IsValid(grabbedController))
        {
            VRTK_ControllerHaptics.CancelHapticPulse(grabbedController);
        }
    }

    void FindControllers()
    {
        if (!VRTK_ControllerReference.IsValid(leftController))
        {
            leftController = VRTK_ControllerReference.GetControllerReference(VRTK_DeviceFinder.GetControllerLeftHand());
        }

        if (!VRTK_ControllerReference.IsValid(rightController))
        {
            rightController = VRTK_ControllerReference.GetControllerReference(VRTK_DeviceFinder.GetControllerRightHand(true));
        }
    }

    public void TriggerPulse()
    {
        if (controller.HasFlag(VR_Controller.Grabbed) && VRTK_ControllerReference.IsValid(grabbedController))
        {
            VRTK_ControllerHaptics.TriggerHapticPulse(grabbedController, stength, duration, pulseInterval);
        }
        else
        {
            if (controller.HasFlag(VR_Controller.Left))
            {
                VRTK_ControllerHaptics.TriggerHapticPulse(leftController, stength, duration, pulseInterval);
            }

            if (controller.HasFlag(VR_Controller.Right))
            {
                VRTK_ControllerHaptics.TriggerHapticPulse(rightController, stength, duration, pulseInterval);
            }
        }
    }

    private void FixedUpdate()
    {
        FindControllers();
    }

    void Update()
    {
        if (!isEnabled)
            return;

        time += Time.deltaTime;

        if (time > timeBetween)
        {
            time = -duration;

            TriggerPulse();
        }
    }
}
