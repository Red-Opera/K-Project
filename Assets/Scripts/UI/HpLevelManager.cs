using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpLevelManager : MonoBehaviour
{
    [SerializeField] private State state;                       // �÷��̾� ����
    [SerializeField] private TextMeshProUGUI maxHpText;         // �ִ� ü�� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI currentHpText;     // ���� ü�� �׽�Ʈ
    [SerializeField] private TextMeshProUGUI levelText;         // ���� �ؽ�Ʈ
    [SerializeField] private Slider hpSlider;                   // ü�¹� �����̴�

    private int currentHp;
    private int maxHp;

    void Start()
    {
        Debug.Assert(state != null, "�÷��̾� ������ �����ϴ�.");
        Debug.Assert(currentHpText != null, "���� ü���� Ȯ���� �� �ִ� UI�� �����ϴ�.");
        Debug.Assert(maxHpText != null, "�ִ� ü���� Ȯ���� �� �ִ� UI�� �����ϴ�.");
        Debug.Assert(hpSlider != null, "ü�� �����̴��� �������� �ʽ��ϴ�.");

        SliderReset();
        SetLevel();
    }

    public void Update()
    {
        
    }

    // �����̴� ����
    private void SliderReset()
    {
        // ���� ���¸� ������
        currentHp = state.currentHp;
        maxHp = state.maxHP;

        currentHpText.text = currentHp.ToString();
        maxHpText.text = maxHp.ToString();

        hpSlider.value = currentHp / (float)maxHp;
    }

    // ������ ����ȭ ��Ű�� �޼ҵ�
    private void SetLevel()
    {
        levelText.text = state.level.ToString();
    }

    // HP �ٸ� ����ȭ ��Ű�� �޼ҵ�
    public void GetState(State state)
    {
        this.state = state;
        SliderReset();
    }

    // �������� ó���ϴ� �޼ҵ�
    public void Damage()
    {
        SliderReset();
    }
}
