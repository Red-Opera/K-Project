using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTrigger : MonoBehaviour
{
   private StageManager StageManager;

    void Start()
    {
        // StageManager 스크립트 가져오기
        StageManager = GameObject.FindObjectOfType<StageManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어와 충돌했을 때
        if (other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("PreviousStageTrigger"))
            {
                StageManager.ChangeStage(-1);
            }
            // 다음 스테이지로 이동
            else if (gameObject.CompareTag("NextStageTrigger"))
            {
                StageManager.ChangeStage(1);
            }
            // 이벤트 스테이지로 이동
            else if (gameObject.CompareTag("EventStageTrigger"))
            {
                // 이벤트 처리를 수행하는 코드 작성
                // 예: 보상 지급, 특정 상태 변경 등
                Debug.Log("EventStage로 이동!");
            }
        }
    }
}
