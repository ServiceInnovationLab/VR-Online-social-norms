using UnityEngine;
using UnityEditor;

public class FindObjectsWithTagWindow : EditorWindow
{
    string tag = "IncludeTeleport";

   [MenuItem("Window/Find Objects with Tag")]
    static void Awake()
    {
        GetWindow<FindObjectsWithTagWindow>("Find Objects with Tag").Show();
    }

    void OnGUI()
    {
        tag = GUILayout.TextField(tag);

        if (GUILayout.Button("Find"))
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
            Selection.objects = objects;

            foreach (var obj in objects)
            {
                Debug.Log(obj.name, obj);
            }
        }
    }

   
}