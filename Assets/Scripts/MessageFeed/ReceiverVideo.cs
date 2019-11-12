using UnityEngine;

public class ReceiverVideo : MonoBehaviour
{
    [SerializeField] YoutubeVideoPlayer videoPlayer;

    void Start()
    {
        var scenario = SocialMediaScenarioPicker.Instance.CurrentScenario;

        if (!string.IsNullOrWhiteSpace(scenario.receiverVideo))
        {
            videoPlayer.PlayVideo(scenario.receiverVideo, scenario.receiverVideoSkipTime);
        }
    }
}
