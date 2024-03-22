using TMPro;
using UnityEngine;

public class SaleItemFrame : MonoBehaviour
{
    [HideInInspector] public int saleItemIndex;             // �Ǹŵ� ������ ��� �κ��丮 �ε���

    [SerializeField] private GameObject inventorySlot;      // �κ��丮 ����
    [SerializeField] private TextMeshProUGUI cost;          // �Ǹ� �ݾ� �ؽ�Ʈ

    public void SaleItem()
    {
        // �Ǹ� ���� �� �ݾ��� ����
        int nowMoney = GameManager.info.playerState.money;
        nowMoney += int.Parse(cost.text.Replace(",", ""));

        GameManager.info.playerState.money = nowMoney;
        GameManager.info.UpdatePlayerState();

        // �κ��丮 ������ ����
        Destroy(inventorySlot.transform.GetChild(saleItemIndex).GetChild(0).gameObject);

        gameObject.SetActive(false);
    }
}
