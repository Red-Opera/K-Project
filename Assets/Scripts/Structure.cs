using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    public int recoveryAmount = 50;
    public float interactionRange = 2f;

    private bool isUsed = false;
     void Update()
    {
        // 플레이어와 구조물 사이의 거리를 계산
        float distance = Vector2.Distance(transform.position, PlayerController.instance.transform.position);

        // 플레이어가 구조물과의 상호작용 거리 내에 있고, 스페이스바를 눌렀으며, 구조물이 아직 사용되지 않은 상태인 경우
        if (distance <= interactionRange && Input.GetKeyDown(KeyCode.Space) && !isUsed)
        {
            // 플레이어의 체력을 회복
            PlayerHealth playerHealth = PlayerController.instance.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Heal(recoveryAmount); // 플레이어의 회복 함수 호출
                isUsed = true; // 구조물을 사용한 상태로 변경
                gameObject.SetActive(false); // 구조물 비활성화
            }
        }
    }
}