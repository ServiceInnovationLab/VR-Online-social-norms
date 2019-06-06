using UnityEngine;
using UnityEditor;

public class RePosition : EditorWindow
{
    Vector3 offset;

    [MenuItem("Window/RePosition")]                                            
    static void Awake()
    {
        GetWindow<RePosition>().Show();
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();

        offset = EditorGUILayout.Vector3Field("Offset", offset);

        if (GUILayout.Button("Reposition"))
        {
            DoReposition();
        }

        EditorGUILayout.EndHorizontal();

    }

    void DoReposition()
    {
        Undo.RecordObjects(Selection.transforms, "Reposition");
        foreach (var transform in Selection.transforms)
        {
            transform.localPosition -= offset;
        }
    }

}