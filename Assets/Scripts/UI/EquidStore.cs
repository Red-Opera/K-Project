using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EquidStore : MonoBehaviour
{
    [SerializeField] private GameObject inventslot;                 // �κ��丮 ���Ե��� �����ϴ� ��������
    [SerializeField] private GameObject slots;                      // ������ �����ϴ� ������Ʈ
    [SerializeField] private GameObject details;                    // �ش� �������� ���� �������� �����ϴ� ������Ʈ
    [SerializeField] private GameObject item;                       // ������ �� ���Ǵ� ������
    [SerializeField] private Button buyButton;                      // ���� ��ư
    [SerializeField] private List<SelectableItem> selectables;      // ������ �� �ִ� ������
    [SerializeField] int contentprintPerSec;                        // 1�ʴ� ��µǴ� ���� ��

    private TextMeshProUGUI outText;// ���γ����� ����� ��ġ
    private string sceneName;       // ���� �� �̸�
    private string typeContent;     // ���γ��뿡 �Է��� ����
    private bool isType = false;    // ���� ������ ���� �ִ��� ����

    public void Start()
    {
        Debug.Assert(inventslot != null, "�κ���� ������ �����ϴ�.");
        Debug.Assert(slots != null, "��� ������ ������ ������ �����ϴ�.");
        Debug.Assert(details != null, "�������� ������ �����ִ� �������� �����ϴ�.");
        Debug.Assert(item != null, "������ �� ���Ǵ� �������� �����ϴ�.");
        Debug.Assert(buyButton != null, "���� ��ư�� �����ϴ�.");

        buyButton.onClick.AddListener(() => EquidStoreItem.BuyItem());
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // ���� �ۼ� ȿ���� ����
            if (isType)
            {
                outText.text = typeContent;
                StopAllCoroutines();

                isType = false;
            }
        }
    }

    // ������ ���� ������ ������Ʈ�ϴ� �޼ҵ�
    private void ItemUpdate()
    {
        for (int i = 0; i < slots.transform.childCount; i++)
        {
            // ������ �� �ִ� ������ ������
            Transform slot = slots.transform.GetChild(i);

            // ���õǾ� �ִ� ��� �������� ������
            while (slot.childCount >= 1)
                Destroy(slot.GetChild(0).gameObject);

            // ������ �������� ������
            GameObject newItem = Instantiate(item, slot);
            newItem.GetComponent<MoveInventory>().enabled = false;
            newItem.AddComponent<EquidStoreItem>().thisIndex = i;

            // ���� ������ �������� �������� ������
                SelectedRandomItem(newItem, i);
        }
    }

    // ������ �������� �������� ���ϴ� �޼ҵ�
    private void SelectedRandomItem(GameObject newItem, int selectedIndex)
    {
        // �������� ������ ������ �������� ����
        int itemIndex = Random.Range(0, selectables.Count);

        // ������ �̹����� Ÿ������ �ٲ�
        newItem.GetComponent<InventableEquipment>().inventableEquipment = selectables[itemIndex].equipmentType;
        newItem.transform.GetChild(0).GetComponent<Image>().sprite = selectables[itemIndex].sprite;

        // ������ �������� ��ġ�� �������� ��� ����
        Transform changeDetail = details.transform.GetChild(selectedIndex);

        changeDetail.GetChild(1).GetComponent<TextMeshProUGUI>().text = selectables[itemIndex].cost.ToString("#,##0");
        changeDetail.GetChild(2).GetChild(0).GetComponent<Image>().sprite = selectables[itemIndex].sprite;
        changeDetail.GetChild(3).GetComponent<TextMeshProUGUI>().text = selectables[itemIndex].content;
    }

    // �������� ������ �ۼ��� �� ���ڸ� �ۼ��ϴ� ȿ���� ��Ÿ���� �޼ҵ�
    private IEnumerator TypingContent()
    {
        // �ѱ��ڰ� ��µǴ� �ð��� ����
        float printDelay = 1f / contentprintPerSec;

        outText.text = "";

        // �ѱ��ھ� ���
        for (int i = 0; i < typeContent.Length; i++)
        {
            outText.text += typeContent[i];

            // 1�ʴ� contentprintPerSec���� ��ŭ ���
            yield return new WaitForSeconds(printDelay);
        }

        isType = false;
    }

    // �������� ���ý� ����Ǵ� �޼ҵ�
    public void OnSelectedItem(int selectedIndex, ref int currentOnDisplayIndex)
    {
        // ���� �ε����� ���� ��� ���� ����
        if (selectedIndex == currentOnDisplayIndex)
            return;

        // �ۼ� ���� �κ��� ��� �ۼ��ϰ� �ۼ�ȿ���� ������
        if (outText != null)
        {
            outText.text = typeContent;
            StopAllCoroutines();
        }

        // ������ ���� �ִ� ���������� ���� ������ ���������� �ٲ�
        details.transform.GetChild(currentOnDisplayIndex).gameObject.SetActive(false);
        details.transform.GetChild(selectedIndex).gameObject.SetActive(true);

        // ���� ���������� �ε����� �����
        currentOnDisplayIndex = selectedIndex;

        // ����� �������� ��ġ�� ����� �ؽ�Ʈ�� ������
        outText = details.transform.GetChild(selectedIndex).GetChild(3).GetComponent<TextMeshProUGUI>();
        typeContent = outText.text;

        isType = true;
        StartCoroutine(TypingContent());
    }

    // ���� �޼ҵ�
    public void BuyEffect(int selectedIndex)
    {
        Transform selectItem = details.transform.GetChild(selectedIndex);
        
        int cost = int.Parse(selectItem.GetChild(1).GetComponent<TextMeshProUGUI>().text.Replace(",", ""));

        if (GameManager.info.allPlayerState.money < cost)
            return;

        // ������ �������� ������
        GameObject getItem = slots.transform.GetChild(selectedIndex).GetChild(0).gameObject;

        // �ش� �������� �κ���� �߰�
        InventroyPosition.CallAddItem(
            getItem.transform.GetChild(0).GetComponent<Image>().sprite.name, 
            getItem.GetComponent<InventableEquipment>().inventableEquipment);

        // ���絷 ����ȭ
        GameManager.info.playerState.money -= cost;
        GameManager.info.UpdatePlayerState();
    }

    public void OnEnable()
    {
        // ���� ���� ������
        string currentScene = SceneManager.GetActiveScene().name;

        // ���� �ٲ���� ��쿡�� ������ ������Ʈ�ǵ��� ����
        if (currentScene != sceneName)
        {
            sceneName = currentScene;

            ItemUpdate();
        }
    }
}

// ������ �� �ִ� �������� ����
[System.Serializable]
public class SelectableItem
{
    public Sprite sprite;                   // �̹���
    public string content;                  // ����
    public int cost;                        // ���
    public EquipmentState equipmentType;    // ���� Ÿ��
}