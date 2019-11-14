using UnityEngine;
using TMPro;

public class YoutubeDisplay : MonoBehaviour
{
    [SerializeField] YoutubeVideoPlayer streamingVideo;
    [SerializeField] YoutubeVideoPlayer talkingVideo;
    [SerializeField] TextMeshProUGUI talkingVideoTitle;

    [SerializeField] RecommendedVideoUI recommendedVideoPrefab;
    [SerializeField] StackChildren stackChildren;

    [SerializeField] bool useYoutube = true;

    private void Awake()
    {
        recommendedVideoPrefab.gameObject.SetActive(false);
    }

    void Start()
    {
        var scenario = SocialMediaScenarioPicker.Instance.CurrentScenario;

        if (useYoutube)
        {
            streamingVideo.PlayVideo(scenario.senderStreamingURL, scenario.senderStreamingSkipTime);
            talkingVideo.PlayVideo(scenario.senderYoutubeURL);

            talkingVideo.VideoPlayer.isLooping = scenario.senderYoutubeLoop;
        }

        talkingVideoTitle.text = scenario.senderYoutubeVideoTitle;

        foreach (var video in scenario.recommendedVideos)
        {
            var videoDisplay = Instantiate(recommendedVideoPrefab, stackChildren.transform);

            videoDisplay.SetData(video);
            videoDisplay.gameObject.SetActive(true);
        }

        stackChildren.Resize();
    }

}
