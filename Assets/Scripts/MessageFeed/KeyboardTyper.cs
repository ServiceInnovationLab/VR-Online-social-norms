using UnityEngine;

public class KeyboardTyper : MonoBehaviour
{
    [SerializeField] float pressedPosition = -0.02f;
    [SerializeField] float bounceBackTime = 0.2f;

    private void OnEnable()
    {
        EventManager.StartListening(Events.KeyboardTextTyped, OnTextTyped);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.KeyboardTextTyped, OnTextTyped);
    }

    void OnTextTyped(IEventArgs args)
    {
        if (!(args is TextTypedEventArgs))
            return;

        var textArgs = (TextTypedEventArgs)args;

        var key = transform.Find(textArgs.TypedText.ToUpper());

        if (key)
        {
            var mover = key.GetComponent<PositionMove>();

            if (!mover)
            {
                mover = key.gameObject.AddComponent<PositionMove>();
            }

            var pos = key.localPosition;
            pos.y = pressedPosition;

            mover.time = bounceBackTime;
            mover.Move(pos);
        }
    }
}
