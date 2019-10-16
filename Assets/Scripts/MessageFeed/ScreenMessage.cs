using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum MessageTimeFormat
{
    TimeSinceSend,
    TimeSent,
    None
}

public class ScreenMessage : MonoBehaviour
{
    public string from;
    public string fromTag;
    public string message;
    public Sprite profilePicture;
    public Sprite image;
    public bool sent = true;
    public bool moveFromTime = true;
    public bool highlight;
    public bool flash;
    public string retweetedBy;
    public AnimatedImage animatedImage;
    public ScreenMessage subMessage;

    [SerializeField] protected MessageTimeFormat timeFormat = MessageTimeFormat.TimeSinceSend;
    [SerializeField] protected bool showFromTag = true;
    [SerializeField] protected OnlineProfile profile;
    
    [SerializeField] protected Image profilePictureImage;
    [SerializeField] protected Image imageDisplay;
    [SerializeField] protected float moveLeftIfNoImage = 0;
    [SerializeField] protected float increaseWidthIfNoImage;
    [SerializeField] protected bool imageAffectsHeight = true;
    [SerializeField] protected bool limitImageAdjustment = false;
    [SerializeField] protected float highlightAlpha = 0.2f;
    [SerializeField] protected Image highlightImage;
    [SerializeField] bool textCentered = false;
    [SerializeField] protected RectTransform textBackground;
    [SerializeField] protected AnimatedImageDisplay animatedImageDisplay;

    [SerializeField] protected Text messageText;
    [SerializeField] protected Text fromTime;
    [SerializeField] protected Text fromPersonText;

    [SerializeField] protected TextMeshProUGUI messageTextPro;
    [SerializeField] protected TextMeshProUGUI fromTimePro;

    [SerializeField] protected bool resizeBasedOnImage;
    [SerializeField] protected bool resizeBasedOnAnimatedImage;

    [SerializeField] RectTransform retweeted;

    float time = 0;
    protected RectTransform rectTransform;

    public Text MessageTextField
    {
        get { return messageText; }
    }

    public TextMeshProUGUI MessageTextFieldPro
    {
        get { return messageTextPro; }
    }

    public Text UsernameTextField
    {
        get { return fromPersonText; }
    }

    public RectTransform TagAndTimeTextField
    {
        get
        {
            if (fromTime)
            {
                return fromTime.rectTransform;
            }

            if (fromTimePro)
            {
                return fromTimePro.rectTransform;
            }

            return null;
        }
    }

    public RectTransform TextBackground
    {
        get { return textBackground; }
    }

    public bool NeedsAFrame { get { return !Mathf.Approximately(increaseWidthIfNoImage, 0); } }

    protected virtual void Awake()
    {
        rectTransform = (RectTransform)transform;

        if (profile)
        {
            from = profile.username;
            profilePicture = profile.picture;
            fromTag = profile.tag;
        }

        if (fromPersonText)
        {
            fromPersonText.text = from;
        }

        if (messageText)
        {
            messageText.text = message;
        }

        if (messageTextPro)
        {
            messageTextPro.text = message;
        }

        if (retweeted)
        {
            if (string.IsNullOrWhiteSpace(retweetedBy))
            {
                retweeted.gameObject.SetActive(false);
            }
            else
            {
                retweeted.GetComponentInChildren<Text>().text = retweetedBy;
            }
        }

        if (profilePictureImage)
        {
            profilePictureImage.sprite = profilePicture;
        }

        if (fromTime && showFromTag)
        {
            fromTime.text = fromTag;
        }
        else if (fromTimePro && showFromTag)
        {
            fromTimePro.text = fromTag;
        }

        if (fromTime && sent && timeFormat == MessageTimeFormat.TimeSent)
        {
            fromTime.text = "4 Aug, 2:38 PM";
        }

        if ( (timeFormat == MessageTimeFormat.None || timeFormat == MessageTimeFormat.TimeSent) && sent)
        {
            enabled = false;
        }


        if (!fromTime)
        {
            enabled = false;
        }

        SetImage();

        if (highlight)
        {
            if (!highlightImage)
            {
                highlightImage = GetComponent<Image>();
            }

            if (highlightImage)
            {
                var newColour = highlightImage.color;
                newColour.a = highlightAlpha;
                highlightImage.color = newColour;
            }
        }

        if (flash)
        {
            var script = GetComponent<FlashingImage>();
            if (script)
            {
                script.enabled = true;
            }
        }
    }

