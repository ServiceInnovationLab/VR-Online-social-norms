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
                feed.messages[i].message = EditorGUILayout.TextArea(feed.messages[i].message);

                feed.messages[i].image = (Sprite)EditorGUILayout.ObjectField(feed.messages[i].image, typeof(Sprite), false);

                feed.messages[i].animatedImage = (AnimatedImage)EditorGUILayout.ObjectField(feed.messages[i].animatedImage, typeof(AnimatedImage), false);

                int currentIndex = allProfiles.IndexOf(feed.messages[i].profile);

                var newIndex = EditorGUILayout.Popup(currentIndex, profileNames);

                if (currentIndex != newIndex)
                {
                    feed.messages[i].profile = allProfiles[newIndex];
                }

                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Highlight: ");
                feed.messages[i].highlight = EditorGUILayout.Toggle(feed.messages[i].highlight);
                GUILayout.EndHorizontal();

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