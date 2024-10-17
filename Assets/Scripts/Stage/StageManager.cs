using UnityEngine;

public class StageManager : MonoBehaviour
{

    public GameObject[] Stages;
    public GameObject BossStageC;
    public GameObject BossStage;
    public GameObject Boss;
    public GameObject BossHPBar; // 추가 
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

    //보스를 잡으면 상자와 다음 레벨로 넘어가는 포탈이 나옴
    public void BossClear()
    {
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");

        if (boss != null)
        {
            // Boss가 활성화되어 있을 때 BossHPBar를 활성화
            BossHPBar.SetActive(true);
        }
        else
        {
            if (BossStage.activeSelf)
            {
                // Boss를 클리어했을 때 BossStage와 BossHPBar를 비활성화
                BossStage.SetActive(false);
                BossHPBar.SetActive(false);
                BossStageC.SetActive(true);
            }
        }
    }
}

