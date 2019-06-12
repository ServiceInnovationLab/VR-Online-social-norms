using UnityEngine;

[CreateAssetMenu(menuName = "Help/ControllerHelpTips")]
public class HelpTips : ScriptableObject
{
    public string helpName;

    public ControllerHelp leftController;
    public ControllerHelp rightController;

    public float leftPosition = 0.0f;
    public float rightPosition = 0.0f;
}