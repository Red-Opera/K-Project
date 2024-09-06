#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class AutoSave : MonoBehaviour
{
    static AutoSave()
    {
        EditorApplication.playModeStateChanged += SaveCurrentScene;
    }

    private static void SaveCurrentScene(PlayModeStateChange state)
    {
        Debug.Log(EditorSceneManager.GetActiveScene().name + " 씬 자동 저장 완료!");

        if (EditorApplication.isPlaying == false && EditorApplication.isPlayingOrWillChangePlaymode == true)
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
    }
}
#endif