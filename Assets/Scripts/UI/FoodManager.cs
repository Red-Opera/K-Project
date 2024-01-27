using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FoodManager : MonoBehaviour
{
    [SerializeField] private Slider foodSlider;                 // ���� ��ⷮ�� ǥ���ϴ� UI
    [SerializeField] private TextMeshProUGUI currentFoodText;   // ���� ��ⷮ�� ǥ���ϴ� �ؽ�Ʈ

    private void Start()
    {
        Debug.Assert(foodSlider != null, "��ⷮ�� Ȯ���ϴ� �����̴� UI�� �����ϴ�.");
    }

    private void Update()
    {
        foodSlider.value = int.Parse(currentFoodText.text);
    }

    // �������� ��⸦ ���ҽ�Ű�� �޼ҵ�
    public void ReduceFoodState(int value)
    {
        int nextFood = int.Parse(currentFoodText.text) - value;

        currentFoodText.SetText(nextFood.ToString());
    }
}
