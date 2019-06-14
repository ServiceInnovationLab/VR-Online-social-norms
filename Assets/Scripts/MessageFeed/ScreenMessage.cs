using UnityEngine;
using UnityEngine.UI;

public class ScreenMessage : MonoBehaviour
{
    public string from;
    public string fromTag;
    public string message;
    public Sprite profilePicture;

    [SerializeField] Text messageText;
    [SerializeField] Image profilePictureImage;
    [SerializeField] Text fromTime;
    [SerializeField] Text fromPersonText;

    float time = 0;

    public Text GetMessageTextField()
    {
        return messageText;
    }

    private void Awake()
    {
        fromPersonText.text = from;
        messageText.text = message;
        profilePictureImage.sprite = profilePicture;
    }

    private void FixedUpdate()
    {
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
}
