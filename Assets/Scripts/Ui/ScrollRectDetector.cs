using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScrollRectDetector : MonoBehaviour
{
    [SerializeField] UnityEvent onScrolled;
    [SerializeField] float desiredAmount = 300;

    ScrollRect rect;
    Vector2 startingPosition;

    public bool ScrollingDone { get; private set; }

    private void Awake()
    {
        rect = GetComponent<ScrollRect>();
    }

    private void OnEnable()
    {
        Restart();
    }

    public void Restart()
    {
        startingPosition = rect.content.anchoredPosition;
        ScrollingDone = false;

        if (!enabled)
        {
            enabled = true;
        }
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(rect.content.anchoredPosition.y - startingPosition.y) > desiredAmount)
        {
            ScrollingDone = true;
            enabled = false;
            onScrolled?.Invoke();
        }
    }
}
