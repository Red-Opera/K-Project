using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EquidStore : MonoBehaviour
{
    [SerializeField] public List<SelectableItem> selectables;                   // 전시할 수 있는 아이템

    [SerializeField] private GameObject slots;                      // 슬롯을 저장하는 오브젝트
    [SerializeField] private GameObject details;                    // 해당 아이템의 세부 정보들을 저장하는 오브젝트
    [SerializeField] private GameObject item;                       // 전시할 때 사용되는 프레임
    [SerializeField] private GameObject buyEffect;                  // 구매 효과 오브젝트
    [SerializeField] private Transform buyEffectTransform;          // 구매 효과 시작 위치
    [SerializeField] private Button buyButton;                      // 구매 버튼
    [SerializeField] private GameObject saleUI;                     // 판매 UI
    [SerializeField] private AudioClip openSound;                   // 열릴 때 나는 소리
    [SerializeField] private AudioClip buySound;                    // 구매할 때 나는 소리
    [SerializeField] int contentprintPerSec;                        // 1초당 출력되는 글자 수

    private TextMeshProUGUI outText;// 세부내용을 출력할 위치
    private AudioSource playerAudio;      // 소리 출력 컴포넌트
    private string sceneName;       // 현재 씬 이름
    private string typeContent;     // 세부내용에 입력할 내용
    private bool isType = false;    // 현재 내용을 쓰고 있는지 여부

    public void Start()
    {
        Debug.Assert(slots != null, "장비 상점의 전시할 슬롯이 없습니다.");
        Debug.Assert(details != null, "세부적인 내용을 보여주는 프레임이 없습니다.");
        Debug.Assert(item != null, "전시할 때 사용되는 프레임이 없습니다.");
        Debug.Assert(buyButton != null, "구매 버튼이 없습니다.");
        Debug.Assert(saleUI != null, "판매 UI가 없습니다.");

        buyButton.onClick.AddListener(() => EquidStoreItem.BuyItem());
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 현재 작성 효과를 중지
            if (isType)
            {
                outText.text = typeContent;
                StopAllCoroutines();

                isType = false;
            }
        }

        if (saleUI.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            saleUI.SetActive(false);

        else if (!saleUI.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            gameObject.SetActive(false);

        if (!UINotDestroyOpen.inventory.activeSelf)
            UINotDestroyOpen.inventory.SetActive(true);
    }

    // 아이템 전시 공간을 업데이트하는 메소드
    private void ItemUpdate()
    {
        for (int i = 0; i < slots.transform.childCount; i++)
        {
            // 전시할 수 있는 슬롯을 가져옴
            Transform slot = slots.transform.GetChild(i);

            // 전시되어 있는 모든 아이템을 제거함
            while (slot.childCount >= 1)
                Destroy(slot.GetChild(0).gameObject);

            // 전시할 아이템을 생성함
            GameObject newItem = Instantiate(item, slot);
            newItem.GetComponent<MoveInventory>().enabled = false;
            newItem.AddComponent<EquidStoreItem>().thisIndex = i;

            // 새로 생성한 아이템을 랜덤으로 생성함
            SelectedRandomItem(newItem, i);
        }
    }

    // 전시할 아이템을 랜덤으로 정하는 메소드
    private void SelectedRandomItem(GameObject newItem, int selectedIndex)
    {
        // 랜덤으로 상점에 전시할 아이템을 정함
        int itemIndex = Random.Range(0, selectables.Count);

        while (selectables[itemIndex].level == EquidLevel.EQUIPMENT_LEVEL_LEGEND)
            itemIndex = Random.Range(0, selectables.Count);

        // 선택한 이미지와 타입으로 바꿈
        newItem.GetComponent<InventableEquipment>().inventableEquipment = selectables[itemIndex].equipmentType;
        newItem.transform.GetChild(0).GetComponent<Image>().sprite = selectables[itemIndex].sprite;

        // 장비 정보를 새로 생성함
        EquidState equidState = newItem.GetComponent<EquidState>();
        equidState.state = ScriptableObject.CreateInstance<State>();

        // 모든 필드를 가져와서 복사함.
        FieldInfo[] allFields = typeof(State).GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (FieldInfo field in allFields)
            field.SetValue(equidState.state, field.GetValue(selectables[itemIndex].state));

        // 선택한 세부정보 위치를 가져오고 모두 수정
        Transform changeDetail = details.transform.GetChild(selectedIndex);

        changeDetail.GetChild(1).GetComponent<TextMeshProUGUI>().text = selectables[itemIndex].state.money.ToString("#,##0");
        changeDetail.GetChild(2).GetChild(0).GetComponent<Image>().sprite = selectables[itemIndex].sprite;
        changeDetail.GetChild(3).GetComponent<TextMeshProUGUI>().text = selectables[itemIndex].content;
    }

    // 세부적인 내용을 작성할 때 글자를 작성하는 효과를 나타내는 메소드
    private IEnumerator TypingContent()
    {
        // 한글자가 출력되는 시간을 구함
        float printDelay = 1f / contentprintPerSec;

        outText.text = "";

        // 한글자씩 출력
        for (int i = 0; i < typeContent.Length; i++)
        {
            outText.text += typeContent[i];

            // 1초당 contentprintPerSec개수 만큼 대기
            yield return new WaitForSeconds(printDelay);
        }

        isType = false;
    }

    // 아이템을 선택시 실행되는 메소드
    public void OnSelectedItem(int selectedIndex, ref int currentOnDisplayIndex)
    {
        // 같은 인덱스를 누를 경우 동작 안함
        if (selectedIndex == currentOnDisplayIndex)
            return;

        // 작성 중인 부분은 모두 작성하고 작성효과를 종료함
        if (outText != null)
        {
            outText.text = typeContent;
            StopAllCoroutines();
        }

        // 기존에 켜져 있는 세부정보를 끄고 선택한 세부정보로 바꿈
        details.transform.GetChild(currentOnDisplayIndex).gameObject.SetActive(false);
        details.transform.GetChild(selectedIndex).gameObject.SetActive(true);

        // 현재 세부정보의 인덱스를 출력함
        currentOnDisplayIndex = selectedIndex;

        // 출력할 세부정보 위치와 출력할 텍스트를 저장함
        outText = details.transform.GetChild(selectedIndex).GetChild(3).GetComponent<TextMeshProUGUI>();
        typeContent = outText.text;

        isType = true;
        StartCoroutine(TypingContent());
    }

    // 구매 메소드
    public void BuyEffect(int selectedIndex)
    {
        Transform selectItem = details.transform.GetChild(selectedIndex);
        
        int cost = int.Parse(selectItem.GetChild(1).GetComponent<TextMeshProUGUI>().text.Replace(",", ""));

        // 선택한 프레임을 가져오고 비용이 부족하거나 아이템을 구매한 적이 있다면 반영 안함
        Transform selectFrame = slots.transform.GetChild(selectedIndex);
        if (GameManager.info.allPlayerState.money < cost || selectFrame.childCount == 0)
            return;

        // 선택한 아이템을 가져옴
        GameObject getItem = selectFrame.GetChild(0).gameObject;

        // 해당 아이템을 인벤토리에 추가
        //InventroyPosition.CallAddItem(
        //    getItem.transform.GetChild(0).GetComponent<Image>().sprite.name, 
        //    getItem.GetComponent<InventableEquipment>().inventableEquipment,
        //    getItem.GetComponent<EquidState>());

        ResultUI.GetItem(getItem.transform.GetChild(0).GetComponent<Image>().sprite.name);

        // 현재돈 동기화
        GameManager.info.playerState.money -= cost;
        GameManager.info.UpdatePlayerState();

        // 구매 효과
        GameObject newBuyEffect = Instantiate(buyEffect, buyEffectTransform);
        newBuyEffect.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = cost + "G";

        playerAudio.PlayOneShot(buySound);

        Destroy(getItem);
    }

    public void OnEnable()
    {
        if (playerAudio == null)
        {
            playerAudio = GetComponent<AudioSource>();
            Debug.Assert(playerAudio != null, "소리 컴포넌트가 없습니다.");
        }

        playerAudio.PlayOneShot(openSound);

        // 현재 씬을 가져옴
        string currentScene = SceneManager.GetActiveScene().name;

        UINotDestroyOpen.inventory.SetActive(true);
        saleUI.SetActive(false);

        // 씬이 바뀌었을 경우에만 상점이 업데이트되도록 설정
        if (currentScene != sceneName)
        {
            sceneName = currentScene;

            ItemUpdate();
        }
    }

    public void OnDisable()
    {
        UINotDestroyOpen.inventory.SetActive(false);
        saleUI.SetActive(false);
    }
}

public enum EquidLevel
{
    EQUIPMENT_LEVEL_NORMAL = 0,         // 노말 장비
    EQUIPMENT_LEVEL_RERE = 1,           // 레어 장비
    EQUIPMENT_LEVEL_EPIC = 2,           // 에픽 장비
    EQUIPMENT_LEVEL_UNIQUE = 3,         // 유니크 장비
    EQUIPMENT_LEVEL_LEGEND = 4,         // 레전드 무기
}

// 전시할 수 있는 아이템의 정보
[System.Serializable]
public class SelectableItem
{
    public Sprite sprite;                   // 이미지
    public string content;                  // 내용
    public EquipmentState equipmentType;    // 무기 타입
    public EquidLevel level;                // 장비 등급

    public State state;                     // 무기 정보
}