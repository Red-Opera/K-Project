using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FoodManager : MonoBehaviour
{
    [SerializeField] private Slider foodSlider;                 // 현재 허기량을 표시하는 UI
    [SerializeField] private TextMeshProUGUI currentFoodText;   // 현재 허기량을 표시하는 텍스트

    private void Start()
    {
        Debug.Assert(foodSlider != null, "허기량을 확인하는 슬라이더 UI가 없습니다.");
    }

    private void Update()
    {
        foodSlider.value = int.Parse(currentFoodText.text);
    }

    // 일정량의 허기를 감소시키는 메소드
    public void ReduceFoodState(int value)
    {
        int nextFood = int.Parse(currentFoodText.text) - value;

        currentFoodText.SetText(nextFood.ToString());
    }
}
