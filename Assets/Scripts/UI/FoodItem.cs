using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FoodItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isEnable = true;
    public int index = 0;                               // 자신이 위치한 인덱스
    public bool isHotFood = false;                      // 해당 음식이 뜨거운 음식인지 여부          

    [SerializeField] private Sprite selectedImage;      // 선택했을 때 이미지
    [SerializeField] private Sprite unSelectedImage;    // 선택하지 않았을 때 이미지

    private Image displayImage;                         // 선택 이미지를 표시할 UI
    private static Image displayFoodImage;              // 마우스에 올라간 음식 이미지를 표시할 UI 
    private static GameObject foodStreamImage;          // 음식 수증기를 표시할 UI

    [SerializeField] private TextMeshProUGUI costText;          // 가격을 표시하는 UI
    [SerializeField] private TextMeshProUGUI addFoodText;       // 추가 허기를 표시하는 UI
    [SerializeField] private TextMeshProUGUI addHPText;         // 추가 체력을 표시하는 UI

    [SerializeField] private GameObject addStatName;            // 추가 스탯 증가하는 값의 이름을 저장하는 오브젝트
    [SerializeField] private GameObject addStatValue;           // 추가 스텟 증가하는 값을 저장하는 오브젝트

    private static int selectedIndex = -1;

    private Vector3 foodImagePos;
    private Vector3 foodStreamPos;

    [SerializeField] private GameObject buyEffect;              // 가격을 표시하는 UI
    [SerializeField] private Transform buyEffectTransform;      // 가격을 표시하는 UI 시작 위치
    [SerializeField] private TextMeshProUGUI remainCoinText;    // 현재 남은 골드를 표시하는 UI
    [SerializeField] private TextMeshProUGUI remainFoodText;    // 현재 남은 허기를 표시하는 UI
    [SerializeField] private TextMeshProUGUI remainMaxFoodText; // 최대 허기를 표시하는 UI
    [SerializeField] private Slider currentFoodSlider;          // 현재 허기량을 나타내는 슬라이더
    [SerializeField] private AudioSource audiosource;           // 구매할 소리를 출력할 컴포넌트
    [SerializeField] private AudioClip buySound;                // 구매하는 소리
    [SerializeField] private AudioClip noSound;                 // 구매 실패 소리
    [SerializeField] private AudioClip mouseOver;               // 마우스 올라갈 때 소리

    [SerializeField] private SerializableDictionary<string, string> stateKoreaToEng;       // 추가 스탯의 한국어를 영어로 바꿔주는 배열

    public void Start()
    {
        displayImage = GetComponent<Image>();
        Debug.Assert(displayImage != null, "바뀐 이미지를 출력할 UI가 없습니다.");

        if (displayFoodImage == null)
            displayFoodImage = GameObject.Find("FoodPosition").GetComponent<Image>();

        if (foodStreamImage == null)
            foodStreamImage = GameObject.Find("FoodStream");

        Debug.Assert(displayFoodImage != null, "선택한 음식을 출력할 UI가 없습니다.");
        Debug.Assert(foodStreamImage != null, "수증기를 출력하는 UI가 없습니다.");

        foodImagePos = displayFoodImage.transform.position;
        foodStreamPos = foodStreamImage.transform.position;

        Debug.Assert(buyEffect != null, "구매 효과 UI가 없습니다.");
        Debug.Assert(remainCoinText != null, "남은 골드를 표시하는 UI가 없습니다.");
        Debug.Assert(remainFoodText != null, "남은 허기를 표시하는 UI가 없습니다.");
        Debug.Assert(remainMaxFoodText != null, "최대 허기를 표시하는 UI가 없습니다.");
        Debug.Assert(audiosource != null, "소리를 출력할 컴포넌트가 없습니다.");

        remainCoinText.text = GameManager.info.playerState.money.ToString("#,##0G");
        remainFoodText.text = GameManager.info.playerState.food.ToString();
        currentFoodSlider.value = GameManager.info.playerState.food / 100.0f;

        foodStreamImage.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(1))
            BuyItem();
    }

    private void BuyItem()
    {
        if (selectedIndex == -1 || selectedIndex != index || !isEnable)
            return;

        int cost = int.Parse(costText.text);

        if (GameManager.info.playerState.money < -cost)
        {
            // 돈이 부족할 경우 처리
            audiosource.PlayOneShot(noSound);

            return;
        }

        int food = GameManager.info.playerState.food + int.Parse(addFoodText.text);

        if (food > 100)
        {
            // 허기가 꽉찼을 경우 처리
            audiosource.PlayOneShot(noSound);

            return;
        }

        if (food < 0)
        {
            // 허기가 -가 된 경우
            audiosource.PlayOneShot(noSound);

            return;
        }

        audiosource.PlayOneShot(buySound);

        // 구매 효과 생성
        GameObject newBuyEffect = Instantiate(buyEffect, buyEffectTransform);
        newBuyEffect.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = costText.text + "G";

        // 구매된 비용 반영
        GameManager.info.playerState.money += cost;
        remainCoinText.text = GameManager.info.playerState.money.ToString("#,##0G");

        // 허기 반영
        GameManager.info.playerState.food = food;
        remainFoodText.text = GameManager.info.playerState.food.ToString();
        currentFoodSlider.value = GameManager.info.playerState.food / 100.0f;

        // 스탯 반영
        SetStat();

        // 초기화
        selectedIndex = -1;
        displayFoodImage.sprite = null;
        displayFoodImage.color = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
        foodStreamImage.SetActive(false);

        // 다시 구매 할 수 없도록 설정
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);
        transform.GetChild(transform.childCount - 1).gameObject.SetActive(true);

        isEnable = false;
    }

    private void SetStat()
    {
        GameManager.info.addFoodState.maxHP += int.Parse(addHPText.text); 

        for (int i = 0; i < addStatName.transform.childCount; i++)
        {
            if (!addStatName.transform.GetChild(i).gameObject.activeSelf)
                break;

            string name = addStatName.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text;
            double value = double.Parse(addStatValue.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text);

            GameManager.info.AddFoodState(stateKoreaToEng[name], value);
        }
    }

    private IEnumerator SettingFood()
    {
        // x축으로 -300만큼 이동
        Vector3 targetFoodImagePos = foodImagePos + new Vector3(-300, 0, 0);
        Vector3 targetFoodStreamPos = foodStreamPos + new Vector3(-300, 0, 0);

        // 이동
        float duration = 0.25f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            displayFoodImage.transform.position = Vector3.Lerp(targetFoodImagePos, foodImagePos, elapsed / duration);
            foodStreamImage.transform.position = Vector3.Lerp(targetFoodStreamPos, foodStreamPos, elapsed / duration);

            elapsed += Time.deltaTime;
            yield return null;
        }

        displayFoodImage.transform.position = foodImagePos;
        foodStreamImage.transform.position = foodStreamPos;
    }

    // 마우스가 해당 오브젝트 위에 있을 때
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        // 이미 산적이 있을 경우 중지
        if (!isEnable)
            return;

        if (displayFoodImage.color.a < 0.1f)
            displayFoodImage.color = Color.white;

        displayImage.sprite = selectedImage;
        displayFoodImage.sprite = transform.GetChild(2).GetComponent<Image>().sprite;

        if (isHotFood)
            foodStreamImage.SetActive(true);

        selectedIndex = index;

        audiosource.PlayOneShot(mouseOver);

        StartCoroutine(SettingFood());
    }

    // 마우스가 해당 오브젝트 밖에 있을 때
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        displayImage.sprite = unSelectedImage;

        foodStreamImage.SetActive(false);
    }
}