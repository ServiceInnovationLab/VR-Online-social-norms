using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(MessageFeed))]
public class MessageFeedEditor : Editor
{
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
    }

    private string[] LoadLines()
    {
        var path = EditorUtility.OpenFilePanel("Load file", "", "txt");

        if (string.IsNullOrWhiteSpace(path))
        {
            return null;
        }

        return File.ReadAllLines(path);
    }
}