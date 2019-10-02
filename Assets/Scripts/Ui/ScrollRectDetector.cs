using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScrollRectDetector : MonoBehaviour
{
    [SerializeField] UnityEvent onScrolled;
    [SerializeField] float desiredAmount = 300;

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
        if (Mathf.Abs(rect.content.anchoredPosition.y - startingPosition.y) > desiredAmount)
        {
            enabled = false;
            onScrolled?.Invoke();
        }
    }
}
