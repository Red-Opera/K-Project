using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class INNChangeItem : MonoBehaviour
{
    [SerializeField] private GameObject[] foodList;     // 음식을 선택할 수 있는 리스트
    [SerializeField] private Sprite[] selectableList;   // 선택 가능한 음식 리스트
    [SerializeField] private FoodState[] foodInfo;      // 음식 선택 리스트
    [SerializeField] private AudioClip openSound;       // 상점 열릴 때 소리     

    [SerializeField] private float range = 0.2f;        // 가격 및 효율 범위

    private AudioSource audio;      // 소리를 출력하는 컴포넌트
    private float addRange;

    private void OnEnable()
    {
        if (audio == null)
        {
            Debug.Assert(foodList.Length != 0, "음식을 선택할 수 있는 프레임이 없습니다.");

            // 프레임 별로 자신이 몇번째인지 확인
            for (int i = 0; i < foodList.Length; i++)
                foodList[i].GetComponent<FoodItem>().index = i;

            audio = GetComponent<AudioSource>();
            Debug.Assert(audio, "오디오 컴포넌트가 없습니다.");
        }

        audio.PlayOneShot(openSound);
        BackgroundSound.StartOtherCilp();

        // 모든 정보를 키고 감사 텍스트는 끔
        for (int i = 0; i < foodList.Length; i++)
        {
            int childCount = foodList[i].transform.childCount;

            for (int j = 0; j < childCount; j++)
                foodList[i].transform.GetChild(j).gameObject.SetActive(true);

            foodList[i].transform.GetChild(childCount - 1).gameObject.SetActive(false);
            foodList[i].GetComponent<FoodItem>().isEnable = true;
        }

        StartCoroutine(DefaultSetting());
    }

    private void OnDisable()
    {
        BackgroundSound.StopOtherCilp();
    }

    private IEnumerator DefaultSetting()
    {
        // 게임 메니저가 없다면 잠시 대기
        int failCount = 0;
        while (GameManager.info == null && failCount < 100)
        {
            failCount++;
            yield return null;
        }

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

            // 해당 음식이 뜨거운 음식인지 알려줌
            foodList[i].GetComponent<FoodItem>().isHotFood = food.isHotFood;

            int addStateCount = food.addState.Count;                // 추가 효과 개수를 가져옴
            Transform addList = foodList[i].transform.GetChild(3);  // 추가 정보를 표시할 위치

            addRange = Random.Range(-range, range);     // 상품에 대한 가치를 구함
            SetEfficiency(i);

            AddStateTurnOff(addList);                   // 모든 추가 상태를 끔

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

            // 기본 정보를 표시할 위치
            Transform baseList = foodList[i].transform.GetChild(4);

            // 추가 효과 개수 만큼 반복
            for (int j = 0; j < 3; j++)
            {
                // 추가 상태 이름과 가치를 UI에 출력함
                if (j != 2)
                    ShowValue(baseList.GetChild(1).GetChild(j).GetComponent<TextMeshProUGUI>(), food.baseState[j].value, true);

                else
                    ShowValue(baseList.GetChild(1).GetChild(j).GetComponent<TextMeshProUGUI>(), food.baseState[j].value, true, true);
            }
        }
    }

    // 세부 사항을 모두 끄는 메소드
    private void AddStateTurnOff(Transform addList)
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                addList.GetChild(j).GetChild(i).gameObject.SetActive(false);
    }

    private void ShowValue(TextMeshProUGUI outText, float value, bool isBase = false, bool isMoney = false)
    {
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

        // 돈일 경우 돈을 가지고 있는 양에 따라 색깔을 바꾸고 무조건 음수로 변경
        if (isMoney)
        {
            outText.color = (GameManager.info.playerState.money - (int)value >= 0) ? Color.green : Color.red;
            outText.text = "-";
        }

        // 기본 정보일 경우 모두 정수형으로 표현
        if (isBase)
            outText.text += ((int)value).ToString();

        else if (Mathf.Abs(value) < 0.1f)
            outText.text += value.ToString("#,##0.##");

        else
            outText.text += value.ToString("#,##0.#");
    }

    private void SetEfficiency(int i)
    {
        TextMeshProUGUI setText = foodList[i].transform.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>();

        if (addRange < 0)
            setText.color = Color.red;

        else
            setText.color = Color.blue;


        setText.text = (1 + addRange).ToString("#,##0.#%");
    }
}
