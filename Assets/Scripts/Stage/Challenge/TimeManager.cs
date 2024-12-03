using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class TimerManager : MonoBehaviour
{
    public GameObject bossRoom1; // BossRoom1을 드래그하여 연결
    public Text timerText;

    private float elapsedTime = 0f; // 경과 시간
    private bool isTiming = false; // 타이머 상태
    private PMove playerMove;
    private int totalBossCount = 3; // 총 보스 수
    private int defeatedBossCount = 0; // 처치한 보스 수

    void Start()
    {
        playerMove = FindObjectOfType<PMove>();
        if (playerMove == null)
        {
            Debug.LogError("PMove 스크립트를 찾을 수 없습니다.");
        }

        if (timerText == null)
        {
            Debug.LogError("TimerText가 설정되지 않았습니다.");
        }
    }

    private void Update()
    {
        if (bossRoom1.activeSelf && !isTiming) // BossRoom1이 활성화되었을 때 타이머 시작
        {
            StartTimer();
        }

        if (isTiming)
        {
            elapsedTime += Time.deltaTime; // 경과 시간 계산
            UpdateTimerUI();
        }

        if (GameManager.info.allPlayerState.currentHp <= 0) // PMove가 Die에서 레이어를 12로 변경
        {
            StopTimer();
        }
    }

    private void StartTimer()
    {
        elapsedTime = 0f; // 타이머 초기화
        isTiming = true;  // 타이머 활성화
    }

    private void StopTimer()
    {
        isTiming = false; // 타이머 정지
    }

    private void UpdateTimerUI()
    {
        // 경과 시간을 시간, 분, 초로 변환
        TimeSpan timeSpan = TimeSpan.FromSeconds(elapsedTime);
        timerText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
    }

    public void BossDefeated()
    {
        defeatedBossCount++;
        if (defeatedBossCount >= totalBossCount)
        {
            Debug.Log("All bosses defeated! Stopping timer.");
            StopTimer();
        }
    }

}
