using UnityEngine;

public class UIOpen : MonoBehaviour
{
    [SerializeField] private GameObject statusUI;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject getItemUI;

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
    }

    // ������ ȹ�� â�� �����ִ� �޼ҵ�
    public void ShowGetItemUI(Sprite sprite, string itemName, Color color)
    {
        getItemUI.SetActive(true);
        GetItemUI itemUI = getItemUI.GetComponent<GetItemUI>();

        if (!GetItemUI.isShowUI)
            StartCoroutine(itemUI.ShowItemUI(sprite, itemName, color));
    }
}
