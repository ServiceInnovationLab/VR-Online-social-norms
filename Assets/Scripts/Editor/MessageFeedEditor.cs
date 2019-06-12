using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

[CustomEditor(typeof(MessageFeed))]
public class MessageFeedEditor : Editor
{
    Sprite sprite;
    string from;
    string fromTag;
    int startIndex;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MessageFeed feed = Selection.activeObject as MessageFeed;

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

        from = GUILayout.TextArea(from);
        fromTag = GUILayout.TextArea(fromTag);
        sprite = (Sprite)EditorGUILayout.ObjectField(sprite, typeof(Sprite));
        startIndex = EditorGUILayout.IntField(startIndex);

        if (feed && GUILayout.Button("Set Every second"))
        {
            for (int i = startIndex; i < feed.messages.Count; i += 2)
            {
                var message = feed.messages[i];
                message.fromProfile = from;
                message.fromTag = fromTag;
                message.profilePicture = sprite;

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