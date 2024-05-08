using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;  // ���� ���� �ݾ� �ؽ�Ʈ
    [SerializeField] private Transform slots;          // ���� ������Ʈ

    private static Transform staticSlots;  // ���� ������Ʈ (Static ����)
    private static bool isLeft = true;      // ���� ���� ��� ������ ����ϴ� ����

    private void Start()
    {
        Debug.Assert(coinText != null, "���� ���� ����� �ؽ�Ʈ�� �����ϴ�.");

        UpdateCoin();
    }

    private void Update()
    {
        UpdateCoin();
    }

    public void UpdateCoin()
    {
        coinText.text = GameManager.info.allPlayerState.money.ToString("#,##0");
    }

    private void OnEnable()
    {
        if (staticSlots == null)
            staticSlots = slots;
    }

    // ���� �����ϰ� �ִ� �̹��� ��������Ʈ ��ȯ
    public static Sprite GetCurrentWeaphonSprite()
    {
        if (isLeft)
            return staticSlots.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite;

        return staticSlots.GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>().sprite;
    }

    // ���� ���� ���� ��� ���� �ɷ�ġ�� ������Ʈ�ϴ� �޼ҵ�
    public static void EquidSlotStateUpdate()
    {
        // ��� ���¸� �ʱ�ȭ ��
        GameManager.info.addWaphonState = ScriptableObject.CreateInstance<State>();

        // ���� ��� ������ ����ϴ� ���
        if (isLeft)
        {
            // �� ���⿡ ���� ������ ��� �� ��� ������ �߰� ���� ������ ����
            Transform weaphon = staticSlots.GetChild(0).GetChild(0);
            GameManager.AddStates(GameManager.info.addWaphonState, weaphon.GetComponent<EquidState>().state);

            Transform subWeaphon = null;
            if (staticSlots.GetChild(1).childCount >= 1)
                subWeaphon = staticSlots.GetChild(1).GetChild(0);

            if (subWeaphon != null)
                GameManager.AddStates(GameManager.info.addWaphonState, subWeaphon.GetComponent<EquidState>().state);
        }

        // ������ ��� ������ ����� ���
        else
        {
            Transform weaphon = staticSlots.GetChild(2).GetChild(0);
            GameManager.AddStates(GameManager.info.addWaphonState, weaphon.GetComponent<EquidState>().state);

            Transform subWeaphon = null;
            if (staticSlots.GetChild(3).childCount >= 1)
                subWeaphon = staticSlots.GetChild(3).GetChild(0);

            if (subWeaphon != null)
                GameManager.AddStates(GameManager.info.addWaphonState, subWeaphon.GetComponent<EquidState>().state);
        }

        // ��� ���� ���Կ� ���ؼ� 
        for (int i = 4; i < 8; i++)
        {
            // ���� ������ ������
            Transform currentSlot = staticSlots.GetChild(i);

            // ���� �ش� ���Կ� ��� ���� ��� ���� �������� �Ѿ
            if (currentSlot.childCount == 0)
                continue;

            // �� ����� �ɷ�ġ�� ��� ����
            GameManager.AddStates(GameManager.info.addWaphonState, currentSlot.GetChild(0).GetComponent<EquidState>().state);
        }

        // ��� �÷��̾� ���¸� ����ȭ ��
        GameManager.info.UpdatePlayerState();
    }
}