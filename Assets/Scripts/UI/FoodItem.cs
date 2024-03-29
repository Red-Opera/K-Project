using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FoodItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isEnable = true;
    public int index = 0;                               // �ڽ��� ��ġ�� �ε���
    public bool isHotFood = false;                      // �ش� ������ �߰ſ� �������� ����          

    [SerializeField] private Sprite selectedImage;      // �������� �� �̹���
    [SerializeField] private Sprite unSelectedImage;    // �������� �ʾ��� �� �̹���

    private Image displayImage;                         // ���� �̹����� ǥ���� UI
    private static Image displayFoodImage;              // ���콺�� �ö� ���� �̹����� ǥ���� UI 
    private static GameObject foodStreamImage;          // ���� �����⸦ ǥ���� UI

    [SerializeField] private TextMeshProUGUI costText;          // ������ ǥ���ϴ� UI
    [SerializeField] private TextMeshProUGUI addFoodText;       // �߰� ��⸦ ǥ���ϴ� UI
    [SerializeField] private TextMeshProUGUI addHPText;         // �߰� ü���� ǥ���ϴ� UI

    [SerializeField] private GameObject addStatName;            // �߰� ���� �����ϴ� ���� �̸��� �����ϴ� ������Ʈ
    [SerializeField] private GameObject addStatValue;           // �߰� ���� �����ϴ� ���� �����ϴ� ������Ʈ

    private static int selectedIndex = -1;

    private Vector3 foodImagePos;
    private Vector3 foodStreamPos;

    [SerializeField] private GameObject buyEffect;              // ������ ǥ���ϴ� UI
    [SerializeField] private Transform buyEffectTransform;      // ������ ǥ���ϴ� UI ���� ��ġ
    [SerializeField] private TextMeshProUGUI remainCoinText;    // ���� ���� ��带 ǥ���ϴ� UI
    [SerializeField] private TextMeshProUGUI remainFoodText;    // ���� ���� ��⸦ ǥ���ϴ� UI
    [SerializeField] private TextMeshProUGUI remainMaxFoodText; // �ִ� ��⸦ ǥ���ϴ� UI
    [SerializeField] private Slider currentFoodSlider;          // ���� ��ⷮ�� ��Ÿ���� �����̴�
    [SerializeField] private AudioSource audiosource;           // ������ �Ҹ��� ����� ������Ʈ
    [SerializeField] private AudioClip buySound;                // �����ϴ� �Ҹ�
    [SerializeField] private AudioClip noSound;                 // ���� ���� �Ҹ�
    [SerializeField] private AudioClip mouseOver;               // ���콺 �ö� �� �Ҹ�

    [SerializeField] private SerializableDictionary<string, string> stateKoreaToEng;       // �߰� ������ �ѱ�� ����� �ٲ��ִ� �迭

    public void Start()
    {
        displayImage = GetComponent<Image>();
        Debug.Assert(displayImage != null, "�ٲ� �̹����� ����� UI�� �����ϴ�.");

        if (displayFoodImage == null)
            displayFoodImage = GameObject.Find("FoodPosition").GetComponent<Image>();

        if (foodStreamImage == null)
            foodStreamImage = GameObject.Find("FoodStream");

        Debug.Assert(displayFoodImage != null, "������ ������ ����� UI�� �����ϴ�.");
        Debug.Assert(foodStreamImage != null, "�����⸦ ����ϴ� UI�� �����ϴ�.");

        foodImagePos = displayFoodImage.transform.position;
        foodStreamPos = foodStreamImage.transform.position;

        Debug.Assert(buyEffect != null, "���� ȿ�� UI�� �����ϴ�.");
        Debug.Assert(remainCoinText != null, "���� ��带 ǥ���ϴ� UI�� �����ϴ�.");
        Debug.Assert(remainFoodText != null, "���� ��⸦ ǥ���ϴ� UI�� �����ϴ�.");
        Debug.Assert(remainMaxFoodText != null, "�ִ� ��⸦ ǥ���ϴ� UI�� �����ϴ�.");
        Debug.Assert(audiosource != null, "�Ҹ��� ����� ������Ʈ�� �����ϴ�.");

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
            // ���� ������ ��� ó��
            audiosource.PlayOneShot(noSound);

            return;
        }

        int food = GameManager.info.playerState.food + int.Parse(addFoodText.text);

        if (food > 100)
        {
            // ��Ⱑ ��á�� ��� ó��
            audiosource.PlayOneShot(noSound);

            return;
        }

        if (food < 0)
        {
            // ��Ⱑ -�� �� ���
            audiosource.PlayOneShot(noSound);

            return;
        }

        audiosource.PlayOneShot(buySound);

        // ���� ȿ�� ����
        GameObject newBuyEffect = Instantiate(buyEffect, buyEffectTransform);
        newBuyEffect.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = costText.text + "G";

        // ���ŵ� ��� �ݿ�
        GameManager.info.playerState.money += cost;
        remainCoinText.text = GameManager.info.playerState.money.ToString("#,##0G");

        // ��� �ݿ�
        GameManager.info.playerState.food = food;
        remainFoodText.text = GameManager.info.playerState.food.ToString();
        currentFoodSlider.value = GameManager.info.playerState.food / 100.0f;

        // ���� �ݿ�
        SetStat();

        // �ʱ�ȭ
        selectedIndex = -1;
        displayFoodImage.sprite = null;
        displayFoodImage.color = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
        foodStreamImage.SetActive(false);

        // �ٽ� ���� �� �� ������ ����
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
        // x������ -300��ŭ �̵�
        Vector3 targetFoodImagePos = foodImagePos + new Vector3(-300, 0, 0);
        Vector3 targetFoodStreamPos = foodStreamPos + new Vector3(-300, 0, 0);

        // �̵�
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

    // ���콺�� �ش� ������Ʈ ���� ���� ��
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        // �̹� ������ ���� ��� ����
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

    // ���콺�� �ش� ������Ʈ �ۿ� ���� ��
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        displayImage.sprite = unSelectedImage;

        foodStreamImage.SetActive(false);
    }
}