using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FoodItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int index = 0;                               // 자신이 위치한 인덱스

    [SerializeField] private Sprite selectedImage;      // 선택했을 때 이미지
    [SerializeField] private Sprite unSelectedImage;    // 선택하지 않았을 때 이미지

    private Image displayImage;             // 선택 이미지를 표시할 UI
    private static Image displayFoodImage;  // 마우스에 올라간 음식 이미지를 표시할 UI 

    [SerializeField] private TextMeshProUGUI costText;    // 가격을 표시하는 UI

    private static int selectedIndex = -1;


    public void Start()
    {
        displayImage = GetComponent<Image>();
        Debug.Assert(displayImage != null, "바뀐 이미지를 출력할 UI가 없습니다.");

        if (displayFoodImage == null)
            displayFoodImage = GameObject.Find("FoodPosition").GetComponent<Image>();
        
        Debug.Assert(displayFoodImage != null, "선택한 음식을 출력할 UI가 없습니다.");
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
            // 돈이 부족할 경우 처리
            return;
        }

        //GameManager.info.playerState.money -= cost;

        selectedIndex = -1;
        displayFoodImage.sprite = null;
        displayFoodImage.color = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
    }


    // 마우스가 해당 오브젝트 위에 있을 때
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (displayFoodImage.color.a < 0.1f)
            displayFoodImage.color = Color.white;

        displayImage.sprite = selectedImage;
        displayFoodImage.sprite = transform.GetChild(2).GetComponent<Image>().sprite;

        selectedIndex = index;
    }

    // 마우스가 해당 오브젝트 밖에 있을 때
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        displayImage.sprite = unSelectedImage;
    }
}
