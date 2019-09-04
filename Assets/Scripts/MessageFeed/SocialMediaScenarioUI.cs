using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class SocialMediaScenarioUI : MonoBehaviour
{
    [SerializeField] StackChildren container;
    [SerializeField] Button prefab;

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
            display.GetComponentInChildren<Text>().text = option.name + "(Project)";

            display.onClick.AddListener(() =>
            {
               display.Select();
               picker.CurrentScenario = option;
            });

            if (picker.CurrentScenario == option)
            {
                display.Select();
            }
        }

        foreach (var option in picker.CustomScenarios)
        {
            var display = Instantiate(prefab, container.transform);

            display.gameObject.SetActive(true);
            display.GetComponentInChildren<Text>().text = Path.GetFileNameWithoutExtension(option) + "(Filesystem)";

            display.onClick.AddListener(() =>
            {
                display.Select();
                picker.LoadScenario(option);
            });
        }

        container.Resize();
    }

}
