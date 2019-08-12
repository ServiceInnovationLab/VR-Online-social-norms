using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class SlideUpOnButtonPress : MonoBehaviour
{
    [SerializeField] float animationTime = 1.0f;
    [SerializeField] UnityEvent movedToTop;
    [SerializeField] UnityEvent movedToBottom;

    float topY;
    bool atTop;

    private void Awake()
    {
        topY = transform.position.y;
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        atTop = false;
    }

    IEnumerator DoMove(float yPosition, UnityEvent onComplete)
    {
        Vector3 moveAmount = new Vector3(0, (yPosition - transform.position.y) / animationTime, 0);

        while (true)
        {
            yield return new WaitForEndOfFrame();
            
            transform.position += moveAmount * Time.deltaTime;            

            if (transform.position.y <= 0 || transform.position.y >= topY)
            {
                break;
            }
        }

        onComplete?.Invoke();
    }

    public void MoveUp()
    {
        StopAllCoroutines();
        StartCoroutine(DoMove(topY, movedToTop));
    }

    public void MoveDown()
    {
        StopAllCoroutines();
        StartCoroutine(DoMove(0, movedToBottom));
    }

    public void Toggle()
    {
        StopAllCoroutines();

        if (atTop)
        {
            MoveDown();
        }
        else
        {
            MoveUp();
        }

        atTop = !atTop;
    }
}
