using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class SenderRoomInteraction : MonoBehaviour
{
    public UnityEvent OnEnterPov;

    public UnityEvent OnCompleted;

    public ScrollRectDetector[] requiredScrolls;
    public FlashingImage[] images;

    public StartAfterScrolled scrollChecker;

    [SerializeField] float timeBeforeActivatingNext = 1.0f;

    public void OnPovSwitch()
    {
        OnEnterPov?.Invoke();
    }

    private void Awake()
    {
        foreach (var image in images)
        {
            image.gameObject.SetActive(false);
        }

        foreach (var detector in requiredScrolls)
        {
            detector.enabled = false;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(DoChecks());

        foreach (var detector in requiredScrolls)
        {
            detector.enabled = true;
        }
    }

    private IEnumerator DoChecks()
    {
        bool activate = true;

        bool[] previous = new bool[requiredScrolls.Length];
        for (int i = 0; i < previous.Length; i++)
        {
            previous[i] = false;
        }

        while (activate)
        {
            yield return new WaitForFixedUpdate();

            for (int i = 0; i < requiredScrolls.Length; i++)
            {
                bool done = requiredScrolls[i].ScrollingDone;

                if (images[i])
                {
                    images[i].gameObject.SetActive(!done && activate);
                }

                if (done)
                {
                    if (!previous[i])
                    {
                        yield return new WaitForSeconds(timeBeforeActivatingNext);
                    }
                }
                else
                {
                    activate = false;
                }

                previous[i] = done;
            }

            // All are complete if we still get here!
            if (activate)
            {
                break;
            }

            activate = true;
        }

        yield return new WaitUntil(() => scrollChecker.view1.IsDone && scrollChecker.view2.IsDone);
        OnCompleted?.Invoke();
    }

}
