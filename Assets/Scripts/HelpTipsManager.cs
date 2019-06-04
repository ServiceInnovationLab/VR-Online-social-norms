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

    VRTK_ObjectTooltip[] leftTooltips;
    VRTK_ObjectTooltip[] rightTooltips;

    float currentLeftScale = 1.0f;
    float currentRightScale = 1.0f;

    private void Awake()
    {
        tooltipsLeft.touchpadTwoText = "";
        tooltipsLeft.buttonOneText = "";
        tooltipsLeft.startMenuText = "";

        tooltipsRight.touchpadTwoText = "";
        tooltipsRight.buttonOneText = "";
        tooltipsRight.startMenuText = "";

        leftTooltips = tooltipsLeft.GetComponentsInChildren<VRTK_ObjectTooltip>();
        rightTooltips = tooltipsRight.GetComponentsInChildren<VRTK_ObjectTooltip>();

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

    private void ShowHelp(HelpTips tips)
    {
        if (currentLeftScale != tips.leftPositionScale)
        {
            foreach (var tooltip in leftTooltips)
            {
                var newPosition = tooltip.transform.position;
                newPosition.x /= currentLeftScale;
                newPosition.x *= tips.leftPositionScale;
            }
            currentLeftScale = tips.leftPositionScale;
        }

        if (currentRightScale != tips.rightPositionScale)
        {
            foreach (var tooltip in rightTooltips)
            {
                var newPosition = tooltip.transform.position;
                newPosition.x /= currentLeftScale;
                newPosition.x *= tips.leftPositionScale;
            }
            currentRightScale = tips.rightPositionScale;
        }

        tooltipsLeft.triggerText = tips.leftController.triggerText;
        tooltipsLeft.gripText = tips.leftController.gripText;
        tooltipsLeft.touchpadText = tips.leftController.touchpadText;
        tooltipsLeft.buttonTwoText = tips.leftController.buttonText;
        tooltipsLeft.ResetTooltip();

        tooltipsRight.triggerText = tips.rightController.triggerText;
        tooltipsRight.gripText = tips.rightController.gripText;
        tooltipsRight.touchpadText = tips.rightController.touchpadText;
        tooltipsRight.buttonTwoText = tips.rightController.buttonText;
        tooltipsRight.ResetTooltip();

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