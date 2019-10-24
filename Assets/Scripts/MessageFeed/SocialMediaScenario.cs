using UnityEngine;
using UnityEngine.Serialization;

public enum SocialMediaScenarioTextType
{
    Sender,
    Receiver,
    Hatespeech
}

public enum SocialMediaScenatioMessageFeedType
{
    None,
    Default,
    Sender,
    Receiver,
    PileOn,
    TwitterWithFriends,
    FourChan
}

public enum SocialMediaScenarioSMStype
{
    Initial,
    Support
}

[CreateAssetMenu(menuName = "SocialMediaScenario")]
public class SocialMediaScenario : ScriptableObject
{
    [FormerlySerializedAs("receiverMessageFeed")]
    public MessageFeed hatespeechMessageFeed;

    public MessageFeed senderMessageFeed;

    public MessageFeed pileOnMessageFeed;

    [FormerlySerializedAs("messengerFeed")]
    public MessageFeed twitterWithFriends;

    [FormerlySerializedAs("smsMessageFeed")]
    public MessageFeed firstSceneSMSFeed;

    [FormerlySerializedAs("supportSmsMessageFeed")]
    public MessageFeed thirdSceneSMSFeed;

    public MessageFeed fourChan;

    public OnlineProfile receiverProfile;

    public OnlineProfile senderProfile;

    public string senderMessage;

    public string receiverMessage;

    public string GetText(SocialMediaScenarioTextType type)
    {
        switch (type)
        {
            case SocialMediaScenarioTextType.Receiver:
                return receiverMessage;

            case SocialMediaScenarioTextType.Sender:
                return senderMessage;
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
                return hatespeechMessageFeed;

            case SocialMediaScenatioMessageFeedType.PileOn:
                return pileOnMessageFeed;

            case SocialMediaScenatioMessageFeedType.TwitterWithFriends:
                return twitterWithFriends;

            case SocialMediaScenatioMessageFeedType.FourChan:
                return fourChan;
        }

        return null;
    }

    public OnlineProfile GetProfile(SocialMediaScenarioTextType type)
    {
        switch (type)
        {
            case SocialMediaScenarioTextType.Sender:
                return senderProfile;

            case SocialMediaScenarioTextType.Receiver:
                return receiverProfile;

            default:
                return null;
        }
    }

    public MessageFeed GetSMSMessageFeed(SocialMediaScenarioSMStype type)
    {
        switch (type)
        {
            case SocialMediaScenarioSMStype.Initial:
                return firstSceneSMSFeed;

            case SocialMediaScenarioSMStype.Support:
                return thirdSceneSMSFeed;
        }

        return null;
    }

    public Message GetHatespeechMessage()
    {
        return hatespeechMessageFeed.messages[0];
    }
}
