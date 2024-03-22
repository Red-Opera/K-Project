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

        Debug.Assert(statusUI != null, "스테이터스 창이 없습니다.");
        Debug.Assert(inventoryUI != null, "인벤토리 창이 없습니다.");
    }

    void Update()
    {
        // 스탯창을 여는 키
        if (Input.GetKeyDown(KeyCode.E) && !statusUI.activeSelf)
            statusUI.SetActive(true);

        else if (Input.GetKeyDown(KeyCode.E) && statusUI.activeSelf)
            statusUI.SetActive(false);

        // 인벤토리 창을 여는 키
        if (Input.GetKeyDown(KeyCode.V) && !inventoryUI.activeSelf)
            inventoryUI.SetActive(true);

        else if (Input.GetKeyDown(KeyCode.V) && inventoryUI.activeSelf)
            inventoryUI.SetActive(false);

        if (statUI.activeSelf && isDefualtOpen)
            ClostDefultUI();

        else if (!statUI.activeSelf && !isDefualtOpen)
            OpenDefaultUI();
            
    }

    // 아이템 획득 창을 보여주는 메소드
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
