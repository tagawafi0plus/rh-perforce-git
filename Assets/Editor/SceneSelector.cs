using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class SceneSelector : EditorWindow
{
    Vector2 m_scroll_pos = Vector2.zero;

    [MenuItem("Window/Scene Selector", false, 1000)]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<SceneSelector>("Scene Selector");
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("本番用シーン");
        createMenu(new string[] {"Assets/Scenes/Product"});
        GUILayout.Box("", GUILayout.Width(this.position.width), GUILayout.Height(1));

        EditorGUILayout.LabelField("開発用シーン");
        createMenu(new string[] {"Assets/Scenes/Develop"});
    }

    void createMenu(string[] lookFor)
    {
        m_scroll_pos = EditorGUILayout.BeginScrollView(m_scroll_pos);
        string[] sceneGUIDs = AssetDatabase.FindAssets("t:Scene", lookFor);

        foreach (var sceneGUID in sceneGUIDs)
        {
            string scenePath = AssetDatabase.GUIDToAssetPath(sceneGUID);
            string[] sceneName = scenePath.Split(new char[] {'/'});
            if (GUILayout.Button(sceneName[sceneName.Length - 1].Replace(".unity", "") + "を開く"))
            {
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    EditorSceneManager.OpenScene(scenePath);
            }
        }

        EditorGUILayout.EndScrollView();
    }
}