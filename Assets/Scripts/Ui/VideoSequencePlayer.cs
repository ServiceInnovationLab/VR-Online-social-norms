using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;

/// <summary>
/// Plays a variety of videos
/// </summary>
[RequireComponent(typeof(VideoPlayer))]
public class VideoSequencePlayer : MonoBehaviour
{
    [SerializeField] VideoClip[] videos;
    [SerializeField] bool randomOrder;
    [SerializeField] int currentVideo = 0;
    [SerializeField] float delayBetweenVideos;
    [SerializeField] Image faderImage;
    [SerializeField] float fadeTime = 0.25f;

    VideoPlayer videoPlayer;
    Color blackColour = new Color(0, 0, 0, 1);

    public IEnumerator ChangeToNextVideo()
    {
        float time = 0;

        while (time < fadeTime)
        {
            yield return null;
            time += Time.deltaTime;

            blackColour.a = (time / fadeTime) * 0.5f;
            faderImage.color = blackColour;
        }

        if (delayBetweenVideos > 0)
        {
            yield return new WaitForSeconds(delayBetweenVideos);
        }

        if (randomOrder)
        {
            currentVideo = Random.Range(0, videos.Length);
        }
        else
        {
            currentVideo = (currentVideo + 1) % videos.Length;
        }

        videoPlayer.clip = videos[currentVideo];
        videoPlayer.Play();

        time = 0;

        while (time < fadeTime)
        {
            yield return null;
            time += Time.deltaTime;

            blackColour.a = 0.5f - (time / fadeTime) * 0.5f;
            faderImage.color = blackColour;
        }

        blackColour.a = 0;
        faderImage.color = blackColour;
    }

    private void Awake()
    {
        if (videos == null || videos.Length == 0)
            return;

        blackColour.a = 0;
        faderImage.color = blackColour;

        currentVideo = Mathf.Clamp(currentVideo, 0, videos.Length);

        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.clip = videos[currentVideo];
        
        videoPlayer.isLooping = false;
        videoPlayer.loopPointReached += VideoPlayer_loopPointReached;
    }

    private void VideoPlayer_loopPointReached(VideoPlayer source)
    {
        StartCoroutine(ChangeToNextVideo());
    }

}