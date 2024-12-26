using UnityEngine;
using UnityEngine.SceneManagement;

public class UIOpen : MonoBehaviour
{
    public static bool isUIOpen { get; private set; } = false;
    public static UIOpen ui;

    public GameObject hpLevelBar;
    public GameObject statusUI;
    public GameObject getItemUI;
    public GameObject customUI;
    public GameObject statUI;
    public GameObject equidUI;
    public GameObject mapUI;
    public GameObject miniMap;
    public GameObject resultUI;

    private bool isDefualtOpen = true;
    private static bool isSceneLoaded = false;

    private void OnEnable()
    {
        ScriptableObject.CreateInstance<State>();

        Debug.Assert(hpLevelBar != null, "ü�� �ٰ� �����ϴ�.");
        Debug.Assert(statusUI != null, "�������ͽ� â�� �����ϴ�.");
        Debug.Assert(customUI != null, "Ŀ���� UI�� �����ϴ�.");
        Debug.Assert(getItemUI != null, "ȹ�� UI�� �����ϴ�.");
        Debug.Assert(statUI != null, "���� â UI�� �����ϴ�.");
        Debug.Assert(mapUI != null, "�� UI�� �����ϴ�.");
        Debug.Assert(miniMap != null, "�̴� �� UI�� �����ϴ�.");

        if (!isSceneLoaded)
        {
            SceneManager.sceneLoaded += NonEssentialUI;
            isSceneLoaded = true;
        }

        customUI.transform.GetChild(1).GetChild(6).GetComponent<OverlayCamera>().AddCamera();

        ui = this;
    }

    private void Update()
    {
        // ����â�� ���� Ű
        if (Input.GetKeyDown(KeyCode.E) && !statusUI.activeSelf)
            statusUI.SetActive(true);

        else if (Input.GetKeyDown(KeyCode.E) && statusUI.activeSelf)
            statusUI.SetActive(false);

        // �κ��丮 â�� ���� Ű
        if (Input.GetKeyDown(KeyCode.V) && !UINotDestroyOpen.inventory.activeSelf)
            UINotDestroyOpen.inventory.SetActive(true);

        else if (Input.GetKeyDown(KeyCode.V) && UINotDestroyOpen.inventory.activeSelf)
            UINotDestroyOpen.inventory.SetActive(false);

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

        if (!statusUI.activeSelf && !UINotDestroyOpen.inventory.activeSelf || !getItemUI.activeSelf || 
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

        miniMap.SetActive(false);

        isDefualtOpen = false;
    }

    private void OpenDefaultUI()
    {
        for (int i = 0; i < 3; i++)
            transform.GetChild(i).gameObject.SetActive(true);

        miniMap.SetActive(true);

        isDefualtOpen = true;
    }

    private void NonEssentialUI(Scene scene, LoadSceneMode scneeMode)
    {
        if (statusUI == null)
            return;

        statusUI.SetActive(false);
        getItemUI.SetActive(false);
        statUI.SetActive(false);
        equidUI.SetActive(false);
        mapUI.SetActive(false);
        customUI.SetActive(false);

        UINotDestroyOpen.inventory.SetActive(false);
    }
}