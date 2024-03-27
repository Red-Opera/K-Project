using TMPro;
using UnityEngine;

public class SaleItemFrame : MonoBehaviour
{
    [HideInInspector] public int saleItemIndex;             // 판매될 아이템 장비 인벤토리 인덱스

    [SerializeField] private GameObject inventorySlot;      // 인벤토리 스롯
    [SerializeField] private TextMeshProUGUI cost;          // 판매 금액 텍스트

    public void SaleItem()
    {
        // 판매 했을 때 금액을 구함
        int nowMoney = GameManager.info.playerState.money;
        nowMoney += int.Parse(cost.text.Replace(",", ""));

        GameManager.info.playerState.money = nowMoney;
        GameManager.info.UpdatePlayerState();

        // 인벤토리 아이템 제거
        Destroy(inventorySlot.transform.GetChild(saleItemIndex).GetChild(0).gameObject);

        gameObject.SetActive(false);
    }
}
