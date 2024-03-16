using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FoodManager : MonoBehaviour
{
    [SerializeField] private State state;                       // �÷��̾� ����
    [SerializeField] private Slider foodSlider;                 // ���� ��ⷮ�� ǥ���ϴ� UI
    [SerializeField] private TextMeshProUGUI currentFoodText;   // ���� ��ⷮ�� ǥ���ϴ� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI currentCoinText;   // ���� ������ ǥ���ϴ� �ؽ�Ʈ

    private void Start()
    {
        Debug.Assert(foodSlider != null, "��ⷮ�� Ȯ���ϴ� �����̴� UI�� �����ϴ�.");
        Debug.Assert(currentFoodText != null, "��ⷮ�� Ȯ���ϴ� �ؽ�Ʈ UI�� �����ϴ�.");
        Debug.Assert(currentCoinText != null, "ü���� Ȯ���ϴ� �ؽ�Ʈ UI�� �����ϴ�.");

        foodSlider.value = state.food;
        currentFoodText.text = state.food.ToString();
        currentCoinText.text = state.money.ToString();
    }

    private void Update()
    {
        // �÷��̾� ���¿��� �÷��̾� �� �� ��ⷮ�� ���� �� �ؽ�Ʈ�� ����ϰ� ��� �����̴� ���� ������
        currentFoodText.text = state.food.ToString();
        currentCoinText.text = state.money.ToString();
        foodSlider.value = int.Parse(currentFoodText.text) / 100f;
    }

    // �������� ��⸦ ���ҽ�Ű�� �޼ҵ�
    public void ReduceFoodState(int value)
    {
        int nextFood = int.Parse(currentFoodText.text) - value;

        currentFoodText.SetText(nextFood.ToString());
    }
}
