using UnityEngine;

public enum SocialMediaScenarioTextType
{
    Sender,
    Receiver
}

[CreateAssetMenu(menuName = "SocialMediaScenario")]
public class SocialMediaScenario : ScriptableObject
{
    public MessageFeed messageFeed;

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
}
