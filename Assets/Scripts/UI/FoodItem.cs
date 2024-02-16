using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FoodItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int index = 0;                               // �ڽ��� ��ġ�� �ε���

    [SerializeField] private Sprite selectedImage;      // �������� �� �̹���
    [SerializeField] private Sprite unSelectedImage;    // �������� �ʾ��� �� �̹���

    private Image displayImage;             // ���� �̹����� ǥ���� UI
    private static Image displayFoodImage;  // ���콺�� �ö� ���� �̹����� ǥ���� UI 

    [SerializeField] private TextMeshProUGUI costText;    // ������ ǥ���ϴ� UI

    private static int selectedIndex = -1;


    public void Start()
    {
        displayImage = GetComponent<Image>();
        Debug.Assert(displayImage != null, "�ٲ� �̹����� ����� UI�� �����ϴ�.");

        if (displayFoodImage == null)
            displayFoodImage = GameObject.Find("FoodPosition").GetComponent<Image>();
        
        Debug.Assert(displayFoodImage != null, "������ ������ ����� UI�� �����ϴ�.");
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(1))
            SetData();
    }

    private void SetData()
    {
        if (selectedIndex == -1 || selectedIndex != index)
            return;

        int cost = int.Parse(costText.text);

        if (GameManager.info.playerState.money < -cost)
        {
            // ���� ������ ��� ó��
            return;
        }

        //GameManager.info.playerState.money -= cost;

        selectedIndex = -1;
        displayFoodImage.sprite = null;
        displayFoodImage.color = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
    }


    // ���콺�� �ش� ������Ʈ ���� ���� ��
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (displayFoodImage.color.a < 0.1f)
            displayFoodImage.color = Color.white;

        displayImage.sprite = selectedImage;
        displayFoodImage.sprite = transform.GetChild(2).GetComponent<Image>().sprite;

        selectedIndex = index;
    }

    // ���콺�� �ش� ������Ʈ �ۿ� ���� ��
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        displayImage.sprite = unSelectedImage;
    }
}
