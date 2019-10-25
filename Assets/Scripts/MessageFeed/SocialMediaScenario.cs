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
    SenderTwitterFeed,
    ReceiverHatespeech,
    ReceiverPileOn,
    TwitterWithFriends,
    SenderFourChan
}

public enum SocialMediaScenarioSMStype
{
    First,
    Second
}

[CreateAssetMenu(menuName = "SocialMediaScenario")]
public class SocialMediaScenario : ScriptableObject
{
    [FormerlySerializedAs("receiverMessageFeed")]
    public MessageFeed hatespeechMessageFeed;

    [FormerlySerializedAs("senderMessageFeed")]
    public MessageFeed senderTwitterFeed;

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
            case SocialMediaScenatioMessageFeedType.SenderTwitterFeed:
                return senderTwitterFeed;

            case SocialMediaScenatioMessageFeedType.ReceiverHatespeech:
                return hatespeechMessageFeed;

            case SocialMediaScenatioMessageFeedType.ReceiverPileOn:
                return pileOnMessageFeed;

            case SocialMediaScenatioMessageFeedType.TwitterWithFriends:
                return twitterWithFriends;

            case SocialMediaScenatioMessageFeedType.SenderFourChan:
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

            case SocialMediaScenarioTextType.Hatespeech:
                return hatespeechMessageFeed.messages[0].profile;

            default:
                return null;
        }
    }

    public MessageFeed GetSMSMessageFeed(SocialMediaScenarioSMStype type)
    {
        switch (type)
        {
            case SocialMediaScenarioSMStype.First:
                return firstSceneSMSFeed;

            case SocialMediaScenarioSMStype.Second:
                return thirdSceneSMSFeed;
        }

        return null;
    }

    public Message GetHatespeechMessage()
    {
        return hatespeechMessageFeed.messages[0];
    }
}
