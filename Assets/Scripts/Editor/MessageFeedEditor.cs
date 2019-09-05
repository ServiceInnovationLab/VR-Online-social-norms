using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System;

[CustomEditor(typeof(MessageFeed))]
public class MessageFeedEditor : Editor
{
    OnlineProfile onlineProfile;
    int startIndex;

    bool[] fadeStatus;

    public override void OnInspectorGUI()
    {
        MessageFeed feed = Selection.activeObject as MessageFeed;

        if (!feed)
            return;

        Array.Resize(ref fadeStatus, feed.messages.Count);

        EditorGUILayout.LabelField("Messages", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Expand All", GUILayout.Width(100)))
        {
            for (int i = 0; i < fadeStatus.Length; i++)
            {
                fadeStatus[i] = false;
            }
        }

        if (GUILayout.Button("Collapse All", GUILayout.Width(100)))
        {
            for (int i = 0; i < fadeStatus.Length; i++)
            {
                fadeStatus[i] = true;
            }
        }

        GUILayout.EndHorizontal();

        for (int i = 0; i < feed.messages.Count; i++)
        {
            fadeStatus[i] = !EditorGUILayout.BeginFoldoutHeaderGroup(!fadeStatus[i], "Message " + i);

            if (!fadeStatus[i])
            {
                feed.messages[i].message = EditorGUILayout.TextArea(feed.messages[i].message);

                feed.messages[i].profile = (OnlineProfile)EditorGUILayout.ObjectField(feed.messages[i].profile, typeof(OnlineProfile), false);

                EditorGUILayout.Separator();

                GUILayout.BeginHorizontal();

                if (GUILayout.Button("Insert Before", GUILayout.Width(100)))
                {
                    feed.messages.Insert(i, new Message());
                }

                if (GUILayout.Button("Insert After", GUILayout.Width(100)))
                {
                    feed.messages.Insert(i + 1, new Message());
                }

                GUILayout.Space(100);

                if (GUILayout.Button("Delete", GUILayout.Width(100)))
                {
                    feed.messages.RemoveAt(i);
                    i--;
                }

                GUILayout.EndHorizontal();
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        //DrawDefaultInspector();

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

        if (GUILayout.Button("Import from file"))
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