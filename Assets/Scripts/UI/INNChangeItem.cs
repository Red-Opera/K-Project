using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class INNChangeItem : MonoBehaviour
{
    [SerializeField] private GameObject[] foodList;     // ������ ������ �� �ִ� ����Ʈ
    [SerializeField] private Sprite[] selectableList;   // ���� ������ ���� ����Ʈ
    [SerializeField] private FoodState[] foodInfo;     // ���� ���� ����Ʈ

    [SerializeField] private float range = 0.2f;        // ���� �� ȿ�� ����

    void Start()
    {
        Debug.Assert(foodList.Length != 0, "������ ������ �� �ִ� �������� �����ϴ�.");

    }

    public void OnEnable()
    {
        for (int i = 0; i < foodList.Length; i++)
        {
            int foodIndex = Random.Range(0, selectableList.Length);
            float addRange = Random.Range(-range, range);

            string selectedName = selectableList[foodIndex].texture.name;
            foodList[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = selectedName;
            foodList[i].transform.GetChild(2).GetComponent<Image>().sprite = selectableList[foodIndex];


        }
    }
}
