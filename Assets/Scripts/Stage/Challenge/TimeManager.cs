using UnityEngine;

public class TimerManager : MonoBehaviour
{
    private float startTime;
    private bool timerRunning = false;

    void Update()
    {
        if (timerRunning)
        {
            // 측정 중인 시간 출력
            float elapsedTime = Time.time - startTime;
            Debug.Log("Elapsed Time: " + elapsedTime + " seconds");
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

    public void TriggerStartTimer(Collider other)
    {
        if (other.CompareTag("Player") && !timerRunning)
        {
            // 특정 오브젝트에 닿았을 때 시간 측정 시작
            StartTimer();
        }
    }
}

public class TimerTrigger : MonoBehaviour
{
    public TimerManager timerManager;

    private void OnTriggerEnter(Collider other)
    {
        if (timerManager != null)
        {
            timerManager.TriggerStartTimer(other);
        }
    }
}
