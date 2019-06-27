using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Provides an ordered list of messages
/// </summary>
[CreateAssetMenu(menuName = "MessageFeed")]
public class MessageFeed : ScriptableObject
{
    public List<Message> messages;

    /// <summary>
    /// Adds new messages to the feed
    /// </summary>
    /// <param name="newMessages">The new messages to add</param>
    public void AppendMessages(Message[] newMessages)
    {
        if (messages == null)
        {
            messages = new List<Message>();
        }

        foreach (var message in newMessages)
        {
            messages.Add(message);
        }
    }

    /// <summary>
    /// Replaces the messages in the feed
    /// </summary>
    /// <param name="newMessages">The messages for the feed to contain</param>
    public void SetMessages(Message[] newMessages)
    {
        messages = new List<Message>(newMessages);
    }
}