using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor.SceneManagement;


public class StageManager : MonoBehaviour
{

    public GameObject[] Stages;
    public GameObject BossStageC;
    public GameObject BossStage;
    public GameObject Boss;
    private int currentStageIndex;
    public float fadeDuration = 1f; // 페이드 인/아웃에 걸리는 시간

    void Start()
    {
        // 초기 스테이지 설정
        currentStageIndex = 0;
        SetActiveStage(currentStageIndex);

    }

    void Update()
    {
        BossClear();
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
    //Map으로 이동
    public void LoadMap()
    {
        Debug.Log("마을로 돌아갑니다.");

        SceneManager.LoadScene("Map");
    }

    //보스를 잡으면 상자와 다음 레벨로 넘어가는 포탈이 나옴
    public void BossClear()
    {
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");

        if (boss == null)
        {
            if(BossStage.activeSelf)
            {
                BossStage.SetActive(false);

                BossStageC.SetActive(true);
            }
        }
    }

}

