using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashUI : MonoBehaviour
{
    public float fillSpeed = 0.5f;                  // 1�ʴ� ȸ���ϴ� �ӵ�

    [SerializeField] private State state;           // �÷��̾� ����
    [SerializeField] private Slider dashBarSlider;  // ���� ��ⷮ�� ǥ���ϴ� UI
    [SerializeField] private Image dashBar;         // ��� ���� ǥ���ϴ� UI
    [SerializeField] private List<Sprite> sprites;  // ��ù� ��������Ʈ

    private float maxPercent = 0.0f;

    void Start()
    {
        Debug.Assert(state != null, "�÷��̾� ������ �����ϴ�.");

        maxPercent = state.dashBarCount * 0.2f;     // �뽬 �� ������ ���� 0.2�� ����
        dashBarSlider.value = maxPercent;
    }

    void Update()
    {
        // �뽬�� ���Ͽ� �ִ밡 �ƴ� ���
        if (maxPercent > dashBarSlider.value)
            FillPercentOverTime();

        if (maxPercent < dashBarSlider.value)
            dashBarSlider.value = maxPercent;

        CheckChangeMaxPersent();
    }

    // �뽬 �ٰ� �������� �� ü��� �޼ҵ�
    private void FillPercentOverTime()
    {
        float currentPersent = dashBarSlider.value + Time.deltaTime * fillSpeed * 0.2f;

        if (currentPersent > maxPercent)
            currentPersent = maxPercent;

        dashBarSlider.value = currentPersent;
    }

    // �ִ� ��ð� �ٲ���� �� ó���Ǵ� �޼ҵ�
    private void CheckChangeMaxPersent()
    {
        maxPercent = state.dashBarCount * 0.2f;
    }
    
    // ��� �ٰ� �ø��ų� ���϶� ���Ǵ� �޼ҵ�
    public void ChangeAddDashBarCount(int count)
    {
        state.dashBarCount += count;

        // ��� �� ���̰� ������ �� �ִ뺸�� �ִ� ���� ��
        if (count > 0)
        {
            if (state.dashBarCount > 5)
                state.dashBarCount = 5;
        }

        // ��� �� ���̰� ������ �� �ּҺ��� ���� �� �ּ� ���� ��
        else
        {
            if (state.dashBarCount < 2)
                state.dashBarCount = 2;
        }

        // �ش� ��� �ٿ� �´� �̹����� ������
        dashBar.sprite = sprites[state.dashBarCount - 2];
    }

    // ��� �����ϴ� �޼ҵ�
    public bool DashUIApply()
    {
        // 0.25���� �����̴� ���� �� ū ��� (����� �� ���� ���)
        if (dashBarSlider.value >= 0.2)
        {
            dashBarSlider.value -= 0.2f;

            return true;
        }

        return false;
    }
}