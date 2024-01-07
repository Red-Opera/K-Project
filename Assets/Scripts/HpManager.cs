using System.Collections;
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
        Debug.Assert(state != null, "�÷��̾� ���¸� ã�� �� �����ϴ�.");
        Debug.Assert(currentHpText != null, "���� ü���� Ȯ���� �� �ִ� UI�� �����ϴ�.");
        Debug.Assert(maxHpText != null, "�ִ� ü���� Ȯ���� �� �ִ� UI�� �����ϴ�.");
        Debug.Assert(hpSlider != null, "ü�� �����̴��� �������� �ʽ��ϴ�.");

        currentHp = state.currentHp;
        maxHp = state.maxHP;

        currentHpText.text = currentHp.ToString();
        maxHpText.text = maxHp.ToString();

        hpSlider.value = currentHp / (float)maxHp;
    }

    void Update()
    {
        
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
