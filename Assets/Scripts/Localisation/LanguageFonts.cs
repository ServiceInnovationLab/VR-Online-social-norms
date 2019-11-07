using UnityEngine;
using TMPro;


[System.Serializable]
public struct LanguageFont
{
    public string language;
    public Font normalFont;

    public TMP_FontAsset mediumSdfFont;
    public TMP_FontAsset lightSdfFont;
    public TMP_FontAsset normalSdfFont;
}

[CreateAssetMenu(menuName = "LanguageFonts")]
public class LanguageFonts : ScriptableObject
{

    public LanguageFont[] Fonts;
    
}
