using VRTK;

/// <summary>
/// A varient of the <see cref="FlashUntilNear"/> which starts flashing again if it is no longer being touched
/// without having been used
/// </summary>
public class FlashUntilUsed : FlashUntilNear
{
    bool hasBeenUsed;

    private void Start()
    {
        interactableObject.InteractableObjectUsed += InteractableObjectUsed;
        interactableObject.InteractableObjectUntouched += InteractableObjectUntouched;
    }

    private void InteractableObjectUntouched(object sender, InteractableObjectEventArgs e)
    {
        if (!hasBeenUsed && interactableObject.isUsable)
        {
            enabled = true;
        }
    }

    private void InteractableObjectUsed(object sender, InteractableObjectEventArgs e)
    {
        hasBeenUsed = true;
    }
}
