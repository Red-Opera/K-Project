using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class INNChangeItem : MonoBehaviour
{
    [SerializeField] private GameObject[] foodList;     // 음식을 선택할 수 있는 리스트
    [SerializeField] private Sprite[] selectableList;   // 선택 가능한 음식 리스트
    [SerializeField] private FoodState[] foodInfo;     // 음식 선택 리스트

    [SerializeField] private float range = 0.2f;        // 가격 및 효율 범위

    void Start()
    {
        Debug.Assert(foodList.Length != 0, "음식을 선택할 수 있는 프레임이 없습니다.");

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
