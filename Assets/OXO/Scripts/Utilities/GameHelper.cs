using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
public class GameHelper
{
    private static string mainPath = "Assets/OXO/";

    [MenuItem("[Game HELPER]/Reset All Game Data %t")]
    static void ClearData() {PlayerPrefs.DeleteAll(); PlayerPrefs.DeleteAll(); PlayerPrefs.Save();}


    [MenuItem("[Game HELPER]/Ping Camera")]
    static void PingCamera()
    {
        Object _object = GameObject.FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
        if (_object == null)
        {
            _object = GameObject.FindObjectOfType<Camera>();
        }

        Selection.activeObject = _object;

        EditorGUIUtility.PingObject(_object);
    }

    [MenuItem("[Game HELPER]/Select Null %g")]
    static void SelectNull() => Selection.activeObject = null;

    [MenuItem("[Game HELPER]/Toggle Inspector Lock %l")] // Ctrl + L
    static void ToggleInspectorLock()
    {
        ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
        ActiveEditorTracker.sharedTracker.ForceRebuild();
    }

    [MenuItem("[Game HELPER]/Clear Console #&c")]
    static void ClearConsole()
    {
        var assembly = Assembly.GetAssembly(typeof(SceneView));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);

    }
}
#endif