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

        Debug.Assert(hpLevelBar != null, "체력 바가 없습니다.");
        Debug.Assert(statusUI != null, "스테이터스 창이 없습니다.");
        Debug.Assert(customUI != null, "커스텀 UI가 없습니다.");
        Debug.Assert(getItemUI != null, "획득 UI가 없습니다.");
        Debug.Assert(statUI != null, "스탯 창 UI가 없습니다.");
        Debug.Assert(mapUI != null, "맵 UI가 없습니다.");
        Debug.Assert(miniMap != null, "미니 맵 UI가 없습니다.");

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
        // 스탯창을 여는 키
        if (Input.GetKeyDown(KeyCode.E) && !statusUI.activeSelf)
            statusUI.SetActive(true);

        else if (Input.GetKeyDown(KeyCode.E) && statusUI.activeSelf)
            statusUI.SetActive(false);

        // 인벤토리 창을 여는 키
        if (Input.GetKeyDown(KeyCode.V) && !UINotDestroyOpen.inventory.activeSelf)
            UINotDestroyOpen.inventory.SetActive(true);

        else if (Input.GetKeyDown(KeyCode.V) && UINotDestroyOpen.inventory.activeSelf)
            UINotDestroyOpen.inventory.SetActive(false);

        // 맵 UI 창을 여는 키
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

    // 아이템 획득 창을 보여주는 메소드
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