using UnityEngine;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public struct OnlineScenarioData
{
    public MessageFeedData MessageFeedData;

    public string SenderMessage;

    public string ReceiverMessage;

    public SocialMediaScenario ToSocialMediaScenario()
    {
        var result = new SocialMediaScenario();

        result.senderMessage = SenderMessage;
        result.receiverMessage = ReceiverMessage;

        var profiles = ConstructProfiles();
        

        var messages = new MessageFeed();
        messages.messages = new List<Message>();

        foreach (var message in MessageFeedData.MessageStream)
        {
            OnlineProfile profile = null;
            if (message.ProfileIndex >= 0 && message.ProfileIndex < profiles.Count)
            {
                profile = profiles[message.ProfileIndex];
            }
            messages.messages.Add(new Message() { message = message.Message, profile = profile });
        }

        result.messageFeed = messages;

        return result;
    }

    private List<OnlineProfile> ConstructProfiles()
    {
        var profiles = new List<OnlineProfile>();
        foreach (var profile in MessageFeedData.Profiles)
        {
            Sprite sprite = null;

            var picturePath = Path.Combine(SocialMediaScenarioPicker.CustomScenarioProfilePicturePath, profile.picture);
            if (File.Exists(picturePath))
            {
                var data = File.ReadAllBytes(Path.Combine(SocialMediaScenarioPicker.CustomScenarioProfilePicturePath, profile.picture));

                var texture = new Texture2D(1, 1, TextureFormat.ARGB32, false, true);
                texture.alphaIsTransparency = true;
                texture.LoadImage(data);                

                var rect = new Rect(0, 0, texture.width, texture.height);

                sprite = Sprite.Create(texture, rect, rect.size / 2);
            }

            profiles.Add(new OnlineProfile() { picture = sprite, username = profile.username, tag = profile.tag });
        }

        return profiles;
    }
}
