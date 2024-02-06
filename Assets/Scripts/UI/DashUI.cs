using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashUI : MonoBehaviour
{
    public float fillSpeed = 0.5f;                  // 1초당 회복하는 속도

    [SerializeField] private State state;           // 플레이어 상태
    [SerializeField] private Slider dashBarSlider;  // 현재 허기량을 표시하는 UI
    [SerializeField] private Image dashBar;         // 대시 바을 표시하는 UI
    [SerializeField] private List<Sprite> sprites;  // 대시바 스프라이트

    private float maxPercent = 0.0f;

    void Start()
    {
        Debug.Assert(state != null, "플레이어 스텟이 없습니다.");

        maxPercent = state.dashBarCount * 0.2f;     // 대쉬 바 개수에 따라 0.2를 곱함
        dashBarSlider.value = maxPercent;
    }

    void Update()
    {
        // 대쉬로 인하여 최대가 아닌 경우
        if (maxPercent > dashBarSlider.value)
            FillPercentOverTime();

        if (maxPercent < dashBarSlider.value)
            dashBarSlider.value = maxPercent;

        CheckChangeMaxPersent();
    }

    // 대쉬 바가 감소했을 때 체우는 메소드
    private void FillPercentOverTime()
    {
        float currentPersent = dashBarSlider.value + Time.deltaTime * fillSpeed * 0.2f;

        if (currentPersent > maxPercent)
            currentPersent = maxPercent;

        dashBarSlider.value = currentPersent;
    }

    // 최대 대시가 바뀌었을 때 처리되는 메소드
    private void CheckChangeMaxPersent()
    {
        maxPercent = state.dashBarCount * 0.2f;
    }
    
    // 대시 바가 늘리거나 줄일때 사용되는 메소드
    public void ChangeAddDashBarCount(int count)
    {
        state.dashBarCount += count;

        // 대시 바 길이가 증가할 때 최대보다 최대 값로 둠
        if (count > 0)
        {
            if (state.dashBarCount > 5)
                state.dashBarCount = 5;
        }

        // 대시 바 길이가 감소할 때 최소보다 작을 때 최소 값로 둠
        else
        {
            if (state.dashBarCount < 2)
                state.dashBarCount = 2;
        }

        // 해당 대시 바에 맞는 이미지를 보여줌
        dashBar.sprite = sprites[state.dashBarCount - 2];
    }

    // 대시 적용하는 메소드
    public bool DashUIApply()
    {
        // 0.25보다 슬라이더 값이 더 큰 경우 (대시할 수 있을 경우)
        if (dashBarSlider.value >= 0.2)
        {
            dashBarSlider.value -= 0.2f;

            return true;
        }

        return false;
    }
}