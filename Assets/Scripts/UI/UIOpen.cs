using UnityEngine;

public class UIOpen : MonoBehaviour
{
    [SerializeField] private GameObject statusUI;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject getItemUI;

    void Start()
    {
        ScriptableObject.CreateInstance<State>();

        Debug.Assert(statusUI != null, "스테이터스 창이 없습니다.");
        Debug.Assert(inventoryUI != null, "인벤토리 창이 없습니다.");
    }

    void Update()
    {
        // 스탯창을 여는 키
        if (Input.GetKeyDown(KeyCode.E))
            statusUI.SetActive(true);

        // 인벤토리 창을 여는 키
        if (Input.GetKeyDown(KeyCode.V))
            inventoryUI.SetActive(true);
    }

    // 아이템 획득 창을 보여주는 메소드
    public void ShowGetItemUI(Sprite sprite, string itemName, Color color)
    {
        getItemUI.SetActive(true);
        GetItemUI itemUI = getItemUI.GetComponent<GetItemUI>();

        if (!GetItemUI.isShowUI)
            StartCoroutine(itemUI.ShowItemUI(sprite, itemName, color));
    }
}
