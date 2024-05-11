using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;  // 현재 소유 금액 텍스트
    [SerializeField] private Transform slots;          // 슬롯 오브젝트

    private static Transform staticSlots;  // 슬롯 오브젝트 (Static 버전)
    private static bool isLeft = true;      // 현재 왼쪽 장비 슬롯을 사용하는 여부

    private void Start()
    {
        Debug.Assert(coinText != null, "현재 돈을 출력할 텍스트가 없습니다.");

        UpdateCoin();
    }

    private void Update()
    {
        UpdateCoin();
    }

    public void UpdateCoin()
    {
        coinText.text = GameManager.info.allPlayerState.money.ToString("#,##0");
    }

    private void OnEnable()
    {
        if (staticSlots == null)
            staticSlots = slots;
    }

    // 현재 장착하고 있는 이미지 스프라이트 반환
    public static Sprite GetCurrentWeaphonSprite()
    {
        if (isLeft)
            return staticSlots.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite;

        return staticSlots.GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>().sprite;
    }

    // 현재 장착 중인 장비에 따라 능력치를 업데이트하는 메소드
    public static void EquidSlotStateUpdate()
    {
        // 장비 상태를 초기화 함
        GameManager.info.addWaphonState = ScriptableObject.CreateInstance<State>();

        // 왼쪽 장비 슬롯을 사용하는 경우
        if (isLeft)
        {
            // 주 무기에 대한 정보를 얻고 그 모든 정보를 추가 무기 정보에 더함
            Transform weaphon = staticSlots.GetChild(0).GetChild(0);
            GameManager.AddStates(GameManager.info.addWaphonState, weaphon.GetComponent<EquidState>().state);

            Transform subWeaphon = null;
            if (staticSlots.GetChild(1).childCount >= 1)
                subWeaphon = staticSlots.GetChild(1).GetChild(0);

            if (subWeaphon != null)
                GameManager.AddStates(GameManager.info.addWaphonState, subWeaphon.GetComponent<EquidState>().state);
        }

        // 오른쪽 장비 슬롯을 사용할 경우
        else
        {
            Transform weaphon = staticSlots.GetChild(2).GetChild(0);
            GameManager.AddStates(GameManager.info.addWaphonState, weaphon.GetComponent<EquidState>().state);

            Transform subWeaphon = null;
            if (staticSlots.GetChild(3).childCount >= 1)
                subWeaphon = staticSlots.GetChild(3).GetChild(0);

            if (subWeaphon != null)
                GameManager.AddStates(GameManager.info.addWaphonState, subWeaphon.GetComponent<EquidState>().state);
        }

        // 모든 반지 슬롯에 대해서 
        for (int i = 4; i < 8; i++)
        {
            // 현재 슬롯을 선택함
            Transform currentSlot = staticSlots.GetChild(i);

            // 만약 해당 슬롯에 장비가 없는 경우 다음 슬롯으로 넘어감
            if (currentSlot.childCount == 0)
                continue;

            // 그 장비의 능력치를 모두 더함
            GameManager.AddStates(GameManager.info.addWaphonState, currentSlot.GetChild(0).GetComponent<EquidState>().state);
        }

        // 모든 플레이어 상태를 동기화 함
        GameManager.info.UpdatePlayerState();
    }
}