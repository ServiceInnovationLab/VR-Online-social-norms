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
        ShowHelp(!isVisible);
    }

    private void ShowHelp(HelpTips tips)
    {
        tooltipsLeft.triggerText = tips.leftController.triggerText;
        tooltipsLeft.gripText = tips.leftController.gripText;
        tooltipsLeft.touchpadText = tips.leftController.touchpadText;
        tooltipsLeft.buttonTwoText = tips.leftController.buttonText;

        tooltipsRight.triggerText = tips.rightController.triggerText;
        tooltipsRight.gripText = tips.rightController.gripText;
        tooltipsRight.touchpadText = tips.rightController.touchpadText;
        tooltipsRight.buttonTwoText = tips.rightController.buttonText;

        if (tips.helpName != currentHelp)
        {
            currentHelp = tips.helpName;
            //ShowHelp(true);
        }
    }

    public void ShowHelp(bool show)
    {
        isVisible = show;

        tooltipsLeft.gameObject.SetActive(isVisible);
        tooltipsRight.gameObject.SetActive(isVisible);
    }

    public void ShowHelp(string name)
    {
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
            ShowHelp(toShow);
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