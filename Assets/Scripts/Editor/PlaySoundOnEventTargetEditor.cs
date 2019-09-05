using UnityEditor;
using System;
using System.Reflection;
using System.Linq;

[CustomEditor(typeof(PlaySoundOnEventTarget))]
public class PlaySoundOnEventTargetEditor : Editor
{
    string[] _choices = { };
    int _choiceIndex = 0;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var playSoundOnEvent = target as PlaySoundOnEventTarget;

        _choiceIndex = Array.IndexOf(_choices, playSoundOnEvent.eventName);

        if (_choiceIndex < 0)
        {
            _choiceIndex = 0;
        }

        _choiceIndex = EditorGUILayout.Popup(_choiceIndex, _choices);

        playSoundOnEvent.eventName = _choices[_choiceIndex];
        EditorUtility.SetDirty(target);
    }

    private void OnEnable()
    {
        _choices = typeof(Events).GetFields(BindingFlags.Static | BindingFlags.Public).Select(x => (string)x.GetValue(null)).ToArray();
    }
}
