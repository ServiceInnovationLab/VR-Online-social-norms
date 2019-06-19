using UnityEngine;
using UnityEngine.Video;
using VideoLibrary;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

[RequireComponent(typeof(VideoPlayer))]
public class YoutubeVideoPlayer : MonoBehaviour
{
    [SerializeField] string url;

    [SerializeField] float skipAmount = 0;

    VideoPlayer videoPlayer;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        LoadVideo(url);
    }

    async Task LoadVideo(string url)
    {
        var youTube = YouTube.Default;
        var videos = await youTube.GetAllVideosAsync(url);

        var video = videos.Where(x => x.Format == VideoFormat.Mp4 && x.AudioFormat != AudioFormat.Unknown).OrderByDescending(x => x.Resolution).FirstOrDefault();
        if (video != null)
        {
            videoPlayer.url = video.Uri;
            videoPlayer.time = skipAmount;
        }
    }

}