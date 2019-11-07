using UnityEngine;
using Lean.Localization;

public class LocalisedText : LeanLocalizedBehaviour
{
    [Tooltip("If PhraseName couldn't be found, this text will be used")]
    public string FallbackText;

    public string CurrentValue { get; private set; }

    private void Awake()
    {
        CurrentValue = FallbackText;
    }

    // This gets called every time the translation needs updating
    public override void UpdateTranslation(LeanTranslation translation)
    {
        // Use translation?
        if (translation != null && translation.Data is string)
        {
            CurrentValue = LeanTranslation.FormatText((string)translation.Data, null, this) ;
        }
        // Use fallback?
        else
        {
            CurrentValue = FallbackText;
        }
    }

}
