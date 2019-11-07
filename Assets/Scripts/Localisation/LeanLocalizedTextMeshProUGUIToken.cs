using Lean.Localization;
using TMPro;

public class LeanLocalizedTextMeshProUGUIToken : LeanLocalizedTextMeshProUGUI
{
    public string prefix;

    public override void UpdateTranslation(LeanTranslation translation)
    {
        // Get the TextMeshProUGUI component attached to this GameObject
        var text = GetComponent<TextMeshProUGUI>();

        // Use translation?
        if (translation != null && translation.Data is string)
        {
            text.text = LeanTranslation.FormatText(prefix + (string)translation.Data, text.text, this);
        }
        // Use fallback?
        else
        {
            text.text = LeanTranslation.FormatText(FallbackText, text.text, this);
        }
    }
}
