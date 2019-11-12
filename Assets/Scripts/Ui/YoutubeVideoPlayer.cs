using UnityEngine;
using UnityEngine.Video;
using VideoLibrary;
using System.Linq;
using System;
using System.IO;

[RequireComponent(typeof(VideoPlayer))]
public class YoutubeVideoPlayer : MonoBehaviour
{
    [SerializeField] bool playOnAwake;
    [SerializeField] string url;
    [SerializeField] float skipAmount = 0;
    [SerializeField] VideoClip[] projectVideoClips;

    public VideoPlayer VideoPlayer { get; private set; }

    private void Awake()
    {
        VideoPlayer = GetComponent<VideoPlayer>();

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
        LoadVideo(VideoPlayer, url, skipAmount, projectVideoClips);
    }

    public static async void LoadVideo(VideoPlayer videoPlayer, string url, float skipAmount = 0, VideoClip[] projectVideoClips = null)
    {
        var youTube = YouTube.Default;

        try
        {
            var videos = await youTube.GetAllVideosAsync(url);

            var video = videos.Where(x => x.Format == VideoFormat.Mp4 && x.AudioFormat != AudioFormat.Unknown).OrderByDescending(x => x.Resolution).FirstOrDefault();
            if (video != null)
            {
                videoPlayer.url = video.Uri;
                videoPlayer.Play();
                videoPlayer.time = skipAmount;
            }
        }
        catch (ArgumentException)
        {
            // Argument exception comes up if it's not a youtube video URL.

            // See if referencing a built in one
            if (projectVideoClips != null && projectVideoClips.Any(x => x.name == url))
            {
                videoPlayer.clip = projectVideoClips.First(x => x.name == url);
            }
            // See if local file
            else if (File.Exists(url))
            {
                videoPlayer.url = url;
            }
            else if (File.Exists(Path.Combine(Application.streamingAssetsPath, url)))
            {
                videoPlayer.url = Path.Combine(Application.streamingAssetsPath, url);
            }
            else if (File.Exists(Path.Combine(SocialMediaScenarioPicker.CustomScenarioPath, url)))
            {
                videoPlayer.url = Path.Combine(SocialMediaScenarioPicker.CustomScenarioPath, url);
            }
            else
            {
                // Doesn't look to be a file so just run it
                videoPlayer.url = url;
            }

            videoPlayer.Play();
            videoPlayer.time = skipAmount;
        }

    }

}