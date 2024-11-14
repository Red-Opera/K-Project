using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    private float startTime;
    private bool timerRunning = false;
    public GameObject startObject; // 특정 시작 오브젝트 지정
    public Text timerText; // 시간을 표시할 UI Text 컴포넌트

    void Update()
    {
        if (timerRunning)
        {
            float elapsedTime = Time.time - startTime;
            timerText.text = "Elapsed Time: " + elapsedTime.ToString("F2") + " seconds";
        }
    }

    public void StartTimer()
    {
        startTime = Time.time;
        timerRunning = true;
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    public float GetElapsedTime()
    {
        return Time.time - startTime;
    }

    public void TriggerStartTimer(Collider2D other)
    {
        if (other.CompareTag("Player") && !timerRunning)
        {
            StartTimer();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 지정된 startObject와 플레이어가 충돌했을 때만 타이머 시작
        if (other.gameObject == startObject && other.CompareTag("Player"))
        {
            TriggerStartTimer(other);
        }
    }
}
