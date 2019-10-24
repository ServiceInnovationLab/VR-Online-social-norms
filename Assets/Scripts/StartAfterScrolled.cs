using UnityEngine;

public class StartAfterScrolled : MonoBehaviour
{
    public ScreenMessageFeedView view1;
    public ScreenMessageFeedView view3;

    public ScrollRectLastItemClick[] checkers;
    int count;

    bool first = false;

    public void SecondGo()
    {
        checkers[0].enabled = true;
        count--;

        first = true;
    }

    private void Awake()
    {
        foreach (var checker in checkers)
        {
            checker.onClicked.AddListener(() =>
            {
                if (checker.enabled)
                {
                    count++;
                    checker.enabled = false;

                    if (count >= checkers.Length)
                    {
                        if (!first)
                        {

                            view1.Continue();
                        }
                        else
                        {
                            view3.CompleteFeed();
                            view3.Continue();
                        }
                    }
                }
            });
        }
    }

    private void FixedUpdate()
    {
        foreach (var checker in checkers)
        {
            if (checker.enabled)
            {
                checker.Check();
            }
        }
    }

}
