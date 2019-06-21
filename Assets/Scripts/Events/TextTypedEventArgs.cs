using System;

public struct TextTypedEventArgs : IEventArgs
{
    /// <summary>
    /// Gets the text that was typed
    /// </summary>
    public string TypedText { get; }

    public TextTypedEventArgs(string typed)
    {
        TypedText = typed;
    }
}