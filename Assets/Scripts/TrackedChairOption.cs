using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Defines the option for whether to use the tracked chair or not.
/// 
/// Provides static way to get the setting from anywhere. 
/// Add this component to a Toggle option to have it be used to set the option.
/// </summary>
public class TrackedChairOption : MonoBehaviour
{
    const string TRACKED_CHAIR_PREFERENCE = "TrackedChair";

    public static bool GetValue()
    {
        return PlayerPrefs.GetInt(TRACKED_CHAIR_PREFERENCE, 1) > 0;
    }

    void Start()
    {
        var toggle = GetComponent<Toggle>();

        toggle.isOn = GetValue();

        toggle.onValueChanged.AddListener(isChecked =>
        {
            PlayerPrefs.SetInt(TRACKED_CHAIR_PREFERENCE, isChecked ? 1 : 0);
            PlayerPrefs.Save();
        });
    }

}
