using UnityEngine;
using UnityEngine.UI;

public class DashUI : MonoBehaviour
{
    public float fillSpeed = 0.5f;                  // 1초당 회복하는 속도

    [SerializeField] private State state;           // 플레이어 상태
    [SerializeField] private Slider dashBarSlider;  // 현재 허기량을 표시하는 UI

    private float maxPercent = 0.0f;

    void Start()
    {
        Debug.Assert(state != null, "플레이어 스텟이 없습니다.");

        maxPercent = state.dashBarCount * 0.25f;    // 대쉬 바 개수에 따라 0.25를 곱함
        dashBarSlider.value = maxPercent;
    }

    void Update()
    {
        // 대쉬로 인하여 최대가 아닌 경우
        if (maxPercent > dashBarSlider.value)
            FillPercentOverTime();

        CheckChangeMaxPersent();
    }

    // 대쉬 바가 감소했을 때 체우는 메소드
    private void FillPercentOverTime()
    {
        float currentPersent = dashBarSlider.value + Time.deltaTime * fillSpeed * 0.25f;

        if (currentPersent > maxPercent)
            maxPercent = currentPersent;

        dashBarSlider.value = currentPersent;
    }

    // 최대 대시가 바뀌었을 때 처리되는 메소드
    private void CheckChangeMaxPersent()
    {
        maxPercent = state.dashBarCount * 0.25f;
    }
}
