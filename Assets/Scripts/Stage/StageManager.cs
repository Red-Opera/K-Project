using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StageManager : MonoBehaviour
{

    public GameObject[] Stages;
    public GameObject BossStage;
    private int currentStageIndex;
    public float fadeDuration = 1f; // 페이드 인/아웃에 걸리는 시간

    void Start()
    {
        // 초기 스테이지 설정
        currentStageIndex = 0;
        SetActiveStage(currentStageIndex);

    }

    public void ChangeStage(int direction)
    {
         GameObject player = GameObject.FindGameObjectWithTag("Player");

        // Player 오브젝트를 찾은 경우
        if (player != null)
        {
            // Player 오브젝트의 위치를 (0, 0, 0)으로 설정
            player.transform.position = Vector3.zero;
        }

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
    }
    //Map으로 이동
    public void LoadMap()
    {
        Debug.Log("마을로 돌아갑니다.");

        SceneManager.LoadScene("Map");
    }
    
}

