using UnityEngine;

[CreateAssetMenu(menuName = "Help/ControllerHelpTips")]
public class HelpTips : ScriptableObject
{
    public string helpName;

    public ControllerHelp leftController;
    public ControllerHelp rightController;

    public float leftPositionScale = 1.0f;
    public float rightPositionScale = 1.0f;
}