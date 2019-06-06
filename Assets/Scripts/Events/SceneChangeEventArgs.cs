using System;

public struct SceneChangeEventArgs : IEventArgs
{
    /// <summary>
    /// Gets the name of the GameObject used as the point to teleport to the scene
    /// </summary>
    public string SceneTeleportName { get; }

    public SceneChangeEventArgs(string name)
    {
        SceneTeleportName = name;
    }
}