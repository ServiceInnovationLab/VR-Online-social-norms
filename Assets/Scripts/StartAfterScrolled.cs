using UnityEngine;

public class StartAfterScrolled : MonoBehaviour
{
    public ScreenMessageFeedView view1;
    public ImageAppender view2;
    public ScreenMessageFeedView view3;

    public ScrollRectLastItemClick[] checkers;
    int count;

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
                        view1.scrollToBottom = true;
                        //view2.scrollToBottom = true;

                        view1.Continue();

                        if (view2)
                        {
                            view2.Continue();
                        }

                        if (view3)
                        {
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
