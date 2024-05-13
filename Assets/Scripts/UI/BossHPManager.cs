using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossHPManager : MonoBehaviour
{
    [SerializeField] private State playerState;             // �÷��̾� ����
    [SerializeField] private TextMeshProUGUI coin;          // �÷��̾� ���� ��差

    private List<GameObject> dropItem;                      // ������ ����ϴ� ������ (���� �߰� ����)

    private void Start()
    {
        Debug.Assert(playerState != null, "�÷��̾� ������ �����ϴ�.");

        coin = GameObject.Find("RemainCoinText").GetComponent<TextMeshProUGUI>();
        Debug.Assert(coin != null, "�÷��̾� ��带 ǥ���ϴ� �ؽ�Ʈ�� �����ϴ�.");

    }

    public void Drap(int cost)
    {
        DrapItem();
        DrapMoney(cost);
    }

    private void DrapItem()
    {
        foreach (GameObject item in dropItem)
        {
            // ���� ������ ��� ó��
        }
    }

    private void DrapMoney(int cost)
    {
        ResultUI.GetGold(cost);

        coin.text = playerState.money.ToString("#,##0");
    }
}