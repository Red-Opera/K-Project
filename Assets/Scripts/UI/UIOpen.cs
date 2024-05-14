using UnityEngine;
using UnityEngine.SceneManagement;

public class UIOpen : MonoBehaviour
{
    public static bool isUIOpen { get; private set; } = false;

    public GameObject statusUI;
    public GameObject inventoryUI;
    public GameObject getItemUI;
    public GameObject customUI;
    public GameObject statUI;
    public GameObject equidUI;
    public GameObject mapUI;
    public GameObject resultUI;

    private bool isDefualtOpen = true;

    private void OnEnable()
    {
        ScriptableObject.CreateInstance<State>();

        Debug.Assert(statusUI != null, "�������ͽ� â�� �����ϴ�.");
        Debug.Assert(inventoryUI != null, "�κ��丮 â�� �����ϴ�.");
        Debug.Assert(customUI != null, "Ŀ���� UI�� �����ϴ�.");
        Debug.Assert(getItemUI != null, "ȹ�� UI�� �����ϴ�.");
        Debug.Assert(statUI != null, "���� â UI�� �����ϴ�.");
        Debug.Assert(mapUI != null, "�� UI�� �����ϴ�.");

        SceneManager.sceneLoaded += NonEssentialUI;

        customUI.transform.GetChild(1).GetChild(6).GetComponent<OverlayCamera>().AddCamera();
    }

    private void Update()
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

        // �� UI â�� ���� Ű
        if (Input.GetKeyDown(KeyCode.I) && !mapUI.activeSelf)
            mapUI.SetActive(true);

        else if (Input.GetKeyDown(KeyCode.I) && mapUI.activeSelf)
            mapUI.SetActive(false);

        if (statUI.activeSelf && isDefualtOpen)
            ClostDefultUI();

        else if (!statUI.activeSelf && !isDefualtOpen)
            OpenDefaultUI();

        if (customUI.activeSelf && isDefualtOpen)
            ClostDefultUI();

        else if (!statUI.activeSelf && !isDefualtOpen)
            OpenDefaultUI();

        if (!statusUI.activeSelf && !inventoryUI.activeSelf || !getItemUI.activeSelf || 
            !statUI.activeSelf || !equidUI.activeSelf || !mapUI.activeSelf || 
            !resultUI.activeSelf || !customUI.activeSelf)
            isUIOpen = false;

        else
            isUIOpen = true;
    }

    // ������ ȹ�� â�� �����ִ� �޼ҵ�
    public void ShowGetItemUI(Sprite sprite, string itemName, EquipmentState euqidState, Color color)
    {
        getItemUI.SetActive(true);
        GetItemUI itemUI = getItemUI.GetComponent<GetItemUI>();

        if (!GetItemUI.isShowUI)
            StartCoroutine(itemUI.ShowItemUI(sprite, itemName, euqidState, color));
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

    private void NonEssentialUI(Scene scene, LoadSceneMode scneeMode)
    {
        statusUI.SetActive(false);
        inventoryUI.SetActive(false);
        getItemUI.SetActive(false);
        statUI.SetActive(false);
        equidUI.SetActive(false);
        mapUI.SetActive(false);
        customUI.SetActive(false);
    }
}