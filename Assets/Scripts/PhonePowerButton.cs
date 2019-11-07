using UnityEngine;
using VRTK;

[RequireComponent(typeof(VRTK_InteractableObject))]
public class PhonePowerButton : MonoBehaviour
{
    [SerializeField] GameObject disableOnTurnOff;
    [SerializeField] float requiredHeldTime = 2.0f;
    [SerializeField] VRTK_ControllerEvents.ButtonAlias button;

    VRTK_InteractableObject phone;
    VRTK_ControllerEvents controllerEvents;
    float time = 0;

    private void Awake()
    {
        phone = GetComponent<VRTK_InteractableObject>();

        phone.InteractableObjectGrabbed += InteractableObjectGrabbed;
        phone.InteractableObjectUngrabbed += InteractableObjectUngrabbed;
    }

    private void InteractableObjectUngrabbed(object sender, InteractableObjectEventArgs e)
    {
        if (!controllerEvents)
        {
            return;
        }

        controllerEvents = null;
    }

    private void InteractableObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        controllerEvents = e.interactingObject.GetComponent<VRTK_ControllerEvents>();
    }


    void Update()
    {
        if (!phone.IsGrabbed())
            return;

        if (controllerEvents.IsButtonPressed(button))
        {
            time += Time.deltaTime;

            if (time >= requiredHeldTime)
            {
                disableOnTurnOff.SetActive(false);
            }
        }
        else
        {
            time = 0;
        }
    }
}
