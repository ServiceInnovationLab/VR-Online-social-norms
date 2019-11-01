using UnityEngine;
using UnityEngine.Video;
using VideoLibrary;
using System.Linq;

[RequireComponent(typeof(VideoPlayer))]
public class YoutubeVideoPlayer : MonoBehaviour
{
    [SerializeField] bool playOnAwake;
    [SerializeField] string url;
    [SerializeField] float skipAmount = 0;

    VideoPlayer videoPlayer;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        if (playOnAwake)
        {
            Play();
        }
    }

    public void Play()
    {
        PlayVideo(url, skipAmount);
    }

    public void PlayVideo(string url, float skipAmount = 0)
    {
        LoadVideo(videoPlayer, url, skipAmount);
    }

    public static async void LoadVideo(VideoPlayer videoPlayer, string url, float skipAmount = 0)
    {
        var youTube = YouTube.Default;
        var videos = await youTube.GetAllVideosAsync(url);

        var s = videos.Where(x => x.Format == VideoFormat.Mp4 && x.AudioFormat != AudioFormat.Unknown).OrderByDescending(x => x.Resolution).ToArray();

        var video = videos.Where(x => x.Format == VideoFormat.Mp4 && x.AudioFormat != AudioFormat.Unknown).OrderByDescending(x => x.Resolution).FirstOrDefault();
        if (video != null)
        {
            videoPlayer.url = video.Uri;
            videoPlayer.Play();
            videoPlayer.time = skipAmount;
        }
    }

}