using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlaceObjectsOnBorder))]
public class PlaceObjectsOnBorderEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Place now"))
        {
            var targetObject = target as PlaceObjectsOnBorder;
            targetObject.DoPlacement();

            EditorUtility.SetDirty(target);

            EditorUtility.SetDirty(targetObject.left);
            EditorUtility.SetDirty(targetObject.right);
            EditorUtility.SetDirty(targetObject.top);
            EditorUtility.SetDirty(targetObject.bottom);
        }
    }

}

