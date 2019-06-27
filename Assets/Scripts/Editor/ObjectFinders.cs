using UnityEngine;
using UnityEditor;
using System.Linq;

public class ObjectFinders : ScriptableObject
{
    [MenuItem("Tools/ObjectFind/FrozenBodies")]
    static void FrozenBodies()
    {
        var objects = FindObjectsOfType<Rigidbody>().Where(x => x.constraints == RigidbodyConstraints.FreezeAll).ToArray();
        Selection.objects = objects;

        foreach (var obj in objects)
        {
            Debug.Log(obj.name, obj);
        }
    }
}