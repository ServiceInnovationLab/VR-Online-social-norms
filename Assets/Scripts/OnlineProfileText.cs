using UnityEngine;
using TMPro;


public enum OnlineProfileTextType
{
    Name,
    Tag
}

public class OnlineProfileText : MonoBehaviour
{
    [SerializeField] string prefix = "";
    [SerializeField] string suffix = "";
    [SerializeField] SocialMediaScenarioTextType profileType;
    [SerializeField] OnlineProfileTextType textType;

    void Start()
    {
        string text = "";

        var profile = SocialMediaScenarioPicker.Instance.CurrentScenario.GetProfile(profileType);

        if (profile != null)
        {
            switch (textType)
            {
                case OnlineProfileTextType.Name:
                    text = profile.username;
                    break;
                case OnlineProfileTextType.Tag:
                    text = profile.tag;
                    break;
                default:
                    text = "<MISSING>";
                    break;
            }
        }

        GetComponent<TextMeshProUGUI>().text = prefix + text + suffix;
    }

}
