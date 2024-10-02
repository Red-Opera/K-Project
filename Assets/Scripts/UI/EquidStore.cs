using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EquidStore : MonoBehaviour
{
    [SerializeField] public List<SelectableItem> selectables;                   // ������ �� �ִ� ������

    [SerializeField] private GameObject slots;                      // ������ �����ϴ� ������Ʈ
    [SerializeField] private GameObject details;                    // �ش� �������� ���� �������� �����ϴ� ������Ʈ
    [SerializeField] private GameObject item;                       // ������ �� ���Ǵ� ������
    [SerializeField] private GameObject buyEffect;                  // ���� ȿ�� ������Ʈ
    [SerializeField] private Transform buyEffectTransform;          // ���� ȿ�� ���� ��ġ
    [SerializeField] private Button buyButton;                      // ���� ��ư
    [SerializeField] private GameObject saleUI;                     // �Ǹ� UI
    [SerializeField] private AudioClip openSound;                   // ���� �� ���� �Ҹ�
    [SerializeField] private AudioClip buySound;                    // ������ �� ���� �Ҹ�
    [SerializeField] int contentprintPerSec;                        // 1�ʴ� ��µǴ� ���� ��

    private TextMeshProUGUI outText;// ���γ����� ����� ��ġ
    private AudioSource playerAudio;      // �Ҹ� ��� ������Ʈ
    private string sceneName;       // ���� �� �̸�
    private string typeContent;     // ���γ��뿡 �Է��� ����
    private bool isType = false;    // ���� ������ ���� �ִ��� ����

    public void Start()
    {
        Debug.Assert(slots != null, "��� ������ ������ ������ �����ϴ�.");
        Debug.Assert(details != null, "�������� ������ �����ִ� �������� �����ϴ�.");
        Debug.Assert(item != null, "������ �� ���Ǵ� �������� �����ϴ�.");
        Debug.Assert(buyButton != null, "���� ��ư�� �����ϴ�.");
        Debug.Assert(saleUI != null, "�Ǹ� UI�� �����ϴ�.");

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

        if (saleUI.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            saleUI.SetActive(false);

        else if (!saleUI.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            gameObject.SetActive(false);

        if (!UINotDestroyOpen.inventory.activeSelf)
            UINotDestroyOpen.inventory.SetActive(true);
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

        while (selectables[itemIndex].level == EquidLevel.EQUIPMENT_LEVEL_LEGEND)
            itemIndex = Random.Range(0, selectables.Count);

        // ������ �̹����� Ÿ������ �ٲ�
        newItem.GetComponent<InventableEquipment>().inventableEquipment = selectables[itemIndex].equipmentType;
        newItem.transform.GetChild(0).GetComponent<Image>().sprite = selectables[itemIndex].sprite;

        // ��� ������ ���� ������
        EquidState equidState = newItem.GetComponent<EquidState>();
        equidState.state = ScriptableObject.CreateInstance<State>();

        // ��� �ʵ带 �����ͼ� ������.
        FieldInfo[] allFields = typeof(State).GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (FieldInfo field in allFields)
            field.SetValue(equidState.state, field.GetValue(selectables[itemIndex].state));

        // ������ �������� ��ġ�� �������� ��� ����
        Transform changeDetail = details.transform.GetChild(selectedIndex);

        changeDetail.GetChild(1).GetComponent<TextMeshProUGUI>().text = selectables[itemIndex].state.money.ToString("#,##0");
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

        // ������ �������� �������� ����� �����ϰų� �������� ������ ���� �ִٸ� �ݿ� ����
        Transform selectFrame = slots.transform.GetChild(selectedIndex);
        if (GameManager.info.allPlayerState.money < cost || selectFrame.childCount == 0)
            return;

        // ������ �������� ������
        GameObject getItem = selectFrame.GetChild(0).gameObject;

        // �ش� �������� �κ��丮�� �߰�
        //InventroyPosition.CallAddItem(
        //    getItem.transform.GetChild(0).GetComponent<Image>().sprite.name, 
        //    getItem.GetComponent<InventableEquipment>().inventableEquipment,
        //    getItem.GetComponent<EquidState>());

        ResultUI.GetItem(getItem.transform.GetChild(0).GetComponent<Image>().sprite.name);

        // ���絷 ����ȭ
        GameManager.info.playerState.money -= cost;
        GameManager.info.UpdatePlayerState();

        // ���� ȿ��
        GameObject newBuyEffect = Instantiate(buyEffect, buyEffectTransform);
        newBuyEffect.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = cost + "G";

        playerAudio.PlayOneShot(buySound);

        Destroy(getItem);
    }

    public void OnEnable()
    {
        if (playerAudio == null)
        {
            playerAudio = GetComponent<AudioSource>();
            Debug.Assert(playerAudio != null, "�Ҹ� ������Ʈ�� �����ϴ�.");
        }

        playerAudio.PlayOneShot(openSound);

        // ���� ���� ������
        string currentScene = SceneManager.GetActiveScene().name;

        UINotDestroyOpen.inventory.SetActive(true);
        saleUI.SetActive(false);

        // ���� �ٲ���� ��쿡�� ������ ������Ʈ�ǵ��� ����
        if (currentScene != sceneName)
        {
            sceneName = currentScene;

            ItemUpdate();
        }
    }

    public void OnDisable()
    {
        UINotDestroyOpen.inventory.SetActive(false);
        saleUI.SetActive(false);
    }
}

public enum EquidLevel
{
    EQUIPMENT_LEVEL_NORMAL = 0,         // �븻 ���
    EQUIPMENT_LEVEL_RERE = 1,           // ���� ���
    EQUIPMENT_LEVEL_EPIC = 2,           // ���� ���
    EQUIPMENT_LEVEL_UNIQUE = 3,         // ����ũ ���
    EQUIPMENT_LEVEL_LEGEND = 4,         // ������ ����
}

// ������ �� �ִ� �������� ����
[System.Serializable]
public class SelectableItem
{
    public Sprite sprite;                   // �̹���
    public string content;                  // ����
    public EquipmentState equipmentType;    // ���� Ÿ��
    public EquidLevel level;                // ��� ���

    public State state;                     // ���� ����
}