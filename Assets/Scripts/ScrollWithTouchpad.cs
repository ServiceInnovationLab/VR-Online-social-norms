using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VRTK;

[RequireComponent(typeof(ScrollRect))]
public class ScrollWithTouchpad : MonoBehaviour
{
    [SerializeField] VRTK_ControllerEvents controllerEvents;

    ScrollRect scrollView;

    Vector2 scrollAmount;
    Vector2 lastPosition;

    [SerializeField] float scrollScale = 10.0f;

    private void Awake()
    {
        scrollView = GetComponent<ScrollRect>();

        if (!controllerEvents)
        {
            Debug.LogError("No controller events are attached!");
        }
    }

    private void Update()
    {
        var currentAxis = controllerEvents.GetTouchpadAxis();

        if (currentAxis != Vector2.zero && lastPosition != Vector2.zero)
        {
            scrollAmount = currentAxis - lastPosition - scrollAmount;
        }
        else
        {
            lastPosition = currentAxis;
            scrollAmount *= 0.75f;

            if (scrollAmount.y < 2 || currentAxis != Vector2.zero)
            {
                scrollAmount = Vector2.zero;
            }
        }

        if (scrollAmount.y != 0)
        {
            var eventData = new PointerEventData(EventSystem.current) { scrollDelta = scrollAmount * scrollScale };

            ExecuteEvents.ExecuteHierarchy(gameObject, eventData, ExecuteEvents.scrollHandler);
        }
    }
}