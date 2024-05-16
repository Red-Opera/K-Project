using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;  // 현재 소유 금액 텍스트
    [SerializeField] private Transform slots;          // 슬롯 오브젝트

    public static Dictionary<int, GameObject> inventorySlotItem;   // 장비 인벤토리 슬롯 아이템
    private static Transform staticSlots;                           // 슬롯 오브젝트 (Static 버전)
    private static bool isLeft = true;                              // 현재 왼쪽 장비 슬롯을 사용하는 여부

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

    public void OnEnable()
    {
        if (staticSlots == null)
            staticSlots = slots;

        InventoryUpdate(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    // 씬 넘어가기 전 모든 장비 파괴 방지 메소드
    public static void InventoryDataUpdate()
    {
        // 현재 장비를 담는 변수 초기화
        inventorySlotItem = new Dictionary<int, GameObject>();

        // 모든 장비를 담음
        for (int i = 0; i < staticSlots.transform.childCount; i++)
        {
            Transform currentSlot = staticSlots.transform.GetChild(i);

            if (currentSlot.name == "Banned")
                continue;

            if (currentSlot.childCount > 0)
            {
                Transform item = currentSlot.GetChild(0);

                if (item.name == "Banned")
                    continue;

                // 장비 데이터를 저장하고 안보이게 다른 곳으로 이동
                inventorySlotItem.Add(i, item.gameObject);
                item.SetParent(null);
                item.GetChild(0).position = new Vector2(-9999, -9999);

                DontDestroyOnLoad(item.gameObject);
            }
        }
    }

    // 씬 넘어간 후 장비 인벤토리 재배치
    public static void InventoryUpdate(Scene scene, LoadSceneMode mode)
    {
        // 현재 스테이지가 Map인 경우
        if (inventorySlotItem == null || staticSlots == null)
            return;

        // 모든 장비에 대해서 장비 인벤토리로 이동
        foreach (int key in inventorySlotItem.Keys)
        {
            inventorySlotItem[key].transform.SetParent(staticSlots.transform.GetChild(key));
            inventorySlotItem[key].transform.GetChild(0).localPosition = Vector2.zero;
            inventorySlotItem[key].transform.localScale = Vector2.one;
        }
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
            Transform slot = staticSlots.GetChild(0);

            if (slot.childCount > 1)
            {
                // 주 무기에 대한 정보를 얻고 그 모든 정보를 추가 무기 정보에 더함
                Transform weaphon = slot.GetChild(0);
                GameManager.AddStates(GameManager.info.addWaphonState, weaphon.GetComponent<EquidState>().state);
            }

            Transform subWeaphon = null;
            if (staticSlots.GetChild(1).childCount >= 1)
                subWeaphon = staticSlots.GetChild(1).GetChild(0);

            if (subWeaphon != null)
                GameManager.AddStates(GameManager.info.addWaphonState, subWeaphon.GetComponent<EquidState>().state);
        }

        // 오른쪽 장비 슬롯을 사용할 경우
        else
        {
            Transform slot = staticSlots.GetChild(2);

            if (slot.childCount > 1)
            {
                Transform weaphon = slot.GetChild(0);
                GameManager.AddStates(GameManager.info.addWaphonState, weaphon.GetComponent<EquidState>().state);
            }

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