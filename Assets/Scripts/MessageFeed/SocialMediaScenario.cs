using UnityEngine;

public enum SocialMediaScenarioTextType
{
    Sender,
    Receiver,
    Friend
}

public enum SocialMediaScenatioMessageFeedType
{
    Default,
    Sender,
    Receiver,
    PileOn
}

[CreateAssetMenu(menuName = "SocialMediaScenario")]
public class SocialMediaScenario : ScriptableObject
{
    public MessageFeed messageFeed;


    public MessageFeed receiverMessageFeed;

    public MessageFeed senderMessageFeed;

    public MessageFeed pileOnMessageFeed;


    public string senderMessage;

    public string receiverMessage;

    public string friendMessage;

    public string GetText(SocialMediaScenarioTextType type)
    {
        switch (type)
        {
            case SocialMediaScenarioTextType.Receiver:
                return receiverMessage;

            case SocialMediaScenarioTextType.Sender:
                return senderMessage;

            case SocialMediaScenarioTextType.Friend:
                return friendMessage;
        }

        return "Unknown type";
    }

    public MessageFeed GetMessageFeed(SocialMediaScenatioMessageFeedType type)
    {
        switch (type)
        {
            case SocialMediaScenatioMessageFeedType.Sender:
                return senderMessageFeed;

            case SocialMediaScenatioMessageFeedType.Receiver:
                return receiverMessageFeed;

            case SocialMediaScenatioMessageFeedType.PileOn:
                return pileOnMessageFeed;
            default:
                return messageFeed;
        }
    }
}
