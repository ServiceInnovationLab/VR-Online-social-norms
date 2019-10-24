using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

[CustomEditor(typeof(SocialMediaScenario))]
public class SocialMediaScenarioEditor : Editor
{


    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        //EditorGUILayout.LabelField("Editing options", EditorStyles.boldLabel);

        //var scenario = Selection.activeObject as SocialMediaScenario;

        //if (scenario && GUILayout.Button("Export"))
        //{
        //    var usedProfiles = scenario.messageFeed.messages.Select(x => x.profile).Distinct().ToList();

        //    var outdir = SocialMediaScenarioPicker.CustomScenarioPath;

        //    Directory.CreateDirectory(outdir);

        //    var profilesDir = SocialMediaScenarioPicker.CustomScenarioProfilePicturePath;
        //    Directory.CreateDirectory(profilesDir);

        //    foreach (var profile in usedProfiles)
        //    {
        //        var texturePath = AssetDatabase.GetAssetPath(profile.picture.texture);
        //        File.Copy(texturePath, Path.Combine(profilesDir, profile.picture.name + Path.GetExtension(texturePath)), true);
        //    }

        //    var messageFeed = new MessageFeedData()
        //    {
        //        Profiles = usedProfiles.Select(x => new ProfileData() { picture = x.picture.name + Path.GetExtension(AssetDatabase.GetAssetPath(x.picture.texture)), username = x.username, tag = x.tag }).ToArray(),
        //        MessageStream = scenario.messageFeed.messages.Select(x => new MessageData() { Message = x.message, ProfileIndex = usedProfiles.IndexOf(x.profile) }).ToArray()
        //    };

        //    var data = new OnlineScenarioData()
        //    {
        //        ReceiverMessage = scenario.receiverMessage,
        //        SenderMessage = scenario.senderMessage,
        //        MessageFeedData = messageFeed
        //    };

        //    File.WriteAllText(Path.Combine(outdir, scenario.name + ".json"), JsonUtility.ToJson(data, true));
        //}
    }


}