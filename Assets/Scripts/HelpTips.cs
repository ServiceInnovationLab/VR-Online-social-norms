using UnityEngine;

[CreateAssetMenu(menuName = "Help/ControllerHelpTips")]
public class HelpTips : ScriptableObject
{
    public string helpName;

    public ControllerHelp leftController;
    public ControllerHelp rightController;
}