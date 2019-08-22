using UnityEngine;
using UnityEditor;
using VRTK;

public class SteamHapticTester : EditorWindow
{
    float strenth = 1;
    float duration = 1;
    float freq = 150;

    [MenuItem("Window/Steam Haptic Tester")]
    static void Awake()
    {
        GetWindow<SteamHapticTester>("Steam Haptic Tester").Show();
    }

    void OnGUI()
    {
        strenth = EditorGUILayout.FloatField("Strength", strenth);
        duration = EditorGUILayout.FloatField("duration", duration);
        freq = EditorGUILayout.FloatField("freq", freq);


        if (GUILayout.Button("Test"))
        {
            var source = Selection.activeGameObject.GetComponent<SDK_SteamVRInputSource>();

            if (source)
            {
                source.TriggerHapticPulse(strenth, duration, freq);
            }
        }
    }


}