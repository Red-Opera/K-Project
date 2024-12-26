using TMPro;
using UnityEngine;

public class SaleItemFrame : MonoBehaviour
{
    [HideInInspector] public int saleItemIndex;             // 판매될 아이템 장비 인벤토리 인덱스

    [SerializeField] private TextMeshProUGUI cost;          // 판매 금액 텍스트

    public void SaleItem()
    {
        // 판매 했을 때 금액을 구함
        int nowMoney = GameManager.info.playerState.money;
        nowMoney += int.Parse(cost.text.Replace(",", ""));

        GameManager.info.playerState.money = nowMoney;
        GameManager.info.UpdatePlayerState();

        string removeItemName = Inventory.staticSlots.GetChild(saleItemIndex).GetChild(0).GetComponent<EquidState>().state.nickName;
        removeItemName = removeItemName.Replace(" ", "");
        ResultUI.getItemList.Remove(removeItemName);

        // 인벤토리 아이템 제거
        Destroy(InventroyPosition.inventory.transform.GetChild(saleItemIndex).GetChild(0).gameObject);

        gameObject.SetActive(false);
    }
}
