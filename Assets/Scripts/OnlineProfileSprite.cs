using UnityEngine;
using UnityEngine.UI;

public class OnlineProfileSprite : MonoBehaviour
{
    [SerializeField] SocialMediaScenarioTextType profileType;

    private void Start()
    {
        var profile = SocialMediaScenarioPicker.Instance.CurrentScenario.GetProfile(profileType);

        GetComponent<Image>().sprite = profile.picture;
    }
}
