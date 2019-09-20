using UnityEngine;
using UnityEngine.UI;

public class ScreenMessage : MonoBehaviour
{
    public string from;
    public string fromTag;
    public string message;
    public Sprite profilePicture;
    public bool sent = true;

    [SerializeField] OnlineProfile profile;
    [SerializeField] Text messageText;
    [SerializeField] Image profilePictureImage;
    [SerializeField] Text fromTime;
    [SerializeField] Text fromPersonText;

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

    private void Awake()
    {
        if (profile)
        {
            from = profile.username;
            profilePicture = profile.picture;
            fromTag = profile.tag;
        }

        fromPersonText.text = from;
        messageText.text = message;
        profilePictureImage.sprite = profilePicture;
        fromTime.text = fromTag;
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