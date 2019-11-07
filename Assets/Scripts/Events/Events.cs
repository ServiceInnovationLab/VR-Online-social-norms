using System;

/// <summary>
/// Lists out events for use with the <see cref="EventManager"></see> to save hard coding the strings in the source/>
/// </summary>
public static class Events
{
    public static string SceneSwitched = nameof(SceneSwitched);

    public static string KeyboardTextTyped = nameof(KeyboardTextTyped);

    public static string PhotoTaken = nameof(PhotoTaken);
}