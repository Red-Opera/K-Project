using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    void Update()
    {
        // 스페이스바를 누르면 stage1로 이동
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            LoadStage1();
        }
    }

    void LoadStage1()
    {
        // stage1 씬을 로드하는 코드
        SceneManager.LoadScene("Stage1");
    }
}
