using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Collections.Generic;

[CustomEditor(typeof(MessageFeed))]
public class MessageFeedEditor : Editor
{
    OnlineProfile onlineProfile;
    int startIndex;

    List<bool> fadeStatus = new List<bool>();

    OnlineProfile[] allProfiles;
    string[] profileNames;

    private void OnEnable()
    {
        allProfiles = EditorHelper.GetAllInstances<OnlineProfile>();
        profileNames = allProfiles.Select(x => x.name).ToArray();
    }

    public override void OnInspectorGUI()
    {
        MessageFeed feed = Selection.activeObject as MessageFeed;

        if (!feed)
            return;

        if (feed.messages == null)
        {
            feed.messages = new List<Message>();
        }

        for (int i = fadeStatus.Count; i < feed.messages.Count; i++)
        {
            fadeStatus.Add(true);
        }

        EditorGUILayout.LabelField("Messages", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Expand All", GUILayout.Width(100)))
        {
            for (int i = 0; i < fadeStatus.Count; i++)
            {
                fadeStatus[i] = true;
            }
        } 

        if (GUILayout.Button("Collapse All", GUILayout.Width(100)))
        {
            for (int i = 0; i < fadeStatus.Count; i++)
            {
                fadeStatus[i] = false;
            }
        }

        GUILayout.EndHorizontal();

        for (int i = 0; i < feed.messages.Count; i++)
        {
            string details = fadeStatus[i] ? "" : " [" + feed.messages[i].message + "]";
            fadeStatus[i] = EditorGUILayout.BeginFoldoutHeaderGroup(fadeStatus[i], "Message " + (i + 1) + details);

            if (fadeStatus[i])
            {
                var message = feed.messages[i];
                EditMessage(ref message);

                EditorGUILayout.Separator();

                GUILayout.BeginHorizontal();

                if (GUILayout.Button("Insert Before", GUILayout.Width(100)))
                {
                    InsertMessage(feed.messages, i);
                }

                if (GUILayout.Button("Insert After", GUILayout.Width(100)))
                {
                    InsertMessage(feed.messages, i + 1);
                }

                GUILayout.Space(100);

                if (GUILayout.Button("Delete", GUILayout.Width(100)))
                {
                    feed.messages.RemoveAt(i);
                    fadeStatus.RemoveAt(i);
                    i--;
                }

                GUILayout.EndHorizontal();
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        EditorGUILayout.Separator();

        if (GUILayout.Button("Add Message", GUILayout.Width(100)))
        {
            feed.messages.Add(new Message());
        }

        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Editing options", EditorStyles.boldLabel);

        onlineProfile = (OnlineProfile)EditorGUILayout.ObjectField(onlineProfile, typeof(OnlineProfile), false);

        if (GUILayout.Button("Append from file"))
        {
            var lines = LoadLines();
            feed.AppendMessages(lines);
        }

        if (GUILayout.Button("Import from file (removes existing!)"))
        {
            var lines = LoadLines();
            feed.SetMessages(lines);
        }

        startIndex = EditorGUILayout.IntField(startIndex);

        if (GUILayout.Button("Set Every Second Profile"))
        {
            for (int i = startIndex; i < feed.messages.Count; i += 2)
            {
                var message = feed.messages[i];
                message.profile = onlineProfile;

                feed.messages[i] = message;
            }
        }

        EditorUtility.SetDirty(feed);
    }

    private void EditMessage(ref Message message)
    {
        EditorGUILayout.LabelField("Message: ");
        message.message = EditorGUILayout.TextArea(message.message, new GUIStyle(EditorStyles.textArea) { wordWrap = true });

        EditorGUILayout.LabelField("Retweeted by: ");
        message.retweetedBy = EditorGUILayout.TextArea(message.retweetedBy);

        if (!message.animatedImage)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Image: ");
            message.image = (Sprite)EditorGUILayout.ObjectField(message.image, typeof(Sprite), false);
            GUILayout.EndHorizontal();
        }

        if (!message.image)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Animated Image: ");
            message.animatedImage = (AnimatedImage)EditorGUILayout.ObjectField(message.animatedImage, typeof(AnimatedImage), false);
            GUILayout.EndHorizontal();
        }

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Sent Profile: ");

        int currentIndex = allProfiles.IndexOf(message.profile);

        var newIndex = EditorGUILayout.Popup(currentIndex, profileNames);

        if (currentIndex != newIndex)
        {
            message.profile = allProfiles[newIndex];
        }

        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Highlight: ");
        message.highlight = EditorGUILayout.Toggle(message.highlight);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Flash: ");
        message.flash = EditorGUILayout.Toggle(message.flash);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Pause here: ");
        message.pauseHere = EditorGUILayout.Toggle(message.pauseHere);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Sender submessage: ");
        message.senderSubMessage = EditorGUILayout.Toggle(message.senderSubMessage);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Hate speech submessage: ");
        message.hateSpeechSubMessage = EditorGUILayout.Toggle(message.hateSpeechSubMessage);
        GUILayout.EndHorizontal();
    }

    private void InsertMessage(List<Message> messages, int index)
    {
        messages.Insert(index, new Message());
        fadeStatus.Insert(index, true);
    }

    private Message[] LoadLines()
    {
        var path = EditorUtility.OpenFilePanel("Load file", "", "txt");

        if (string.IsNullOrWhiteSpace(path))
        {
            return null;
        }

        return File.ReadAllLines(path).Select(x => new Message() { message = x }).ToArray();
    }
}