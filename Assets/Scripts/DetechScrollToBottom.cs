using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(ScrollRect))]
public class DetechScrollToBottom : MonoBehaviour
{
    [SerializeField] UnityEvent onReachedThreshold = new UnityEvent();
    [SerializeField] float threshold = 0.8f;

    ScrollRect rect;

    private void Awake()
    {
        rect = GetComponent<ScrollRect>();
    }

    private void OnEnable()
    {
        rect.onValueChanged.AddListener(OnScroll);
    }

    private void OnDisable()
    {
        rect.onValueChanged.RemoveListener(OnScroll);
    }

    private void OnScroll(Vector2 amount)
    {
        Debug.Log(rect.verticalNormalizedPosition);
        if (rect.verticalNormalizedPosition >= threshold)
        {
            onReachedThreshold?.Invoke();
        }
    }
}
