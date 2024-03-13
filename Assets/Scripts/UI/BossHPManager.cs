using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossHPManager : MonoBehaviour
{
    [SerializeField] private State playerState;             // 플레이어 상태
    [SerializeField] private TextMeshProUGUI coin;          // 플레이어 현재 골드량

    private List<GameObject> dropItem;                      // 보스가 드랍하는 아이템 (추후 추가 예정)

    private void Start()
    {
        Debug.Assert(playerState != null, "플레이어 스텟이 없습니다.");
        Debug.Assert(coin != null, "플레이어 골드를 표시하는 텍스트가 없습니다.");
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
            // 추후 아이템 드랍 처리
        }
    }

    private void DrapMoney(int cost)
    {
        playerState.money += cost;

        coin.text = playerState.money.ToString("#,##0");
    }
}