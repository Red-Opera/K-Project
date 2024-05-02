using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SaleItem : MonoBehaviour, IPointerClickHandler
{
    private static GameObject equidStore;           // 장비 구매 오브젝트
    private static EquidStore equidStoreCompo;      // 장비 상점 관련 컴포넌트
    private static GameObject saleAlert;            // 판매시 표시되는 오브젝트
    private static GameObject infoText;             // 정보 출력하는 텍스트를 담는 오브젝트
    private static TextMeshProUGUI equidName;       // 장비 이름
    private static Image saleItemIamge;             // 판매할 아이템의 이미지
    private static SaleItemFrame saleItemFrame;     // 판매 UI 스크립트
    private static TextMeshProUGUI saleCostText;    // 판매 금액 텍스트

    private EquidState state;                       // 장비 능력치
    private MoveInventory moveInventory;            // 장비 이동관련 스크립트

    // 해당 아이템을 오른쪽 마우스로 클릭할 경우
    public void OnPointerClick(PointerEventData eventData)
    {
        // 판매 UI가 켜져 있거나 장비 구매 UI가 꺼저 있을 경우 처리 안함, 이미 장착중인 장비는 판매 중지
        if (saleAlert.activeSelf || !equidStore.activeSelf || moveInventory.displayIndex < 8)
            return;

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            saleAlert.SetActive(true);

            // 판매할 이미지를 표시하고 판매할 장비 인덱스를 넘김
            saleItemIamge.sprite = transform.GetChild(0).GetComponent<Image>().sprite;
            saleItemFrame.saleItemIndex = moveInventory.displayIndex;

            // 판매 금액을 출력함
            for (int i = 0; i < equidStoreCompo.selectables.Count; i++)
            {
                if (saleItemIamge.sprite == equidStoreCompo.selectables[i].sprite)
                    saleCostText.text = (equidStoreCompo.selectables[i].cost * 0.8f).ToString("#,##0");
            }

            // 장비 정보 상태 숨김
            for (int i = 0; i < 3; i++)
                infoText.transform.GetChild(i).gameObject.SetActive(false);

            int infoCount = 0;  // 정보 개수

            FieldInfo[] allFields = typeof(State).GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (FieldInfo field in allFields)
            {
                if (field.Name == "money")
                    continue;

                if (field.Name == "nickName")
                {
                    equidName.text = (string)field.GetValue(state.state);

                    continue;
                }

                object value = field.GetValue(state.state);

                if (field.FieldType == typeof(float))
                {
                    if ((float)value < 0.01f)
                        continue;
                }

                else if (field.FieldType == typeof(int))
                {
                    if ((int)value == 0)
                        continue;
                }
                
                else
                    Debug.Assert(false, "잘못된 상태 타입");

                Transform onInfoText = infoText.transform.GetChild(infoCount);
                onInfoText.gameObject.SetActive(true);
                onInfoText.GetChild(0).GetComponent<TextMeshProUGUI>().text = field.Name + " : " + field.GetValue(state.state);
                
                infoCount++;
            }
        }
    }

    public void OnEnable()
    {
        if (equidStore == null)
        {
            equidStore = GameObject.Find("EventSystem").GetComponent<UIOpen>().equidUI;
            Debug.Assert(equidStore != null, "장비 상점 UI가 없습니다.");

            equidStore.SetActive(true);

            equidStoreCompo = equidStore.GetComponent<EquidStore>();
            Debug.Assert(equidStoreCompo != null, "장비 상점 컴포넌트가 없습니다.");

            saleAlert = equidStore.transform.Find("Alert").gameObject;
            Debug.Assert(saleAlert != null, "장비 판매 UI가 없습니다.");

            Transform infoFrame = saleAlert.transform.Find("InfoFrame");

            infoText = infoFrame.Find("Ability").gameObject;
            Debug.Assert(infoText != null, "장비 정보를 담는 오브젝트가 없습니다.");

            equidName = infoFrame.Find("EquidNameLabel").GetChild(0).GetComponent<TextMeshProUGUI>();
            Debug.Assert(equidName != null, "장비 이름을 넣는 텍스트가 없습니다.");

            saleItemIamge = infoFrame.Find("ItemFrame").GetChild(0).GetComponent<Image>();
            Debug.Assert(saleItemIamge != null, "판매할 이미지 프레임이 없습니다.");

            saleItemFrame = saleAlert.GetComponent<SaleItemFrame>();
            Debug.Assert(saleItemFrame != null, "장비 UI 스크립트가 없습니다.");

            saleCostText = infoFrame.Find("CostLabel").GetChild(0).GetComponent<TextMeshProUGUI>();
            Debug.Assert(saleCostText != null, "판매 금액 텍스트가 없습니다.");

            equidStore.SetActive(false);
        }

        if (state == null)
        {
            state = GetComponent<EquidState>();             // 현재 상태를 가져옴
            moveInventory = GetComponent<MoveInventory>();  // 장비 이동관련 컴포넌트를 가져옴
        }
    }
}
