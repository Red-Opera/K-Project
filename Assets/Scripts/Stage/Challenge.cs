using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Challenger : MonoBehaviour
{
    public GameObject[] Stages;
    public GameObject[] Boss;        // 보스룸 배열
    public GameObject[] BossHPBar;
    private int currentStageIndex;

    void Start()
    {
        // 초기 스테이지 설정
        currentStageIndex = 0;
        SetActiveStage(currentStageIndex);

    }

   public void ChangeStage(int direction)
    {
        
         GameObject player = GameObject.FindGameObjectWithTag("Player");

        int newStageIndex = currentStageIndex + direction;

        // 유효한 스테이지 인덱스인지 확인
        if (newStageIndex >= 0 && newStageIndex < Stages.Length)
        {
            // 현재 스테이지 비활성화
            Stages[currentStageIndex].SetActive(false);

            // 새로운 스테이지 활성화
            currentStageIndex = newStageIndex;
            SetActiveStage(currentStageIndex);
        }
    }

    void SetActiveStage(int index)
    {
        Stages[index].SetActive(true);
        CurrentSceneNameUI.StartSceneNameAnimation();
        FoodManager.ReduceFoodState(5);
    }

}
