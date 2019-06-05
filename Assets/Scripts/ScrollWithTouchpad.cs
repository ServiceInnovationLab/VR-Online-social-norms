using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VRTK;

/// <summary>
/// Scrolls with the touchpad. Supports swiping as well. Currently only Y axis
/// </summary>
[RequireComponent(typeof(ScrollRect))]
public class ScrollWithTouchpad : MonoBehaviour
{
    [SerializeField] VRTK_ControllerEvents controllerEvents;
    [SerializeField] float dropOff = 0.9f;
    [SerializeField] float scrollScale = 10.0f;
    [SerializeField] float swipeScale = 2.5f;

    ScrollRect scrollView;

    Vector2 scrollAmount;
    bool wasTouched;
    Vector2 lastPosition;
    Vector2 touchStartPosition;    
    float touchStartTime;

    private void Awake()
    {
        scrollView = GetComponent<ScrollRect>();

        if (!controllerEvents)
        {
            var controllerWithGrab = FindObjectOfType<VRTK_ObjectAutoGrab>();

            if (controllerWithGrab)
            {
                controllerEvents = controllerWithGrab.GetComponent<VRTK_ControllerEvents>();
            }
            else
            {
                Debug.LogError("No controller events are attached!");
            }
        }
    }

    private void OnEnable()
    {
        foreach (var teleporter in controllerEvents.GetComponentsInChildren<VRTK_Pointer>())
        {

            teleporter.enabled = false;
        }
    }

    private IEnumerator DoScroll(float velocity, Vector2 direction)
    {
        while (velocity >= 1)
        {
            yield return new WaitForFixedUpdate();

            var eventData = new PointerEventData(EventSystem.current) { scrollDelta = direction * velocity };
            ExecuteEvents.ExecuteHierarchy(gameObject, eventData, ExecuteEvents.scrollHandler);

            velocity *= dropOff;
        }
    }

    private void Update()
    {
        bool isTouched = controllerEvents.IsButtonPressed(VRTK_ControllerEvents.ButtonAlias.TouchpadTouch) ||
            controllerEvents.IsButtonPressed(VRTK_ControllerEvents.ButtonAlias.TouchpadPress);

        var currentAxis = controllerEvents.GetTouchpadAxis();

        if (isTouched && !wasTouched)
        {
            scrollAmount = Vector2.zero;
            lastPosition = currentAxis;
            touchStartPosition = currentAxis;
            touchStartTime = Time.time;
            StopAllCoroutines();
        }
        else if (!isTouched && wasTouched)
        {
            var touchEndTime = Time.time;
            var touchTime = touchEndTime - touchStartTime;

            if (touchTime > 0)
            {
                var swipeVector = lastPosition - touchStartPosition;
                var velocity = swipeVector.magnitude / touchTime;
                var scrollDir = new Vector2(0, Mathf.Sign(swipeVector.y));

                StartCoroutine(DoScroll(velocity * swipeScale, scrollDir));
            }
        }        

        if (isTouched)
        {
            scrollAmount = currentAxis - lastPosition;
            lastPosition = currentAxis;

            if (scrollAmount.y != 0)
            {
                var eventData = new PointerEventData(EventSystem.current) { scrollDelta = scrollAmount * scrollScale };

                ExecuteEvents.ExecuteHierarchy(gameObject, eventData, ExecuteEvents.scrollHandler);
            }
        }

        wasTouched = isTouched;
    }
}