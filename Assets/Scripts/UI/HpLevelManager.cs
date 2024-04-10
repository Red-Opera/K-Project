using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpLevelManager : MonoBehaviour
{
    [SerializeField] private State state;                       // 플레이어 상태
    [SerializeField] private TextMeshProUGUI maxHpText;         // 최대 체력 텍스트
    [SerializeField] private TextMeshProUGUI currentHpText;     // 현재 체력 테스트
    [SerializeField] private TextMeshProUGUI levelText;         // 레벨 텍스트
    [SerializeField] private Slider hpSlider;                   // 체력바 슬라이더

    private int currentHp;
    private int maxHp;

    void Start()
    {
        Debug.Assert(state != null, "플레이어 스텟이 없습니다.");
        Debug.Assert(currentHpText != null, "현재 체력을 확인할 수 있는 UI가 없습니다.");
        Debug.Assert(maxHpText != null, "최대 체력을 확인할 수 있는 UI가 없습니다.");
        Debug.Assert(hpSlider != null, "체력 슬라이더가 존재하지 않습니다.");

        SliderReset();
        SetLevel();
    }

    public void Update()
    {
        
    }

    // 슬라이더 리셋
    private void SliderReset()
    {
        // 현재 상태를 가져옴
        currentHp = state.currentHp;
        maxHp = state.maxHP;

        currentHpText.text = currentHp.ToString();
        maxHpText.text = maxHp.ToString();

        hpSlider.value = currentHp / (float)maxHp;
    }

    // 레벨을 동기화 시키는 메소드
    private void SetLevel()
    {
        levelText.text = state.level.ToString();
    }

    // HP 바를 동기화 시키는 메소드
    public void GetState(State state)
    {
        this.state = state;
        SliderReset();
    }

    // 데미지를 처리하는 메소드
    public void Damage()
    {
        SliderReset();
    }
}
