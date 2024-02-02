using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FoodManager : MonoBehaviour
{
    [SerializeField] private State state;                       // 플레이어 상태
    [SerializeField] private Slider foodSlider;                 // 현재 허기량을 표시하는 UI
    [SerializeField] private TextMeshProUGUI currentFoodText;   // 현재 허기량을 표시하는 텍스트
    [SerializeField] private TextMeshProUGUI currentCoinText;   // 현재 코인을 표시하는 텍스트

    private void Start()
    {
        Debug.Assert(foodSlider != null, "허기량을 확인하는 슬라이더 UI가 없습니다.");
        Debug.Assert(currentFoodText != null, "허기량을 확인하는 텍스트 UI가 없습니다.");
        Debug.Assert(currentCoinText != null, "체력을 확인하는 텍스트 UI가 없습니다.");

        foodSlider.value = state.food;
        currentFoodText.text = state.food.ToString();
        currentCoinText.text = state.money.ToString();
    }

    private void Update()
    {
        foodSlider.value = int.Parse(currentFoodText.text);
        currentFoodText.text = state.food.ToString();
        currentCoinText.text = state.money.ToString();
    }

    // 일정량의 허기를 감소시키는 메소드
    public void ReduceFoodState(int value)
    {
        int nextFood = int.Parse(currentFoodText.text) - value;

        currentFoodText.SetText(nextFood.ToString());
    }
}
