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

        var keyName = textArgs.TypedText.ToUpper();

        if (keyName == " ")
        {
            keyName = "Space";
        }

        var key = transform.Find(keyName);

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
