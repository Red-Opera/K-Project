using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;  // ���� ���� �ݾ� �ؽ�Ʈ

    public void Start()
    {
        Debug.Assert(coinText != null, "���� ���� ����� �ؽ�Ʈ�� �����ϴ�.");

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
