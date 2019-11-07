using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Linq;
using VRTK;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using TMPro;

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
        var lines = new List<string>();

        var objects = VRTK_SharedMethods.FindEvenInactiveComponentsInValidScenes<Text>(false);

        foreach (var obj in objects)
        {
            if (obj.gameObject.name == "UITextFront"
                || obj.gameObject.name == "UITextReverse"
                || obj.gameObject.name.EndsWith(".")
                || string.IsNullOrWhiteSpace(obj.text)
                || (obj.gameObject.name == "FromText" && obj.GetComponentsInParent<ScreenMessage>(true)?.Length > 0)
                || (obj.gameObject.name == "FromTime" && obj.GetComponentsInParent<ScreenMessage>(true)?.Length > 0)
                || (obj.gameObject.name == "Text" && obj.GetComponentsInParent<ScreenMessage>(true)?.Length > 0)
                || (obj.gameObject.name == "Tag" && obj.GetComponentsInParent<ScreenMessage>(true)?.Length > 0)
                )
                continue;

            lines.Add(obj.gameObject.name + "=" + obj.text.Replace("\r", "").Replace("\n", "\\n"));
        }

        var objects2 = VRTK_SharedMethods.FindEvenInactiveComponentsInValidScenes<TextMeshProUGUI>(false);

        foreach (var obj in objects2)
        {
            if (obj.gameObject.name == "UITextFront"
                || obj.gameObject.name == "UITextReverse"
                || obj.gameObject.name.EndsWith(".")
                || string.IsNullOrWhiteSpace(obj.text)
                || (obj.gameObject.name == "FromText" && obj.GetComponentsInParent<ScreenMessage>(true)?.Length > 0)
                || (obj.gameObject.name == "FromTime" && obj.GetComponentsInParent<ScreenMessage>(true)?.Length > 0)
                || (obj.gameObject.name == "Text" && obj.GetComponentsInParent<ScreenMessage>(true)?.Length > 0)
                || (obj.gameObject.name == "Tag" && obj.GetComponentsInParent<ScreenMessage>(true)?.Length > 0)
                )
                continue;

            lines.Add(obj.gameObject.name + "=" + obj.text.Replace("\r", "").Replace("\n", "\\n"));
        }

        File.WriteAllLines("D:\\" + SceneManager.GetActiveScene().name + ".txt", lines);
    }
}