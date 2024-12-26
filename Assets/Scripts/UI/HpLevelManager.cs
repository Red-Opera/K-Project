using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpLevelManager : MonoBehaviour
{
    [SerializeField] public MonsterState BossState;
    [SerializeField] private TextMeshProUGUI maxHpText;         // 최대 체력 텍스트
    [SerializeField] private TextMeshProUGUI currentHpText;     // 현재 체력 테스트
    [SerializeField] private TextMeshProUGUI levelText;         // 레벨 텍스트
    [SerializeField] private Slider hpSlider;                   // 체력바 슬라이더
    [SerializeField] private Image ShieldSlider;

    private void Start()
    {
        Debug.Assert(currentHpText != null, "현재 체력을 확인할 수 있는 UI가 없습니다.");
        Debug.Assert(maxHpText != null, "최대 체력을 확인할 수 있는 UI가 없습니다.");
        Debug.Assert(hpSlider != null, "체력 슬라이더가 존재하지 않습니다.");

        // 플레이어 체력바인 경우
        if (BossState == null)
        {
            GameManager.info.UpdatePlayerState();
            Debug.Assert(ShieldSlider != null, "보호막 슬라이더가 존재하지 않습니다.");

            UpdatePlayerHP();
            SetLevel();
        }

        else
            BossSliderReset();
    }

    private void Update()
    {
        if (BossState != null)
            BossSliderReset();
    }

    public void BossSliderReset()
    {
        // ���� ���¸� ������
        int currentHp = BossState.currentHp;
        int maxHp = BossState.maxHP;

        currentHpText.text = currentHp.ToString();
        maxHpText.text = maxHp.ToString();

        hpSlider.value = currentHp / (float)maxHp;
    }

    // 레벨을 동기화 시키는 메소드
    private void SetLevel()
    {
        levelText.text = GameManager.info.allPlayerState.level.ToString();
    }

    // 슬라이더 리셋
    public void UpdatePlayerHP()
    {
        // 현재 상태를 가져옴
        int maxHp = GameManager.info.allPlayerState.maxHP;
        int currentHp = GameManager.info.allPlayerState.currentHp;
        int currentShield = GameManager.info.abilityState.shield;

        currentHpText.text = currentHp.ToString();
        maxHpText.text = maxHp.ToString();

        hpSlider.value = currentHp / (float)maxHp;
        ShieldSlider.fillAmount = (currentShield / (float)maxHp) + 0.1f;
    }
}
