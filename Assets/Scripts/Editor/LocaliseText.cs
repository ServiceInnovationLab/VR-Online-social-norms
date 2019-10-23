using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Linq;
using TMPro;
using VRTK;
using System.Collections.Generic;
using System.IO;
using Lean.Localization;

public class LocaliseText : ScriptableObject
{
    [MenuItem("Tools/LocaliseText")]
    static void DoLocaliseText()
    {
        var local = new HashSet<string>();

        foreach (var line in File.ReadAllLines(@"D:\Unity Projects\VR-Empathy\Assets\StreamingAssets\Languages\English.txt"))
        {
            local.Add(line.Split('=')[0]);
        }

        var objects = VRTK_SharedMethods.FindEvenInactiveComponentsInValidScenes<Text>(false);

        foreach (var obj in objects)
        {
            if (!local.Contains(obj.gameObject.name))
                continue;

            var textLocal = obj.GetComponent<LeanLocalizedText>();

            if (!textLocal)
            {
                textLocal = ObjectFactory.AddComponent<LeanLocalizedText>(obj.gameObject); 
            }

            textLocal.FallbackText = obj.text;
            textLocal.TranslationName = obj.gameObject.name;
        }

        var objects2 = VRTK_SharedMethods.FindEvenInactiveComponentsInValidScenes<TextMeshProUGUI>(false);

        foreach (var obj in objects2)
        {
            if (!local.Contains(obj.gameObject.name))
                continue;

            var textLocal = obj.GetComponent<LeanLocalizedTextMeshProUGUI>();

            if (!textLocal)
            {
                textLocal = ObjectFactory.AddComponent<LeanLocalizedTextMeshProUGUI>(obj.gameObject);
            }

            textLocal.FallbackText = obj.text;
            textLocal.TranslationName = obj.gameObject.name;
        }
    }

}