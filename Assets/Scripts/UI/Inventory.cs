using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private State state;               // �÷��̾� ����
    [SerializeField] private TextMeshProUGUI coinText;  // �ִ� ü�� �ؽ�Ʈ

    public void Start()
    {
        Debug.Assert(state != null, "�÷��̾� ������ �����ϴ�.");
        Debug.Assert(coinText != null, "���� ���� ����� �ؽ�Ʈ�� �����ϴ�.");

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
