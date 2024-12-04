using UnityEngine;
using UnityEngine.UI;
using System;

public class TimerManager : MonoBehaviour
{
    public GameObject bossRoom1; // BossRoom1을 드래그하여 연결
    public Text timerText;       // 현재 시간 UI
    public Text bestTimeText;    // 최단 시간 UI

    private float elapsedTime = 0f;  // 경과 시간
    private bool isTiming = false;  // 타이머 상태
    private float bestTime = float.MaxValue; // 최단 시간
    private int totalBossCount = 3;  // 총 보스 수
    private int defeatedBossCount = 0; // 처치된 보스 수

    void Start()
    {
        // 최단 시간 불러오기
        if (PlayerPrefs.HasKey("BestTime"))
        {
            bestTime = PlayerPrefs.GetFloat("BestTime");
        }
        else
        {
            bestTime = float.MaxValue; // 초기 값
        }

        // 최단 시간을 UI에 업데이트
        UpdateBestTimeUI();

        if (timerText == null || bestTimeText == null)
        {
            Debug.LogError("UI Text가 설정되지 않았습니다.");
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

        if (GameManager.info.allPlayerState.currentHp <= 0) // 플레이어가 사망 시
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
        SaveBestTime();   // 최단 시간 저장
    }

    private void UpdateTimerUI()
    {
        // 경과 시간을 UI에 표시
        timerText.text = FormatTime(elapsedTime);
    }

    private void UpdateBestTimeUI()
    {
        // 최단 시간을 UI에 표시
        if (bestTime < float.MaxValue)
        {
            bestTimeText.text = "Best Time: " + FormatTime(bestTime);
        }
        else
        {
            bestTimeText.text = "Best Time: --:--"; // 초기 상태
        }
    }

    private void SaveBestTime()
    {
        if (elapsedTime < bestTime)
        {
            bestTime = elapsedTime;
            PlayerPrefs.SetFloat("BestTime", bestTime); // 최단 시간 저장
            PlayerPrefs.Save(); // 변경 사항 저장

            // UI 업데이트
            UpdateBestTimeUI();
            Debug.Log($"New Best Time: {FormatTime(bestTime)}");
        }
        else
        {
            Debug.Log($"Current Time: {FormatTime(elapsedTime)}, Best Time: {FormatTime(bestTime)}");
        }
    }

    private string FormatTime(float timeInSeconds)
    {
        // 시간 -> 분:초 형식으로 포맷
        TimeSpan timeSpan = TimeSpan.FromSeconds(timeInSeconds);
        return string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
    }

    public void BossDefeated()
    {
        defeatedBossCount++;
        Debug.Log($"Boss defeated! Total defeated: {defeatedBossCount}/{totalBossCount}");
        
        if (defeatedBossCount >= totalBossCount) // 모든 보스가 처치된 경우
        {
            Debug.Log("All bosses defeated! Stopping timer.");
            StopTimer();
        }
    }
}
