using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;  // ���� ���� �ݾ� �ؽ�Ʈ
    [SerializeField] private Transform slots;          // ���� ������Ʈ

    public static Dictionary<int, GameObject> inventorySlotItem;   // ��� �κ��丮 ���� ������
    private static Transform staticSlots;                           // ���� ������Ʈ (Static ����)
    private static bool isLeft = true;                              // ���� ���� ��� ������ ����ϴ� ����

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

    public void OnEnable()
    {
        if (staticSlots == null)
            staticSlots = slots;

        InventoryUpdate(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    // �� �Ѿ�� �� ��� ��� �ı� ���� �޼ҵ�
    public static void InventoryDataUpdate()
    {
        // ���� ��� ��� ���� �ʱ�ȭ
        inventorySlotItem = new Dictionary<int, GameObject>();

        // ��� ��� ����
        for (int i = 0; i < staticSlots.transform.childCount; i++)
        {
            Transform currentSlot = staticSlots.transform.GetChild(i);

            if (currentSlot.name == "Banned")
                continue;

            if (currentSlot.childCount > 0)
            {
                Transform item = currentSlot.GetChild(0);

                if (item.name == "Banned")
                    continue;

                // ��� �����͸� �����ϰ� �Ⱥ��̰� �ٸ� ������ �̵�
                inventorySlotItem.Add(i, item.gameObject);
                item.SetParent(null);
                item.GetChild(0).position = new Vector2(-9999, -9999);

                DontDestroyOnLoad(item.gameObject);
            }
        }
    }

    // �� �Ѿ �� ��� �κ��丮 ���ġ
    public static void InventoryUpdate(Scene scene, LoadSceneMode mode)
    {
        // ���� ���������� Map�� ���
        if (inventorySlotItem == null || staticSlots == null)
            return;

        // ��� ��� ���ؼ� ��� �κ��丮�� �̵�
        foreach (int key in inventorySlotItem.Keys)
        {
            inventorySlotItem[key].transform.SetParent(staticSlots.transform.GetChild(key));
            inventorySlotItem[key].transform.GetChild(0).localPosition = Vector2.zero;
            inventorySlotItem[key].transform.localScale = Vector2.one;
        }
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
            Transform slot = staticSlots.GetChild(0);

            if (slot.childCount > 1)
            {
                // �� ���⿡ ���� ������ ��� �� ��� ������ �߰� ���� ������ ����
                Transform weaphon = slot.GetChild(0);
                GameManager.AddStates(GameManager.info.addWaphonState, weaphon.GetComponent<EquidState>().state);
            }

            Transform subWeaphon = null;
            if (staticSlots.GetChild(1).childCount >= 1)
                subWeaphon = staticSlots.GetChild(1).GetChild(0);

            if (subWeaphon != null)
                GameManager.AddStates(GameManager.info.addWaphonState, subWeaphon.GetComponent<EquidState>().state);
        }

        // ������ ��� ������ ����� ���
        else
        {
            Transform slot = staticSlots.GetChild(2);

            if (slot.childCount > 1)
            {
                Transform weaphon = slot.GetChild(0);
                GameManager.AddStates(GameManager.info.addWaphonState, weaphon.GetComponent<EquidState>().state);
            }

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