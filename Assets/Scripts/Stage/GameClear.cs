using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClear : MonoBehaviour
{
    public ResultUI resultUIManager; // Inspector에서 연결

    private void OnEnable()
    {
        if (resultUIManager != null)
        {
            resultUIManager.GameIsEnd();
        }
        else
        {
            Debug.LogWarning("ResultUI Manager가 연결되지 않았습니다.");
        }
    }
}
