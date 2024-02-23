using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageStart : MonoBehaviour
{
 
    public float interactDistance = 3f; // 상호작용 가능한 거리
    void OnTriggerStay(Collider other)
    {
        // 플레이어가 일정 거리 내에 있고 스페이스바를 눌렀을 때
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.Space))
        {
            // 다음 씬으로 넘어감
            SceneManager.LoadScene("Stage1");
        }
    }
}
