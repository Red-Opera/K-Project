using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    void Update()
    {
        // �����̽��ٸ� ������ stage1�� �̵�
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            LoadStage1();
        }
    }

    void LoadStage1()
    {
        // stage1 ���� �ε��ϴ� �ڵ�
        SceneManager.LoadScene("Stage1");
    }
}
