using UnityEngine;

public class UIOpen : MonoBehaviour
{
    [SerializeField] private GameObject statusUI;
    [SerializeField] private GameObject inventoryUI;

    void Start()
    {
        ScriptableObject.CreateInstance<State>();

        Debug.Assert(statusUI != null, "스테이터스 창이 없습니다.");
        Debug.Assert(inventoryUI != null, "인벤토리 창이 없습니다.");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            statusUI.SetActive(true);

        if (Input.GetKeyDown(KeyCode.V))
            inventoryUI.SetActive(true);

    }
}
