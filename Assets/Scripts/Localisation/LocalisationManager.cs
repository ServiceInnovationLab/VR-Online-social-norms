using UnityEngine;
using Lean.Localization;
using System.IO;
using System.Collections.Generic;

public class LocalisationManager : LeanLocalization
{

    [SerializeField] LanguageFonts languageFonts;

    public static string LanguagesPath
    {
        get { return Path.Combine(Application.streamingAssetsPath, "Languages"); }
    }

    static bool areLanguagesCreated = false;
    static readonly List<string> languages = new List<string>();

    private void Awake()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
            return;
#endif

        enabled = true;

        LoadLanguages();

        SetupFonts();

        foreach (var language in languages)
        {
            Languages.Add(new LeanLanguage() { Name = language });
        }
    }

    private void SetupFonts()
    {
        var normalFont = new GameObject("NormalFont");
        normalFont.transform.SetParent(transform);
        var normalPhrase = normalFont.AddComponent<LeanPhrase>();
        normalPhrase.Data = LeanPhrase.DataType.Object;

        var mediumSdfFont = new GameObject("MediumSdfFont");
        mediumSdfFont.transform.SetParent(transform);
        var mediumSdfPhrase = mediumSdfFont.AddComponent<LeanPhrase>();
        mediumSdfPhrase.Data = LeanPhrase.DataType.Object;

        var lightSdfFont = new GameObject("LightSdfFont");
        lightSdfFont.transform.SetParent(transform);
        var lightSdfPhrase = lightSdfFont.AddComponent<LeanPhrase>();
        lightSdfPhrase.Data = LeanPhrase.DataType.Object;

        var normalSdfFont = new GameObject("NormalSdfFont");
        normalSdfFont.transform.SetParent(transform);
        var normalSdPhrase = normalSdfFont.AddComponent<LeanPhrase>();
        normalSdPhrase.Data = LeanPhrase.DataType.Object;

        foreach (var font in languageFonts.Fonts)
        {
            normalPhrase.AddEntry(font.language, null, font.normalFont);
            mediumSdfPhrase.AddEntry(font.language, null, font.mediumSdfFont);
            lightSdfPhrase.AddEntry(font.language, null, font.lightSdfFont);
            normalSdPhrase.AddEntry(font.language, null, font.normalSdfFont);
        }
    }

    private void LoadLanguages()
    {
        if (areLanguagesCreated)
            return;

        foreach (var file in Directory.EnumerateFiles(LanguagesPath, "*.txt", SearchOption.TopDirectoryOnly))
        {
            var language = Path.GetFileNameWithoutExtension(file);

            var languageObject = new GameObject("Language: " + language);
            DontDestroyOnLoad(languageObject);

            var languageCSV = languageObject.AddComponent<LeanLanguageCSV>();
            languageCSV.Source = new TextAsset(File.ReadAllText(file));
            languageCSV.Language = language;

            languages.Add(language);
        }

        areLanguagesCreated = true;
    }


}
