using UnityEngine;
using UnityEngine.UI;

public class LogScrollValues : MonoBehaviour
{
    [SerializeField] float theshold = 10;

    ScrollRect rect;

    Vector2 startingPosition;

    private void Awake()
    {
        rect = GetComponent<ScrollRect>();
    }

    private void OnEnable()
    {
        startingPosition = rect.content.anchoredPosition;
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(rect.content.anchoredPosition.y - startingPosition.y) > theshold)
        {
            Debug.Log(rect.content.anchoredPosition.y);
            startingPosition = rect.content.anchoredPosition;
        }
    }
}
