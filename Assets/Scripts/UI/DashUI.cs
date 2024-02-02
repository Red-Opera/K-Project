using UnityEngine;
using UnityEngine.UI;

public class DashUI : MonoBehaviour
{
    public float fillSpeed = 0.5f;                  // 1�ʴ� ȸ���ϴ� �ӵ�

    [SerializeField] private State state;           // �÷��̾� ����
    [SerializeField] private Slider dashBarSlider;  // ���� ��ⷮ�� ǥ���ϴ� UI

    private float maxPercent = 0.0f;

    void Start()
    {
        Debug.Assert(state != null, "�÷��̾� ������ �����ϴ�.");

        maxPercent = state.dashBarCount * 0.25f;    // �뽬 �� ������ ���� 0.25�� ����
        dashBarSlider.value = maxPercent;
    }

    void Update()
    {
        // �뽬�� ���Ͽ� �ִ밡 �ƴ� ���
        if (maxPercent > dashBarSlider.value)
            FillPercentOverTime();

        CheckChangeMaxPersent();
    }

    // �뽬 �ٰ� �������� �� ü��� �޼ҵ�
    private void FillPercentOverTime()
    {
        float currentPersent = dashBarSlider.value + Time.deltaTime * fillSpeed * 0.25f;

        if (currentPersent > maxPercent)
            maxPercent = currentPersent;

        dashBarSlider.value = currentPersent;
    }

    // �ִ� ��ð� �ٲ���� �� ó���Ǵ� �޼ҵ�
    private void CheckChangeMaxPersent()
    {
        maxPercent = state.dashBarCount * 0.25f;
    }
}
