using UnityEngine;
using VRTK;

public class PositionBasedOnTouchpad : MonoBehaviour
{
    [SerializeField] float scaleX = 1.0f;
    [SerializeField] float scaleZ = 1.0f;

    VRTK_ControllerEvents controllerEvents;
    new Renderer renderer;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        var interactable = GetComponentInParent<VRTK_InteractableObject>();

        interactable.InteractableObjectGrabbed += InteractableObjectGrabbed;
        interactable.InteractableObjectUngrabbed += InteractableObjectUngrabbed;
    }

    private void InteractableObjectUngrabbed(object sender, InteractableObjectEventArgs e)
    {
        controllerEvents = null;
    }

    private void InteractableObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        controllerEvents = e.interactingObject.GetComponent<VRTK_ControllerEvents>();
    }

    void Update()
    {
        if (controllerEvents)
        {
            bool isTouched = controllerEvents.IsButtonPressed(VRTK_ControllerEvents.ButtonAlias.TouchpadTouch) ||
                controllerEvents.IsButtonPressed(VRTK_ControllerEvents.ButtonAlias.TouchpadPress);

            renderer.enabled = isTouched;

            if (isTouched)
            {
                var currentAxis = controllerEvents.GetTouchpadAxis();
                transform.localPosition.Set(currentAxis.x * scaleX, transform.localPosition.y, currentAxis.y * scaleZ);
            }
        }
    }
}
