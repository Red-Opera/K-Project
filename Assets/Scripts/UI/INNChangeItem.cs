using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class INNChangeItem : MonoBehaviour
{
    [SerializeField] private GameObject[] foodList;     // 음식을 선택할 수 있는 리스트
    [SerializeField] private Sprite[] selectableList;   // 선택 가능한 음식 리스트
    [SerializeField] private FoodState[] foodInfo;      // 음식 선택 리스트

    [SerializeField] private float range = 0.2f;        // 가격 및 효율 범위

    void Start()
    {
        Debug.Assert(foodList.Length != 0, "음식을 선택할 수 있는 프레임이 없습니다.");

    }

    public void OnEnable()
    {
        for (int i = 0; i < foodList.Length; i++)
        {
            // 음식 이미지 중 랜덤으로 하나 뽑음
            int foodIndex = Random.Range(0, selectableList.Length);

            // 선택한 이미지의 이름을 저장하고 이름과 표시할 UI의 이미지를 선택한 음식 이미지로 교환
            string selectedName = selectableList[foodIndex].texture.name;
            foodList[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = selectedName;
            foodList[i].transform.GetChild(2).GetComponent<Image>().sprite = selectableList[foodIndex];

            // 랜덤으로 선택한 음식을 음식 정보 배열에서 찾음
            FoodState food = null;
            for (int j = 0; j < foodInfo.Length; j++)
            {
                if (foodInfo[j].foodName == selectedName)
                {
                    food = foodInfo[j];
                    break;
                }

                // 음식 정보를 찾을 수 없을 경우
                if (j == foodList.Length - 1)
                    Debug.Assert(false, "해당 이름의 음식 정보가 존재하지 않습니다.");
            }
            
            int addStateCount = food.addState.Count;                // 추가 효과 개수를 가져옴
            Transform addList = foodList[i].transform.GetChild(3);  // 추가 정보를 표시할 위치

            AddStateTurnOff(addList);

            // 추가 효과 개수 만큼 반복
            for (int j = 0; j < addStateCount; j++)
            {
                // 먼저 정보를 표시할 UI를 모두 활성화
                for (int k = 0; k < 3; k++)
                    addList.GetChild(k).GetChild(j).gameObject.SetActive(true);

                // 추가 상태 이름과 가치를 UI에 출력함
                addList.GetChild(1).GetChild(j).GetComponent<TextMeshProUGUI>().text = food.addState[j].stateName;
                ShowValue(addList.GetChild(2).GetChild(j).GetComponent<TextMeshProUGUI>(), food.addState[j].value);
            }
        }
    }

    private void AddStateTurnOff(Transform addList)
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                addList.GetChild(j).GetChild(i).gameObject.SetActive(false);
    }

    private void ShowValue(TextMeshProUGUI outText, float value)
    {
        float addRange = Random.Range(-range, range);

        value += addRange * value;

        if (value >= 0)
        {
            outText.color = Color.green;
            outText.text = "+";
        }

        else
        {
            outText.color = Color.red;
            outText.text = "";
        }

        if (Mathf.Abs(value) < 0.1f)
            outText.text += value.ToString("#,##0.##");

        else
            outText.text += value.ToString("#,##0.#");
    }
}
