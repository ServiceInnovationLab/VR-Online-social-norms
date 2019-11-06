using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class SocialMediaScenarioUI : MonoBehaviour
{
    [SerializeField] StackChildren container;
    [SerializeField] Button prefab;

    Button selected;

    // Use this for initialization
    void Start()
    {
        var picker = SocialMediaScenarioPicker.Instance;

        if (!picker.CanChange)
        {
            Destroy(gameObject);
            return;
        }

        foreach (var option in picker.ScenariosInProject)
        {
            var display = Instantiate(prefab, container.transform);

            display.gameObject.SetActive(true);
            display.GetComponentInChildren<Text>().text = option.name;

            display.onClick.AddListener(() =>
            {
                if (selected)
                {
                    selected.interactable = true;
                }

                display.interactable = false;
                selected = display;
                picker.CurrentScenario = option;
            });

            if (picker.CurrentScenario == option)
            {
                selected = display;
                display.interactable = false;
            }
        }

        foreach (var option in picker.CustomScenarios)
        {
            var display = Instantiate(prefab, container.transform);

            display.gameObject.SetActive(true);
            display.GetComponentInChildren<Text>().text = Path.GetFileNameWithoutExtension(option) + " (Excel)";

            display.onClick.AddListener(() =>
            {
                if (selected)
                {
                    selected.interactable = true;
                }

                display.interactable = false;
                selected = display;
                picker.LoadScenario(option);
            });
        }

        container.Resize();
    }

}
