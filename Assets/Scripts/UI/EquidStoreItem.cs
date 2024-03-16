using UnityEngine;
using UnityEngine.EventSystems;

public class EquidStoreItem : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector] public int thisIndex;         // 이 오브젝트의 인덱스

    private static EquidStore equidStore;           // 장비 상점을 관리하는 스크립트
    private static int currentSelectedIndex = 0;    // 현재 선택한 아이템 인덱스

    public void Start()
    {
        if (equidStore == null)
        {
            equidStore = GameObject.Find("EquidStore").GetComponent<EquidStore>();
            Debug.Assert(equidStore != null, "상점 스토어가 없습니다.");
        }
    }

    public static void BuyItem()
    {
        equidStore.BuyEffect(currentSelectedIndex);
    }

    // 해당 아이템을 선택시처리되는 메소드
    public void DisplayItem()
    {
        equidStore.OnSelectedItem(thisIndex, ref currentSelectedIndex);
    }

    // 해당 아이템을 클릭시 실행되는 메소드
    public void OnPointerClick(PointerEventData eventData)
    {
        DisplayItem();
    }
}
