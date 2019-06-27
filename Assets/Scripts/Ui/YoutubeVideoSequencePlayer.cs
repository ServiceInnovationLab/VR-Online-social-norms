using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class YoutubeVideoSequencePlayer : MonoBehaviour
{
    [SerializeField] string[] urls;
    [SerializeField] int currentVideo = 0;

    VideoPlayer videoPlayer;

    private void LoadVideo()
    {
        YoutubeVideoPlayer.LoadVideo(videoPlayer, urls[currentVideo]);
    }

    private void Awake()
    {
        currentVideo = Mathf.Clamp(currentVideo, 0, urls.Length);

        videoPlayer = GetComponent<VideoPlayer>();
        LoadVideo();

        videoPlayer.isLooping = false;
        videoPlayer.loopPointReached += VideoPlayer_loopPointReached;
    }

    private void VideoPlayer_loopPointReached(VideoPlayer source)
    {
        currentVideo = (currentVideo + 1) % urls.Length;
        LoadVideo();
    }
}