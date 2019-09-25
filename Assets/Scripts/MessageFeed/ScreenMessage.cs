using UnityEngine;
using UnityEngine.UI;

public enum MessageTimeFormat
{
    TimeSinceSend,
    TimeSent
}

public class ScreenMessage : MonoBehaviour
{
    public string from;
    public string fromTag;
    public string message;
    public Sprite profilePicture;
    public bool sent = true;

    [SerializeField] MessageTimeFormat timeFormat = MessageTimeFormat.TimeSinceSend;
    [SerializeField] bool showFromTag = true;
    [SerializeField] OnlineProfile profile;
    [SerializeField] Text messageText;
    [SerializeField] Image profilePictureImage;
    [SerializeField] Text fromTime;
    [SerializeField] Text fromPersonText;

    [SerializeField] RectTransform textBackground;

    float time = 0;

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

    private void Awake()
    {
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
        }

        if (fromTime && sent && timeFormat == MessageTimeFormat.TimeSent)
        {
            fromTime.text = "4 Aug, 2:38 PM";
            enabled = false;
        }
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