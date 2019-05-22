using UnityEngine;
using VRTK;

/// <summary>
/// This behaviour allows for an application menu to be shown by pressing the application
/// button. Disables teleporting while the menu is open.
/// </summary>
public class ControllerSceneMenu : MonoBehaviour
{
    VRTK_RadialMenu radialMenu;
    VRTK_Pointer pointer;

    private void Start()
    {        
        radialMenu = GetComponentInChildren<VRTK_RadialMenu>();
        if (!radialMenu)
        {
            Debug.Log("This ControllerSceneMenu object doesn't have a radial menu attached!");
            return;
        }

        foreach (var button in radialMenu.buttons)
        {
            button.OnClick.AddListener(Hide);
        }

        var controllerEvents = GetComponentInParent<VRTK_ControllerEvents>();
        pointer = GetComponentInParent<VRTK_Pointer>();

        if (controllerEvents)
        {
            controllerEvents.ButtonTwoPressed += Controller_ButtonTwoPressed;
        }

        Hide();
    }

    private void Controller_ButtonTwoPressed(object sender, ControllerInteractionEventArgs e)
    {
        Show();
    }

    public void Show()
    {
        pointer.enabled = false;
        gameObject.SetActive(true);
        radialMenu.ShowMenu();
    }

    public void Hide()
    {        
        gameObject.SetActive(false);
        pointer.enabled = true;
    }
}
