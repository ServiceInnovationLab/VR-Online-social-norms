using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RecommendedVideoUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI fromText;
    [SerializeField] TextMeshProUGUI viewsText;
    [SerializeField] Image image;

    public void SetData(RecommendedVideo recommendedVideo)
    {
        image.sprite = recommendedVideo.picture;
        titleText.text = recommendedVideo.title;
        fromText.text = recommendedVideo.from;
        viewsText.text = recommendedVideo.views;
    }
    
}

