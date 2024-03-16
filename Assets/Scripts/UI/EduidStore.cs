using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EduidStore : MonoBehaviour
{
    [SerializeField] private GameObject slots;
    [SerializeField] private GameObject item;
    [SerializeField] private List<GameObject> selectableItem;

    private string sceneName;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void ItemUpdate()
    {
        for (int i = 0; i < slots.transform.childCount; i++)
        {
            // ������ �� �ִ� ������ ������
            Transform slot = slots.transform.GetChild(i);

            // ���õǾ� �ִ� ��� �������� ������
            while (slot.childCount >= 1)
                Destroy(slot.GetChild(0).gameObject);

            GameObject newItem = Instantiate(item, slot);
            newItem.GetComponent<MoveInventory>().enabled = false;


            SelectedRandomItem();
        }
    }

    private void SelectedRandomItem()
    {
        int itemIndex = Random.Range(0, selectableItem.Count);


    }

    public void OnEnable()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == sceneName)
        {
            sceneName = currentScene;

            ItemUpdate();
        }
    }
}
