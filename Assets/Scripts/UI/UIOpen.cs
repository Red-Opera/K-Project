using UnityEngine;

public class UIOpen : MonoBehaviour
{
    [SerializeField] private GameObject statusUI;
    [SerializeField] private GameObject inventoryUI;

    void Start()
    {
        ScriptableObject.CreateInstance<State>();

        Debug.Assert(statusUI != null, "�������ͽ� â�� �����ϴ�.");
        Debug.Assert(inventoryUI != null, "�κ��丮 â�� �����ϴ�.");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            statusUI.SetActive(true);

        if (Input.GetKeyDown(KeyCode.V))
            inventoryUI.SetActive(true);

    }
}
