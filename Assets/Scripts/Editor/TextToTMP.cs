using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Linq;
using TMPro;
using System.Collections.Generic;
using Lean.Localization;

public class TextToTMP : ScriptableObject
{
    [MenuItem("Tools/TextToTMP")]
    static void DoTextToTMP()
    {
        foreach (var obj in Selection.transforms)
        {
            Undo.RecordObject(obj.gameObject, "Convert to TMP");

            var text = obj.GetComponent<Text>();
            if (text)
            {
                var size = text.rectTransform.sizeDelta;

                var color = text.color;
                var textString = text.text;
                var fontSize = text.fontSize;
                var lineSpacing = text.lineSpacing;
                var fontStyle = (FontStyles)text.fontStyle;

                DestroyImmediate(text);


                var textPro = ObjectFactory.AddComponent<TextMeshProUGUI>(obj.gameObject);

                textPro.color = color;
                textPro.text = textString;
                textPro.fontSize = fontSize;
                textPro.lineSpacing = lineSpacing;
                textPro.fontStyle = fontStyle;
                textPro.overflowMode = TextOverflowModes.Truncate;
                ((RectTransform)obj).sizeDelta = size;
            }
        }
    }

    [MenuItem("Tools/FontFind")]
    static void LocateFonts()
    {
        var set = new HashSet<Font>();

        foreach (var item in FindObjectsOfType<Text>())
        {
            if (item.font.name.Contains("Bold"))
            {
                Debug.Log(item.gameObject.name, item.gameObject);
            }
            set.Add(item.font);
        }

        foreach (var font in set)
        {
            Debug.Log(font.name, font);
        }

        var set2 = new HashSet<TMP_FontAsset>();

        foreach (var item in FindObjectsOfType<TextMeshProUGUI>())
        {
            set2.Add(item.font);
        }

        foreach (var font in set2)
        {
            Debug.Log(font.name, font);
        }
    }

    [MenuItem("Tools/LocaliseFonts")]
    static void SetUpLocalisedFonts()
    {
        foreach (var item in FindObjectsOfType<Text>())
        {
            if (item.GetComponent<LeanLocalizedTextFont>())
                continue;

            Undo.RecordObject(item.gameObject, "Add LeanLocalizedTextFont");

            var localizedTextFont = item.gameObject.AddComponent<LeanLocalizedTextFont>();

            localizedTextFont.FallbackFont = item.font;
            localizedTextFont.TranslationName = "NormalFont";
        }

        foreach (var item in FindObjectsOfType<TextMeshProUGUI>())
        {
            if (item.GetComponent<LeanLocalizedTextMeshProUGUIFont>())
                continue;

            Undo.RecordObject(item.gameObject, "Add LeanLocalizedTextMeshProUGUIFont");

            var localizedTextFont = item.gameObject.AddComponent<LeanLocalizedTextMeshProUGUIFont>();

            localizedTextFont.FallbackFont = item.font;

            var fontName = item.font.name.ToLower();

            if (fontName.Contains("light"))
            {
                localizedTextFont.TranslationName = "LightSdfFont";
            }
            else if (fontName.Contains("medium"))
            {
                localizedTextFont.TranslationName = "MediumSdfFont";
            }
            else
            {
                localizedTextFont.TranslationName = "NormalSdfFont";
            }
        }
    }

}