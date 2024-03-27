using UnityEngine;

public class UIOpen : MonoBehaviour
{
    public GameObject statusUI;
    public GameObject inventoryUI;
    public GameObject getItemUI;
    public GameObject statUI;
    public GameObject equidUI;

    private bool isDefualtOpen = true;

    void Start()
    {
        ScriptableObject.CreateInstance<State>();

        Debug.Assert(statusUI != null, "�������ͽ� â�� �����ϴ�.");
        Debug.Assert(inventoryUI != null, "�κ��丮 â�� �����ϴ�.");
    }

    void Update()
    {
        // ����â�� ���� Ű
        if (Input.GetKeyDown(KeyCode.E) && !statusUI.activeSelf)
            statusUI.SetActive(true);

        else if (Input.GetKeyDown(KeyCode.E) && statusUI.activeSelf)
            statusUI.SetActive(false);

        // �κ��丮 â�� ���� Ű
        if (Input.GetKeyDown(KeyCode.V) && !inventoryUI.activeSelf)
            inventoryUI.SetActive(true);

        else if (Input.GetKeyDown(KeyCode.V) && inventoryUI.activeSelf)
            inventoryUI.SetActive(false);

        if (statUI.activeSelf && isDefualtOpen)
            ClostDefultUI();

        else if (!statUI.activeSelf && !isDefualtOpen)
            OpenDefaultUI();
            
    }

    // ������ ȹ�� â�� �����ִ� �޼ҵ�
    public void ShowGetItemUI(Sprite sprite, string itemName, Color color)
    {
        getItemUI.SetActive(true);
        GetItemUI itemUI = getItemUI.GetComponent<GetItemUI>();

        if (!GetItemUI.isShowUI)
            StartCoroutine(itemUI.ShowItemUI(sprite, itemName, color));
    }

    private void ClostDefultUI()
    {
        for (int i = 0; i < 3; i++)
            transform.GetChild(i).gameObject.SetActive(false);

        isDefualtOpen = false;
    }

    private void OpenDefaultUI()
    {
        for (int i = 0; i < 3; i++)
            transform.GetChild(i).gameObject.SetActive(true);

        isDefualtOpen = true;
    }
}
