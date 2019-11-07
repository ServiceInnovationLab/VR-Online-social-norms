using UnityEngine;
using VRTK;

public class HelpTipsManager : MonoBehaviour
{
    [SerializeField] VRTK_ControllerEvents controllerEventsLeft;
    [SerializeField] VRTK_ControllerTooltips tooltipsLeft;

    [SerializeField] VRTK_ControllerEvents controllerEventsRight;
    [SerializeField] VRTK_ControllerTooltips tooltipsRight;

    [SerializeField] HelpTips[] tips;

    bool isVisible;
    string currentHelp;

    private void Awake()
    {
        tooltipsLeft.touchpadTwoText = "";
        tooltipsLeft.buttonOneText = "";
        tooltipsLeft.startMenuText = "";

        tooltipsRight.touchpadTwoText = "";
        tooltipsRight.buttonOneText = "";
        tooltipsRight.startMenuText = "";

        if (tips.Length > 0)
        {
            ShowHelp(tips[0]);
        }
    }

    private void OnEnable()
    {
        controllerEventsLeft.ButtonTwoPressed += Controller_ButtonTwoPressed;
        controllerEventsRight.ButtonTwoPressed += Controller_ButtonTwoPressed;
    }

    private void OnDisable()
    {
        controllerEventsLeft.ButtonTwoPressed -= Controller_ButtonTwoPressed;
        controllerEventsRight.ButtonTwoPressed -= Controller_ButtonTwoPressed;
    }

    private void Controller_ButtonTwoPressed(object sender, ControllerInteractionEventArgs e)
    {
        ShowHelp(!tooltipsLeft.gameObject.activeInHierarchy);
    }

    private void ShowHelp(HelpTips tips, bool swapLeftAndRight = false)
    {
        if (!enabled)
            return;

        var leftController = swapLeftAndRight ? tips.rightController : tips.leftController;
        var rightController = swapLeftAndRight ? tips.leftController : tips.rightController;

        tooltipsLeft.triggerText = leftController.triggerText;
        tooltipsLeft.gripText = leftController.gripText;
        tooltipsLeft.touchpadText = leftController.touchpadText;
        tooltipsLeft.buttonTwoText = leftController.buttonText;
        tooltipsLeft.ResetTooltip();

        tooltipsRight.triggerText = rightController.triggerText;
        tooltipsRight.gripText = rightController.gripText;
        tooltipsRight.touchpadText = rightController.touchpadText;
        tooltipsRight.buttonTwoText = rightController.buttonText;
        tooltipsRight.ResetTooltip();

        tooltipsLeft.transform.localPosition = new Vector3(tips.leftPosition, 0, 0);
        tooltipsRight.transform.localPosition = new Vector3(tips.rightPosition, 0, 0);

        if (tips.helpName != currentHelp)
        {
            currentHelp = tips.helpName;
            //ShowHelp(true);
        }
    }

    public void ShowHelp(bool show)
    {
        if (!enabled && show)
            return;

        isVisible = show;

        tooltipsLeft.gameObject.SetActive(isVisible);
        tooltipsRight.gameObject.SetActive(isVisible);
    }

    public void ShowHelp(string name, bool swapLeftAndRight = false)
    {
        if (!enabled)
            return;

        HelpTips toShow = null;
        foreach (var tip in tips)
        {
            if (tip.helpName == name)
            {
                toShow = tip;
                break;
            }
        }

        if (toShow)
        {
            ShowHelp(toShow, swapLeftAndRight);
        }
        else
        {
            Debug.LogError("Help text not found!");
        }
    }

    public string GetCurrentHelp()
    {
        return currentHelp;
    }
}