using UnityEngine;
using UnityEngine.UI;
using Lean.Localization;

public class LanguagePickerUI : MonoBehaviour
{
    [SerializeField] LeanLocalization localization;
    [SerializeField] StackChildren container;
    [SerializeField] Button prefab;

    Button selected;

    void Start()
    {
        // Current language takes some time to be set
        Invoke(nameof(ShowItems), 0.5f);
    }

    void ShowItems()
    {
        foreach (var option in localization.Languages)
        {
            var display = Instantiate(prefab, container.transform);

            display.gameObject.SetActive(true);
            display.GetComponentInChildren<Text>().text = option.Name;

            display.onClick.AddListener(() =>
            {
                if (selected)
                {
                    selected.interactable = true;
                }

                display.interactable = false;
                selected = display;
                localization.SetCurrentLanguage(option.Name);
            });

            if (LeanLocalization.CurrentLanguage == option.Name)
            {
                display.interactable = false;
                selected = display;
            }
        }

        container.Resize();
    }
}
