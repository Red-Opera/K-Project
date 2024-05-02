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
        }
    }
}
