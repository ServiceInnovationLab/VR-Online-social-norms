using UnityEngine;

public interface ISenderEventArgs : IEventArgs
{
    Transform Sender { get; }
}
