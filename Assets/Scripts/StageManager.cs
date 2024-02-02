using UnityEngine;
using System.Collections;

public class StageManager : MonoBehaviour
{
    public string[] stageNames; // 스테이지 이름 배열
    private int currentStageIndex = 0; // 현재 스테이지 인덱스

    void Update()
    {
        // 위쪽 화살표 키를 누르면 다음 스테이지로 이동
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            LoadNextStage();
        }

        // 아래쪽 화살표 키를 누르면 이전 스테이지로 이동
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            LoadPreviousStage();
        }
    }

    // 다음 스테이지로 이동하는 함수
    void LoadNextStage()
    {
        currentStageIndex = (currentStageIndex + 1) % stageNames.Length;
        LoadStage(currentStageIndex);
    }

    // 이전 스테이지로 이동하는 함수
    void LoadPreviousStage()
    {
        currentStageIndex = (currentStageIndex - 1 + stageNames.Length) % stageNames.Length;
        LoadStage(currentStageIndex);
    }

    // 스테이지를 로드하는 함수
    void LoadStage(int stageIndex)
    {
        string stageName = stageNames[stageIndex];
        Debug.Log("Loading stage: " + stageName);

        // 여기에 실제 스테이지 로드하는 코드를 추가할 수 있습니다.
    }
}
