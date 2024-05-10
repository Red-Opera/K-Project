using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTrigger : MonoBehaviour
{
    private StageManager StageManager;
    FadeEffect fadeEffect;

    void Start()
    {
        fadeEffect = GameObject.Find("Fade").GetComponent<FadeEffect>();

        // StageManager 스크립트 가져오기
        StageManager = GameObject.FindObjectOfType<StageManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
         StartCoroutine(StageM(other));
    }

    private IEnumerator StageM(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            StartCoroutine(fadeEffect.FadeOut());
            while (fadeEffect.isFadeOut)
            {
                yield return null;
            }

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
    private void OnEnable()
    {
        fadeEffect = GameObject.Find("Fade").GetComponent<FadeEffect>();

        // StageManager 스크립트 가져오기
        StageManager = GameObject.FindObjectOfType<StageManager>();

        StartCoroutine(fadeEffect.FadeIn());
    }
    
}
