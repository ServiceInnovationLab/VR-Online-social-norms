using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEditor.SceneManagement;

public class BuildScript
{

    static string[] GetScenePaths()
    {
        return EditorBuildSettings.scenes.Select(x => x.path).ToArray();
    }

    [MenuItem("Build/Test Build")]
    public static void Build()
    {
        string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
        string[] levels = GetScenePaths();
        
        BuildPipeline.BuildPlayer(levels, path + "/" + Application.productName + ".exe", BuildTarget.StandaloneWindows64, BuildOptions.ShowBuiltPlayer);
    }

    static bool PrepareScenes()
    {
        string currentScene = EditorSceneManager.GetActiveScene().path;

        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            return false;

        EditorSceneManager.OpenScene("");

        // Make changes here
        // DestroyImmediately.  GameObject.Find. Etc.

        EditorSceneManager.SaveOpenScenes();

        EditorSceneManager.OpenScene(currentScene);

        return true;
    }

    //[UnityEditor.Callbacks.PostProcessScene]
    public static void OnPostprocessScene()
    {
        if (BuildPipeline.isBuildingPlayer)
        {

        }
        else
        {

        }
    }

}
