using UnityEngine;
using VRTK;

/// <summary>
/// This script gives some haptic feedback on every interactable object in the scene.
/// It can be placed on any object in the scene to run. There should only be one of these scripts in a scene.
/// </summary>
public class HapticOnObjectTouch : MonoBehaviour
{
    [Header("Frequency settings")]
    public float amplitude = 1;
    public float duration = 1;
    public float frequency = 150;

    [Header("Fallback settings")]

    [Tooltip("Denotes how strong the rumble in the controller will be on touch.")]
    [Range(0, 1)]
    public float strengthOnTouch = 0;
    [Tooltip("Denotes how long the rumble in the controller will last on touch.")]
    public float durationOnTouch = 0f;
    [Tooltip("Denotes interval betweens rumble in the controller on touch.")]
    public float intervalOnTouch = minInterval;
    [Tooltip("If this is checked then the rumble will be cancelled when the controller is no longer touching.")]
    public bool cancelOnUntouch = true;

    protected const float minInterval = 0.05f;

    /// <summary>
    /// The CancelHaptics method cancels any existing haptic feedback on the given controller.
    /// </summary>
    /// <param name="controllerReference"></param>
    public virtual void CancelHaptics(VRTK_ControllerReference controllerReference)
    {
        VRTK_ControllerHaptics.CancelHapticPulse(controllerReference);
    }

    /// <summary>
    /// The HapticsOnTouch method triggers the haptic feedback on the given controller for the settings associated with touch.
    /// </summary>
    /// <param name="controllerReference">The reference to the controller to activate the haptic feedback on.</param>
    public virtual void HapticsOnTouch(VRTK_ControllerReference controllerReference)
    {
        if (strengthOnTouch > 0 && durationOnTouch > 0f)
        {
            TriggerHapticPulse(controllerReference);
        }
        else
        {
            VRTK_ControllerHaptics.CancelHapticPulse(controllerReference);
        }
    }

    private void Awake()
    {
        if (FindObjectsOfType<HapticOnObjectTouch>().Length > 1)
        {
            Debug.LogError("There should only be one HapticOnObjectTouch in the scene!", gameObject);
        }
    }

    private void Start()
    {
        foreach (var interactable in FindObjectsOfType<VRTK_InteractableObject>())
        {
            interactable.InteractableObjectTouched += TouchHaptics;
            interactable.InteractableObjectUntouched += CancelTouchHaptics;
        }
    }

    protected virtual void TriggerHapticPulse(VRTK_ControllerReference controllerReference)
    {
        if (VRTK_ControllerHaptics.TriggerHapticFrequency(controllerReference, amplitude, duration, frequency))
            return;

        VRTK_ControllerHaptics.TriggerHapticPulse(controllerReference, strengthOnTouch, durationOnTouch, (intervalOnTouch >= minInterval ? intervalOnTouch : minInterval));
    }

    protected virtual void TouchHaptics(object sender, InteractableObjectEventArgs e)
    {
        VRTK_ControllerReference controllerReference = VRTK_ControllerReference.GetControllerReference(e.interactingObject);
        if (VRTK_ControllerReference.IsValid(controllerReference))
        {
            HapticsOnTouch(controllerReference);
        }
    }

    VRTK_ControllerReference CancelOn(GameObject givenObject)
    {
        VRTK_ControllerReference controllerReference = VRTK_ControllerReference.GetControllerReference(givenObject);
        if (VRTK_ControllerReference.IsValid(controllerReference))
        {
            CancelHaptics(controllerReference);
        }

        return controllerReference;
    }

    protected virtual void CancelTouchHaptics(object sender, InteractableObjectEventArgs e)
    {
        if (cancelOnUntouch)
        {
            var controller = CancelOn(e.interactingObject);
        }
    }
}
