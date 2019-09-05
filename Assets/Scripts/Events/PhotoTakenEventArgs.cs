using UnityEngine;

public struct PhotoTakenEventArgs : ISenderEventArgs
{
    /// <summary>
    /// The camera that was used to take the photo
    /// </summary>
    public Camera Camera { get; }

    public Transform Sender
    {
        get { return Camera.transform; }
    }

    public PhotoTakenEventArgs(Camera camera)
    {
        Camera = camera;
    }
}

