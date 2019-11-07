using UnityEngine;

[System.Serializable]
public class Message 
{
    public string message;
    public OnlineProfile profile;
    public Sprite image;
    public AnimatedImage animatedImage;
    public bool highlight;
    public bool flash;
    public bool pauseHere;
    public string retweetedBy;
    public bool senderSubMessage;
    public bool hateSpeechSubMessage;
    public bool skip;
    public bool startOfSubMessages;
}
