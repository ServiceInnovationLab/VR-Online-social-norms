using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Linq;
using TMPro;

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
                textPro.fontStyle = (FontStyles)fontStyle;
                textPro.overflowMode = TextOverflowModes.Truncate;
                ((RectTransform)obj).sizeDelta = size;
            }
        }
    }

}