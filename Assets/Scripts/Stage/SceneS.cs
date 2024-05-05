using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneS : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트가 Player인지 확인
        if (other.CompareTag("Player"))
        {
            // Player와 충돌한 경우, f 키 입력을 감지하여 다음 스테이지로 이동
            if (Input.GetKeyDown(KeyCode.F))
            {
                // 현재 씬 이름을 가져옴
                string currentSceneName = SceneManager.GetActiveScene().name;

                // 다음 스테이지로 이동
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
                    // 스테이지 3까지 클리어한 경우 Map 씬으로 이동
                    SceneManager.LoadScene("Map");
                }
            }
        }
    }
}
