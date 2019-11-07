using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(ScrollRect))]
public class DetechScrollToBottom : MonoBehaviour
{
    [SerializeField] UnityEvent onReachedThreshold = new UnityEvent();
    [SerializeField] bool bottomIsZero = true;
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
        if ((rect.verticalNormalizedPosition >= threshold && !bottomIsZero) || (bottomIsZero && rect.verticalNormalizedPosition <= 1 - threshold))
        {
            onReachedThreshold?.Invoke();
        }
    }
}
