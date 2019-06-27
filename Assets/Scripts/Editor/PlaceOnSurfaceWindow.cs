using UnityEngine;
using UnityEditor;

public class PlaceOnSurfaceWindow : EditorWindow
{
    [MenuItem("Window/Place on Surface")]
    static void Awake()
    {
        GetWindow<PlaceOnSurfaceWindow>("Place on Surface").Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Place using: ", EditorStyles.boldLabel);        

        if (GUILayout.Button("Bottom"))
        {
            PlaceObjects("Bottom");
        }

        if (GUILayout.Button("Origin"))
        {
            PlaceObjects("Origin");
        }

        if (GUILayout.Button("Center"))
        {
            PlaceObjects("Center");
        }        
    }

    void PlaceObjects(string placementPoint)
    {
        Undo.RecordObjects(Selection.transforms, "Place on Surface");

        for (int i = 0; i < Selection.transforms.Length; i++)
        {
            var gameObject = Selection.transforms[i].gameObject;

            Bounds bounds = gameObject.GetComponentInChildren<Renderer>().bounds;

            var savedLayer = gameObject.layer;
            float yOffset = 0;
            
            gameObject.layer = LayerMask.GetMask("Ignore Raycast");

            if (Physics.Raycast(gameObject.transform.position, -Vector3.up, out RaycastHit hit))
            {
                switch (placementPoint)
                {
                    case "Bottom":
                        yOffset = gameObject.transform.position.y - bounds.min.y;
                        break;
                    case "Center":
                        yOffset = bounds.center.y - gameObject.transform.position.y;
                        break;
                }

                gameObject.transform.position = new Vector3(hit.point.x, hit.point.y + yOffset, hit.point.z);
            }

            gameObject.layer = savedLayer;
        }
    }
}