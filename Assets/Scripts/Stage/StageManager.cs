using UnityEngine;

public class StageManager : MonoBehaviour
{
    public GameObject[] Stages;
    public GameObject BossStageC;
    public GameObject BossStage;
    public GameObject Boss;
    public GameObject BossHPBar; // 추가 
    public Transform playerStartPosition; // 씬 진입 시 플레이어가 시작할 위치

    private int currentStageIndex;
    public float fadeDuration = 1f; // 페이드 인/아웃에 걸리는 시간

    void Start()
    {
        // 플레이어 가져오기
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // 플레이어 위치 이동
        if (player != null && playerStartPosition != null)
        {
            player.transform.position = playerStartPosition.position;
        }

        // 챌린지 모드 확인
        if (GameManager.instance != null && GameManager.instance.challenge)
        {
            Debug.Log("챌린지 모드 시작 → 보스 스테이지만 활성화");

            // 모든 일반 스테이지 끄기
            foreach (GameObject stage in Stages)
            {
                stage.SetActive(false);
            }

            // 보스 스테이지만 켜기
            BossStage.SetActive(true);

            // Boss HPBar 활성화
            BossHPBar.SetActive(true);
        }
        else
        {
            player.transform.position = new Vector3(0, 0, 0.17f); // 플레이어 시작 위치 설정
            // 기본 스테이지 설정
            currentStageIndex = 0;
            SetActiveStage(currentStageIndex);
        }
    }

    void Update()
    {
        BossClear();
    }

    public void ChangeStage(int direction)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        int newStageIndex = currentStageIndex + direction;

        if (newStageIndex >= 0 && newStageIndex < Stages.Length)
        {
            Stages[currentStageIndex].SetActive(false);
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

    public void BossClear()
    {
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");

        if (boss != null && !BossHPBar.activeSelf)
        {
            BossHPBar.SetActive(true);
        }
    }

    public void rewardMap()
    {
        BossStage.SetActive(false);
        BossHPBar.SetActive(false);
        BossStageC.SetActive(true);
    }
}
