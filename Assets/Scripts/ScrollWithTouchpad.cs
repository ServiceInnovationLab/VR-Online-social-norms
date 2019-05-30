using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VRTK;

[RequireComponent(typeof(ScrollRect))]
public class ScrollWithTouchpad : MonoBehaviour
{
    [SerializeField] VRTK_ControllerEvents controllerEvents;

    ScrollRect scrollView;

    private void Awake()
    {
        scrollView = GetComponent<ScrollRect>();

        if (!controllerEvents)
        {
            Debug.LogError("No controller events are attached!");
        }
    }

    void FixedUpdate()
    {
        var eventData = new PointerEventData(EventSystem.current) { scrollDelta = controllerEvents.GetTouchpadAxis() };

        ExecuteEvents.ExecuteHierarchy(gameObject, eventData, ExecuteEvents.scrollHandler);
    }
}