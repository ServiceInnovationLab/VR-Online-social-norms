using UnityEngine;

public struct PhotoTakenEventArgs : IEventArgs
{
    /// <summary>
    /// The camera that was used to take the photo
    /// </summary>
    public Camera Camera { get; }

    public PhotoTakenEventArgs(Camera camera)
    {
        Camera = camera;
    }
}

