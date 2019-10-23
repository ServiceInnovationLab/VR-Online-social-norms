using UnityEngine;
using VRTK;
using Lean.Localization;

public class LeanLocalizedTooltip : LeanLocalizedBehaviour
{

    [Tooltip("If PhraseName couldn't be found, this text will be used")]
    public string FallbackText;

    // This gets called every time the translation needs updating
    public override void UpdateTranslation(LeanTranslation translation)
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
            return;
#endif

        // Get the tooltip component attached to this GameObject
        var text = GetComponent<VRTK_ObjectTooltip>();

        // Use translation?
        if (translation != null && translation.Data is string)
        {
            text.UpdateText(LeanTranslation.FormatText((string)translation.Data, text.displayText, this));
        }
        // Use fallback?
        else
        {
            text.UpdateText(LeanTranslation.FormatText(FallbackText, text.displayText, this));
        }
    }

    protected virtual void Awake()
    {
        // Should we set FallbackText?
        if (string.IsNullOrEmpty(FallbackText) == true)
        {
            // Get the tooltip component attached to this GameObject
            var text = GetComponent<VRTK_ObjectTooltip>();

            // Copy current text to fallback
            FallbackText = text.displayText;
        }
    }
}
