using System;
using UnityEngine;
using VRTK;

[Flags]
public enum VR_Controller
{
    Left = 1,
    Right = 2
}

public class PeriodicHapticFeedback : MonoBehaviour
{
    [SerializeField] VR_Controller controller;
    [SerializeField] float duration;
    [SerializeField, Range(0, 1)] float stength;
    [SerializeField] float pulseInterval = 0.05f;
    [SerializeField] float timeBetween;

    VRTK_ControllerReference leftController;
    VRTK_ControllerReference rightController;

    float time;
    bool isEnabled;

    public void StartFeedback()
    {
        time = 0;
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
}
