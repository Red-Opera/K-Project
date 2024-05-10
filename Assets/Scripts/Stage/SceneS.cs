using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneS : MonoBehaviour
{
    private void Update() 
    {
        
    }
    public void ChangeToNextScene()
    {
        // 현재 씬 이름을 가져옴
        string currentSceneName = SceneManager.GetActiveScene().name;

        // 다음 씬으로 이동
        if (currentSceneName == "Stage1")
        {
            SceneManager.LoadScene("Stage2");
        }
        else if (currentSceneName == "Stage2")
        {
            SceneManager.LoadScene("Stage3");
        }
        else if (currentSceneName == "Stage3")
        {
            SceneManager.LoadScene("Map");
        }
        else if (currentSceneName == "Map")
        {
            SceneManager.LoadScene("Stage1");
        }
    }
}
