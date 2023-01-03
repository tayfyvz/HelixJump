using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class OxoEditor
{
    private static string mainPath = "Assets/OXO/";

    #region For Designers

    [MenuItem("[Oxo Games]/ For Designers / Ping Audio Clips Folder")]
    static void PingAudioClips()
    {
        string path = $"{mainPath}AudioClips";

        EditorUtility.FocusProjectWindow();
        Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
        Selection.activeInstanceID = obj.GetInstanceID();
        EditorGUIUtility.PingObject(obj);

        // Selection.activeObject = obj;
    }
    [MenuItem("[Oxo Games]/ For Designers / Open AudioManager Prefab")]
    static void OpenAudioManagerPrefab()
    {
        string path = $"{mainPath}Prefabs/AudioManager.prefab";
        PrefabUtility.LoadPrefabContents(path);
        Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
        Selection.activeInstanceID = obj.GetInstanceID();
        EditorGUIUtility.PingObject(obj);

        // Selection.activeObject = obj;
    }
    [MenuItem("[Oxo Games] / Load Game Scene")]
    static void LoadGameScene()
    {
        string path = AssetDatabase.GetAssetPath(GeneralSettings.Instance.Scene);
        EditorSceneManager.OpenScene($"{path}");
    }
    #endregion

    #region For Developers
    
    [MenuItem("[Oxo Games]/ For Developer / Ping General Settings")]
    static void PingGeneralSettings()
    {
        Object obj  = Resources.Load("OXO/Game Settings",typeof(Object));

        EditorUtility.FocusProjectWindow();
        EditorGUIUtility.PingObject(obj);
        Selection.activeObject = obj;
    }
    

    #endregion
}
