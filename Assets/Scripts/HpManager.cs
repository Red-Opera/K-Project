using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpManager : MonoBehaviour
{
    public State state;
    public TextMeshProUGUI currentHpText;
    public TextMeshProUGUI maxHpText;
    public Slider hpSlider;

    private int currentHp;
    private int maxHp;

    void Start()
    {
        Debug.Assert(currentHpText != null, "현재 체력을 확인할 수 있는 UI가 없습니다.");
        Debug.Assert(maxHpText != null, "최대 체력을 확인할 수 있는 UI가 없습니다.");
        Debug.Assert(hpSlider != null, "체력 슬라이더가 존재하지 않습니다.");

        if (state != null)
            SliderReset();
    }

    void Update()
    {
        
    }

    // 슬라이더 리셋
    private void SliderReset()
    {
        currentHp = state.currentHp;
        maxHp = state.maxHP;

        currentHpText.text = currentHp.ToString();
        maxHpText.text = maxHp.ToString();

        hpSlider.value = currentHp / (float)maxHp;
    }

    public void GetState(State state)
    {
        this.state = state;
        SliderReset();
    }

    public void Damage(int damage)
    {
        if (currentHp >= damage)
            currentHp -= damage;

        else
            currentHp = 0;

        state.currentHp = currentHp;
        currentHpText.SetText(currentHp.ToString());

        hpSlider.value = currentHp / (float)maxHp;
    }
}
