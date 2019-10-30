using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System;

[CustomEditor(typeof(SocialMediaScenario))]
public class SocialMediaScenarioEditor : Editor
{


    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.LabelField("Editing options", EditorStyles.boldLabel);

        var scenario = Selection.activeObject as SocialMediaScenario;
    }

    [MenuItem("Import/OnlineScenario")]
    static void ImportScenario()
    {
        try
        {
            EditorUtility.DisplayProgressBar("Importing", "Please wait..", 0);
            DoImport();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }
    }

    static void DoImport()
    {
        var file = EditorUtility.OpenFilePanel("Choose spreadsheet", "", "xlsx");

        if (!string.IsNullOrWhiteSpace(file))
        {
            var scenarioName = Path.GetFileNameWithoutExtension(file);

            var folder = Path.GetDirectoryName("Assets/Data/SocialMediaFeed/");

            var newFolder = Path.Combine(folder, "_Imported", scenarioName);

            if (!Directory.Exists(Path.Combine(folder, "_Imported")))
            {
                AssetDatabase.CreateFolder(folder, "_Imported");
            }

            AssetDatabase.DeleteAsset(newFolder);

            AssetDatabase.CreateFolder(Path.Combine(folder, "_Imported"), scenarioName);


            var newData = ScenarioSpreadsheetLoader.LoadSocialMediaScenario(file);

            SaveOnlineProfile(newData.senderProfile, newFolder);
            SaveOnlineProfile(newData.receiverProfile, newFolder);

            foreach (var messageFeedType in (SocialMediaScenatioMessageFeedType[])Enum.GetValues(typeof(SocialMediaScenatioMessageFeedType)))
            {
                var messageFeed = newData.GetMessageFeed(messageFeedType);

                if (messageFeed == null)
                    continue;

                var name = Enum.GetName(typeof(SocialMediaScenatioMessageFeedType), messageFeedType);

                AssetDatabase.CreateFolder(newFolder, name);
                SaveMessageFeed(messageFeed, Path.Combine(newFolder, name), name);
            }

            foreach (var messageFeedType in (SocialMediaScenarioSMStype[])Enum.GetValues(typeof(SocialMediaScenarioSMStype)))
            {
                var messageFeed = newData.GetSMSMessageFeed(messageFeedType);

                if (messageFeed == null)
                    continue;

                var name = Enum.GetName(typeof(SocialMediaScenarioSMStype), messageFeedType);

                AssetDatabase.CreateFolder(newFolder, name);
                SaveMessageFeed(messageFeed, Path.Combine(newFolder, name), name);
            }

            AssetDatabase.CreateAsset(newData, Path.Combine(newFolder, scenarioName + ".asset"));
        }
    }

    private static void SaveMessageFeed(MessageFeed feed, string path, string name)
    {
        var texturePath = Path.Combine(path, "Textures");
        feed.name = name;

        foreach (var profiles in feed.messages.Select(x => x.profile).Where(x => x != null))
        {
            SaveOnlineProfile(profiles, path);
        }

        // Handle duplicates...
        int i = 0;
        foreach (var message in feed.messages)
        {
            if (message.profile != null)
            {
                message.profile = AssetDatabase.LoadAssetAtPath<OnlineProfile>(Path.Combine(path, message.profile.name + ".asset"));
            }

            if (message.image || message.animatedImage)
            {
                if (!AssetDatabase.IsValidFolder(texturePath))
                {
                    AssetDatabase.CreateFolder(path, "Textures");
                }
            }

            if (message.image)
            {
                message.image = SaveImage(message.image.texture, Path.Combine(texturePath, "image_" + i + ".png"));
            }

            if (message.animatedImage)
            {
                var path_name = "animated_image_" + i;
                AssetDatabase.CreateFolder(path, path_name);
                SaveAnimatedImage(message.animatedImage, Path.Combine(path, path_name), i);
            }

            i++;
        }

        AssetDatabase.CreateAsset(feed, Path.Combine(path, name + ".asset"));
    }

    private static void SaveOnlineProfile(OnlineProfile profile, string path)
    {
        profile.name = profile.username;

        var assetPath = Path.Combine(path, profile.name + ".asset");

        // Only save profiles once
        if (File.Exists(Path.Combine(Application.dataPath.Replace("/Assets", ""), assetPath)))
            return;

        if (profile.picture)
        {
            var texturePath = Path.Combine(path, "Textures");

            if (!AssetDatabase.IsValidFolder(texturePath))
            {
                AssetDatabase.CreateFolder(path, "Textures");
            }

            profile.picture = SaveImage(profile.picture.texture, Path.Combine(texturePath, profile.name + ".png"));
        }

        AssetDatabase.CreateAsset(profile, assetPath);
    }

    private static void SaveAnimatedImage(AnimatedImage image, string path, int index)
    {
        for (int i = 0; i < image.images.Length; i++)
        {
            image.images[i] = SaveImage(image.images[i].texture, Path.Combine(path, i + ".png"));
        }

        AssetDatabase.CreateAsset(image, Path.Combine(path, "AnimatedImage_" + index +  ".asset"));
    }

    private static Sprite SaveImage(Texture2D texture, string path)
    {
        File.WriteAllBytes(path, texture.EncodeToPNG());

        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

        TextureImporter tImporter = AssetImporter.GetAtPath(path) as TextureImporter;
        tImporter.textureType = TextureImporterType.Sprite;
        tImporter.SaveAndReimport();

        return AssetDatabase.LoadAssetAtPath<Sprite>(path);
    }


}