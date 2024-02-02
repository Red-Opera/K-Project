using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private State state;               // 플레이어 상태
    [SerializeField] private TextMeshProUGUI coinText;  // 최대 체력 텍스트

    public void Start()
    {
        Debug.Assert(state != null, "플레이어 스텟이 없습니다.");
        Debug.Assert(coinText != null, "현재 돈을 출력할 텍스트가 없습니다.");

        UpdateCoin();
    }

    public void Update()
    {
        
    }

    public void UpdateCoin()
    {
        coinText.text = state.money.ToString();
    }
}