    private void OnEnable()
    {
        Awake();
    }

    protected void SetImage()
    {
        if (imageDisplay)
        {
            var heightAdjustment = 0f;

            if (image)
            {
                imageDisplay.sprite = image;

                if (resizeBasedOnImage)
                {
                    heightAdjustment = imageDisplay.rectTransform.rect.height - image.rect.height;

                    if (heightAdjustment > 0)
                    {
                        imageDisplay.rectTransform.sizeDelta = new Vector2(imageDisplay.rectTransform.sizeDelta.x, image.rect.height);
                    }
                    else
                    {
                        heightAdjustment = 0;
                    }
                }
            }
            else if (animatedImageDisplay && animatedImage)
            {
                animatedImageDisplay.animatedImage = animatedImage;
                animatedImageDisplay.enabled = true;

                if (resizeBasedOnAnimatedImage)
                {
                    imageDisplay.rectTransform.sizeDelta = new Vector2(imageDisplay.rectTransform.sizeDelta.x, animatedImage.images[0].rect.height);
                    heightAdjustment = imageDisplay.rectTransform.rect.height - animatedImage.images[0].rect.height;
                }
            }
            else
            {
                heightAdjustment = imageDisplay.rectTransform.rect.height;

                imageDisplay.gameObject.SetActive(false);

                if (!Mathf.Approximately(moveLeftIfNoImage, 0))
                {
                    rectTransform.SetLeft(moveLeftIfNoImage);
                }

                if (!Mathf.Approximately(increaseWidthIfNoImage, 0))
                {
                    rectTransform.sizeDelta += new Vector2(increaseWidthIfNoImage, 0);
                    textBackground.sizeDelta += new Vector2(increaseWidthIfNoImage, 0);
                }
            }

            if (imageAffectsHeight && !Mathf.Approximately(heightAdjustment, 0))
            {
                if (limitImageAdjustment && textBackground)
                {
                    float textHeight = Mathf.Max(Mathf.Abs(textBackground.rect.yMin), Mathf.Abs(textBackground.rect.yMax));

                    if (rectTransform.rect.height - heightAdjustment < textHeight + 50)
                    {
                        heightAdjustment = rectTransform.rect.height - textHeight - 80;

                        if (heightAdjustment < 0)
                        {
                            heightAdjustment = 0;
                        }
                    }
                }

                rectTransform.sizeDelta -= new Vector2(0, heightAdjustment);

                if (textCentered)
                {
                    messageText.rectTransform.anchoredPosition -= new Vector2(0, heightAdjustment);
                }
            }
        }
    }

    protected virtual void FixedUpdate()
    {
        if (!sent)
            return;

        time += Time.deltaTime;

        if (time < 1)
        {
            fromTime.text = fromTag + " - < 1s";
        }
        else
        {
            int seconds = Mathf.FloorToInt(time);
            int minutes = seconds / 60;

            if (minutes > 0)
            {
                fromTime.text = fromTag + " - " + minutes + "m";
            }
            else
            {
                fromTime.text = fromTag + " - " + seconds + "s";
            }
        }
    }

    public void Send(InputField input)
    {
        transform.Find("Icons").gameObject.SetActive(true);
        messageText.text = input.text;
        input.gameObject.SetActive(false);

        messageText.gameObject.SetActive(true);

        sent = true;
    }
}