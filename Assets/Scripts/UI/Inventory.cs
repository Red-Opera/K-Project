using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;  // 현재 소유 금액 텍스트

    public void Start()
    {
        Debug.Assert(coinText != null, "현재 돈을 출력할 텍스트가 없습니다.");

        UpdateCoin();
    }

    public void Update()
    {
        UpdateCoin();
    }

    public void UpdateCoin()
    {
        coinText.text = GameManager.info.allPlayerState.money.ToString("#,##0");
    }
}
