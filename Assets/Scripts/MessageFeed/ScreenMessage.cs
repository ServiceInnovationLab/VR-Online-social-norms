using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] protected MessageTimeFormat timeFormat = MessageTimeFormat.TimeSinceSend;
    [SerializeField] protected bool showFromTag = true;
    [SerializeField] protected OnlineProfile profile;
    [SerializeField] protected Text messageText;
    [SerializeField] protected Image profilePictureImage;
    [SerializeField] protected Text fromTime;
    [SerializeField] protected Text fromPersonText;
    [SerializeField] protected Image imageDisplay;
    [SerializeField] protected float moveLeftIfNoImage = 0;
    [SerializeField] protected bool imageAffectsHeight = true;
    [SerializeField] protected bool limitImageAdjustment = false;

    [SerializeField] RectTransform textBackground;

    float time = 0;
    RectTransform rectTransform;

    public Text MessageTextField
    {
        get { return messageText; }
    }

    public Text UsernameTextField
    {
        get { return fromPersonText; }
    }

    public Text TagAndTimeTextField
    {
        get { return fromTime; }
    }

    public RectTransform TextBackground
    {
        get { return textBackground; }
    }

    protected virtual void Awake()
    {
        rectTransform = ((RectTransform)transform);

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

        if (profilePictureImage)
        {
            profilePictureImage.sprite = profilePicture;
        }

        if (fromTime && showFromTag)
        {
            fromTime.text = fromTag;

            if (timeFormat == MessageTimeFormat.None && sent)
            {
                enabled = false;
            }
        }

        if (fromTime && sent && timeFormat == MessageTimeFormat.TimeSent)
        {
            fromTime.text = "4 Aug, 2:38 PM";
            enabled = false;
        }


        if (!fromTime)
        {
            enabled = false;
        }

        if (imageDisplay)
        {
            if (image)
            {
                imageDisplay.sprite = image;
            }
            else
            {
                if (imageAffectsHeight)
                {
                    var heightAdjustment = imageDisplay.rectTransform.rect.height;

                    float textHeight = Mathf.Max(Mathf.Abs(textBackground.rect.yMin), Mathf.Abs(textBackground.rect.yMax));

                    if (limitImageAdjustment && rectTransform.rect.height - heightAdjustment < textHeight)
                    {
                        heightAdjustment = rectTransform.rect.height - textHeight - 80;

                        if (heightAdjustment < 0)
                        {
                            heightAdjustment = 0;
                        }
                    }

                    rectTransform.sizeDelta -= new Vector2(0, heightAdjustment);
                }

                imageDisplay.gameObject.SetActive(false);

                if (!Mathf.Approximately(moveLeftIfNoImage, 0))
                {
                    rectTransform.SetLeft(moveLeftIfNoImage);
                }
            }
        }
    }

    private void OnEnable()
    {
        Awake();
    }

    private void FixedUpdate()
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