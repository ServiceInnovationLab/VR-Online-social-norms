using UnityEngine;

/// <summary>
/// Just a simple way to store text as an object to allow it to be reused
/// </summary>
[CreateAssetMenu(menuName = "TextObject")]
public class TextObject : ScriptableObject
{
    public string text;
}
