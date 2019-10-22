using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Linq;
using VRTK;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;

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

    [MenuItem("Tools/ObjectFind/Text")]
    static void Text()
    {
        var objects = VRTK_SharedMethods.FindEvenInactiveComponentsInValidScenes<Text>(false);

        foreach (var obj in objects)
        {
            Debug.Log(obj.name, obj);
        }
    }
    [MenuItem("Tools/ObjectFind/TextToCSV")]
    static void TextToCSV()
    {
        var objects = VRTK_SharedMethods.FindEvenInactiveComponentsInValidScenes<Text>(false);

        var lines = new List<string>();

        foreach (var obj in objects)
        {
            lines.Add(obj.gameObject.name + "=" + obj.text.Replace("\r", "").Replace("\n", "\\n"));
        }

        File.WriteAllLines("D:\\" + SceneManager.GetActiveScene().name + ".txt", lines);
    }
}