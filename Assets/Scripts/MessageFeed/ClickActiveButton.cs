using UnityEngine;
using UnityEngine.UI;

public class ClickActiveButton : MonoBehaviour
{
    [SerializeField] Button[] buttons;
    [SerializeField] bool onlyClickFirstActive;

    public void DoClick()
    {
        foreach (var button in buttons)
        {
            if (!button.gameObject.activeInHierarchy)
                continue;

            button.onClick?.Invoke();

            if (onlyClickFirstActive)
            {
                break;
            }
        }
    }
}
