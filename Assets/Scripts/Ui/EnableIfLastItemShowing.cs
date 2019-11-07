using UnityEngine;

public class EnableIfLastItemShowing : MonoBehaviour
{
    public ScrollRectLastItemClick checker;

    public GameObject toChange;

    private void OnDisable()
    {
        toChange.SetActive(false);
    }

    private void FixedUpdate()
    {
        toChange.SetActive(checker.IsLastItemShowing());
    }
}
