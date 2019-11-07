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

            Undo.RecordObjects(new[] { targetObject.left.transform, targetObject.right.transform, targetObject.top.transform, targetObject.bottom.transform }, "Place Objects");


            targetObject.DoPlacement();

            EditorUtility.SetDirty(targetObject.left);
            EditorUtility.SetDirty(targetObject.right);
            EditorUtility.SetDirty(targetObject.top);
            EditorUtility.SetDirty(targetObject.bottom);
        }
    }

}

