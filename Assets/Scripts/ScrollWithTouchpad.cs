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
    float scrollTime;

    [SerializeField] float scrollVelocityTime = 0.15f;

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
        scrollAmount += controllerEvents.GetTouchpadAxis();
        scrollTime += Time.deltaTime;

        if (scrollTime >= scrollVelocityTime)
        {
            var eventData = new PointerEventData(EventSystem.current) { scrollDelta = scrollAmount / scrollVelocityTime };

            ExecuteEvents.ExecuteHierarchy(gameObject, eventData, ExecuteEvents.scrollHandler);
        }
    }

    //void FixedUpdate()
    //{
    //    var eventData = new PointerEventData(EventSystem.current) { scrollDelta = controllerEvents.GetTouchpadAxis() };

    //    ExecuteEvents.ExecuteHierarchy(gameObject, eventData, ExecuteEvents.scrollHandler);
    //}
}