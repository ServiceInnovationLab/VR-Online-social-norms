using UnityEngine;
using UnityEngine.UI;

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

        toggle.onValueChanged.AddListener(b =>
        {
            PlayerPrefs.SetInt(TRACKED_CHAIR_PREFERENCE, b ? 1 : 0);
        });
    }

}
