using UnityEngine;

public class PositionMove : MonoBehaviour
{
    public float time = 0.5f;

    float originalPositin;
    Vector3 start;
    Vector3 from;
    bool moveBack = false;

    private void Awake()
    {
        originalPositin = transform.localPosition.y;
        start = transform.localPosition;
    }

    public void Move(Vector3 newPos)
    {
        transform.localPosition = newPos;
        from = newPos;
        moveBack = true;
    }

    private void Update()
    {
        if (!moveBack) return;

        var diff = originalPositin - from.y;

        transform.localPosition += diff * Time.deltaTime * (1 / time) * Vector3.up;

        if (transform.localPosition.y >= originalPositin)
        {
            transform.localPosition = start;
            moveBack = false;
        }
    }
}
