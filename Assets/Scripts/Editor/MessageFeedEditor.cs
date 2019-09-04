using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

[CustomEditor(typeof(MessageFeed))]
public class MessageFeedEditor : Editor
{
    OnlineProfile onlineProfile;
    int startIndex;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.LabelField("Editing options", EditorStyles.boldLabel);

        MessageFeed feed = Selection.activeObject as MessageFeed;

        onlineProfile = (OnlineProfile)EditorGUILayout.ObjectField(onlineProfile, typeof(OnlineProfile), false);

        if (feed && GUILayout.Button("Append from file"))
        {
            var lines = LoadLines();
            feed.AppendMessages(lines);
            EditorUtility.SetDirty(feed);
        }

        if (feed && GUILayout.Button("Import from file"))
        {
            var lines = LoadLines();
            feed.SetMessages(lines);
            EditorUtility.SetDirty(feed);
        }

        startIndex = EditorGUILayout.IntField(startIndex);

        if (feed && GUILayout.Button("Set Every Second Profile"))
        {
            for (int i = startIndex; i < feed.messages.Count; i += 2)
            {
                var message = feed.messages[i];
                message.profile = onlineProfile;

                feed.messages[i] = message;
            }
            EditorUtility.SetDirty(feed);
        }
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