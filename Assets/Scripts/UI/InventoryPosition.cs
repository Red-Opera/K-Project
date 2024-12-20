using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Reflection;
using MySqlConnector;
using System;

public class InventroyPosition : MonoBehaviour
{
    public static GameObject inventory;

    public static bool isChange = true;                 // 자리를 교환했는지 여부
    public static bool isAddSucceed = false;            // 구매 성공 여부
    public static bool isAddItemable = false;           // AddItem 사용 가능 여부
    public static bool isItemAdd = false;

    [SerializeField] private GameObject slots;          // 전시할 수 있는 오브젝트를 담는 슬롯
    [SerializeField] private GameObject itemDisplay;    // 아이템을 전시하기 위한 오브젝트

    private List<GameObject> displayData;               // 현재 전시중인 아이템
    private Transform[] displayPos;                     // 아이템을 전시할 수 있는 오브젝트

    public static event System.Action<string, EquipmentState, EquidState> OnAddItem;        // 이 스크립트를 가지고 있는 모든 오브젝트가 실행할 이벤트

    public List<Sprite> spriteData = new List<Sprite>();                        // 추가할 아이템 이미지
    private Dictionary<string, Sprite> sprites;                                 // 추가할 아이템 이름과 이미지 배열

    private void Awake()
    {
        displayPos = new Transform[slots.transform.childCount];
        for (int i = 0; i < slots.transform.childCount; i++)
            displayPos[i] = slots.transform.GetChild(i);

        displayData = new List<GameObject>();

        for (int i = 0; i < displayPos.Length; i++)
        {
            if (displayPos[i].childCount == 0)
                continue;

            Transform item = displayPos[i].GetChild(0);

            if (item.GetComponent<MoveInventory>() == null)
                continue;

            // 아이템의 위치을 각각 오브젝트별로 알려줌
            item.GetComponent<MoveInventory>().displayIndex = i;

            // 오브젝트를 차례대로 인벤토리에 넣음
            item.transform.localPosition = Vector3.zero;
        }

        sprites = new Dictionary<string, Sprite>();

        foreach (Sprite sprite in spriteData)
            sprites.Add(sprite.name, sprite);

        inventory = slots;

        isAddItemable = true;
        OnAddItem = AddItem;
    }

    private void Start()
    {
        StartGameAddItem();
    }

    private void StartGameAddItem()
    {
        if (Login.currentLoginName == "" || isItemAdd)
            return;

        isItemAdd = true;

        string query = "SELECT * FROM PlayerItem WHERE Name = @Name";   // SQL 쿼리 문자열을 작성하여 PlayerLogin 테이블에서 특정 ID를 검색
        MySqlCommand cmd = new MySqlCommand(query, Login.conn);
        cmd.Parameters.AddWithValue("@Name", Login.currentLoginName);

        int[] itemNums = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        try
        {
            Login.conn.Open();

            // 쿼리를 실행하고 MySqlDataReader 객체를 생성하여 결과를 읽어옴
            using MySqlDataReader dataReader = cmd.ExecuteReader();

            if (dataReader.Read())
            {
                for (int i = 1; i <= 23; i++)
                {
                    if (dataReader["Item" + i] == DBNull.Value)
                        continue;

                    int itemNum = dataReader.GetInt32("Item" + i);

                    itemNums[i] = itemNum;
                }
            }

            else
            {
                Debug.LogWarning("No player item data found for current login name.");
            }
        }

        catch (Exception ex)
        {
            Debug.LogError($"Failed to execute query for PlayerInfo: " + ex.Message);
        }

        finally
        {
            Login.conn.Close();
        }

        for (int i = 1; i <= 23; i++)
        {
            if (itemNums[i] == 0)
                continue;

            string itemName = "";

            foreach (string name in SaveSystem.itemID.Keys)
            {
                if (SaveSystem.itemID[name] == itemNums[i])
                    itemName = name;
            }

            ResultUI.GetItem(itemName);
        }
    }

    // 아이템의 위치를 조정하는 메소드
    public void ChangePos(int displayIndex, int dragIndex)
    {
        // 아직 초기화하지 않았다면 초기화
        if (displayPos == null)
            Awake();

        if (displayPos[displayIndex].childCount == 0)
            return;

        // 이동할 장비, 붙여 놓을 위치
        EquipmentState displayEquipment = displayPos[displayIndex].GetChild(0).GetComponent<InventableEquipment>().inventableEquipment;
        EquipmentState dragEquipment = displayPos[dragIndex].GetComponent<InventableEquipment>().inventableEquipment;

        // 같은 장소로 이동하거나 이미 바꿨거나 해당 타입의 장비를 해당 위치에 둘 수 없을 경우
        if (displayIndex == dragIndex || isChange || ((displayEquipment & dragEquipment) == 0))
            return;

        // 한손 장비를 왼쪽 슬롯에 넣을 경우
        bool isPosOneHand = (displayEquipment & (EquipmentState.EQUIPMENT_WEAPON_ONEHANDED_LARGE_RANGE | EquipmentState.EQUIPMENT_WEAPON_ONEHANDED_SHORT_RANGE)) != 0;
        if ((dragIndex == 0 || dragIndex == 2) && isPosOneHand && displayPos[dragIndex + 1].childCount > 0)
            return;

        // 한손 장비를 가지고 오른손에 장비를 작용할려고하는 경우
        bool isPosTwoHand = (displayEquipment & (EquipmentState.EQUIPMENT_WEAPON_TWOHANDED_LARGE_RANGE | EquipmentState.EQUIPMENT_WEAPON_TWOHANDED_SHORT_RANGE)) != 0;
        if ((dragIndex == 1 || dragIndex == 3) && displayPos[dragIndex - 1].childCount > 0)
        {
            EquipmentState leftEquidState = displayPos[dragIndex - 1].GetChild(0).GetComponent<InventableEquipment>().inventableEquipment;
            bool isLeftEquidOneHand = (leftEquidState & (EquipmentState.EQUIPMENT_WEAPON_ONEHANDED_LARGE_RANGE | EquipmentState.EQUIPMENT_WEAPON_ONEHANDED_SHORT_RANGE)) != 0;

            if (isPosTwoHand && isLeftEquidOneHand)
                return;
        }

        // 인벤토리의 범위를 넘긴 경우
        if (displayData == null || displayIndex < 0 || dragIndex < 0 || displayIndex >= displayPos.Length || dragIndex >= displayPos.Length)
        {
            Debug.Assert(false, "Error (Out of Range) : 잘못된 인덱스가 전달되었습니다.");
            return;
        }

        // 아이템 위치를 빈공간으로 옮기는 경우
        if (displayPos[dragIndex].childCount == 0)
        {
            Transform moveInventory = displayPos[displayIndex].GetChild(0);

            moveInventory.GetComponent<MoveInventory>().displayIndex = dragIndex;
            moveInventory.SetParent(displayPos[dragIndex]);
        }

        // displayIndex와 dragIndex의 오브젝트를 교환
        else
        {
            if (displayPos[displayIndex].childCount <= 0 || displayPos[dragIndex].childCount <= 0)
                return;

            // 슬롯의 장비에 넣을 수 있는 목록을 가져옴
            EquipmentState displaySlotEquidState = displayPos[displayIndex].GetComponent<InventableEquipment>().inventableEquipment;
            EquipmentState dragSlotEquidState = displayPos[dragIndex].GetComponent<InventableEquipment>().inventableEquipment;

            // 장비 종류를 가져옴
            EquipmentState displayState = displayPos[displayIndex].GetChild(0).GetComponent<InventableEquipment>().inventableEquipment;
            EquipmentState dragState = displayPos[dragIndex].GetChild(0).GetComponent<InventableEquipment>().inventableEquipment;

            // 옮겼을 때 장비 위치를 바꿀 수 없는 경우 중지
            if ((displaySlotEquidState & dragState) == 0 || (dragSlotEquidState & displayState) == 0)
                return;

            // 교체 당하는 장비가 왼쪽 슬롯에 넣을 경우
            bool isChangeOneHand = (dragSlotEquidState & (EquipmentState.EQUIPMENT_WEAPON_ONEHANDED_LARGE_RANGE | EquipmentState.EQUIPMENT_WEAPON_ONEHANDED_SHORT_RANGE)) != 0;
            if ((displayIndex == 0 || displayIndex == 2) && isChangeOneHand && displayPos[displayIndex + 1].childCount > 0)
                return;

            MoveInventory aMoveInventory = displayPos[displayIndex].GetChild(0).GetComponent<MoveInventory>();
            MoveInventory bMoveInventory = displayPos[dragIndex].GetChild(0).GetComponent<MoveInventory>();

            if (aMoveInventory == null || bMoveInventory == null)
                return;

            int a = aMoveInventory.displayIndex;
            int b = bMoveInventory.displayIndex;

            bMoveInventory.displayIndex = a;
            displayPos[dragIndex].GetChild(0).SetParent(displayPos[a]);

            aMoveInventory.displayIndex = b;
            displayPos[displayIndex].GetChild(0).SetParent(displayPos[b]);
        }

        // 이동시킬 오브젝트와 밀러난 아이템 위치를 천천히 중앙으로 옮김
        UpdateDisplayPositions(dragIndex);
        UpdateDisplayPositions(displayIndex);
    }

    // 아이템 위치를 정상적인 위치로 천천히 이동시키는 메소드
    private void UpdateDisplayPositions(int moveIndex)
    {
        if (displayPos[moveIndex].childCount <= 0)
            return;

        displayPos[moveIndex].GetChild(0).localPosition = Vector3.zero;

        isChange = true;
    }

    // 아이템을 추가하거나 생성하는 함수
    public void AddItem(string name, EquipmentState equipmentState, EquidState state = null)
    {
        Debug.Assert(sprites.ContainsKey(name), "해당 이름의 아이템은 존재하지 않습니다");
        int i = 8;

        bool isOneHand = (equipmentState & (EquipmentState.EQUIPMENT_WEAPON_ONEHANDED_LARGE_RANGE | EquipmentState.EQUIPMENT_WEAPON_ONEHANDED_SHORT_RANGE)) != 0;
        bool isTwoHand = (equipmentState & (EquipmentState.EQUIPMENT_WEAPON_TWOHANDED_LARGE_RANGE | EquipmentState.EQUIPMENT_WEAPON_TWOHANDED_SHORT_RANGE)) != 0;

        // 무기를 첫번째 슬롯에 장착할 수 있는 경우
        if (isOneHand || isTwoHand)
        {
            // 오른쪽 슬롯이 비어 있거나 양손검인 경우
            if (displayPos[1].childCount <= 0 || isTwoHand)
            {
                if (displayPos[0].childCount <= 0)
                    i = 0;
            }

            else if (displayPos[3].childCount <= 0 || isTwoHand)
            {
                if (displayPos[2].childCount <= 0)
                    i = 2;
            }
        }

        // 검 두개를 장착할 수 있는 경우
        if (i == 8 && isTwoHand)
        {
            if (displayPos[0].childCount > 0)
            {
                EquipmentState firstWeaphon = displayPos[0].GetChild(0).GetComponent<InventableEquipment>().inventableEquipment;
                bool isFirstSlotOneHand = (firstWeaphon & (EquipmentState.EQUIPMENT_WEAPON_ONEHANDED_LARGE_RANGE | EquipmentState.EQUIPMENT_WEAPON_ONEHANDED_SHORT_RANGE)) != 0;

                if (displayPos[1].childCount <= 0 && !isFirstSlotOneHand)
                    i = 1;
            }

            else if (displayPos[2].childCount > 0)
            {
                EquipmentState thirdWeaphon = displayPos[2].GetChild(0).GetComponent<InventableEquipment>().inventableEquipment;
                bool isThirdSlotOneHand = (thirdWeaphon & (EquipmentState.EQUIPMENT_WEAPON_ONEHANDED_LARGE_RANGE | EquipmentState.EQUIPMENT_WEAPON_ONEHANDED_SHORT_RANGE)) != 0;

                if (displayPos[3].childCount <= 0 && !isThirdSlotOneHand)
                    i = 3;
            }
        }

        // 반지일 경우
        else if ((equipmentState & EquipmentState.EQUIPMENT_ACCESSORY) != 0)
        {
            i = 4;
            for (; i <= 7; i++)
            {
                if (displayPos[i].childCount <= 0)
                    break;
            }
        }

        // 장비를 장착할 수 없어 인벤토리에 저장할 경우
        for (; i < displayPos.Length; i++)
        {
            // 슬롯을 찾음
            Transform slot = displayPos[i];

            // 해당 슬롯에 아이템을 넣을 수 있는지 확인
            if (slot.childCount <= 0)
            {
                // 해당 아이템을 생성 후 인벤토리에 맞게 조정
                GameObject newItem = Instantiate(itemDisplay);
                newItem.GetComponent<InventableEquipment>().inventableEquipment = equipmentState;
                newItem.GetComponent<MoveInventory>().displayIndex = i;
                newItem.transform.GetChild(0).GetComponent<Image>().sprite = sprites[name];

                // 해당 아이템을 원위치 시킴
                newItem.transform.SetParent(slot);
                newItem.transform.localPosition = Vector3.zero;

                if (state != null)
                {
                    // 모든 필드를 가져와서 복사함.
                    FieldInfo[] allFields = typeof(State).GetFields(BindingFlags.Public | BindingFlags.Instance);
                    EquidState newItemState = newItem.GetComponent<EquidState>();
                    newItemState.state = ScriptableObject.CreateInstance<State>();

                    // 모든 필드 값을 상점에 있는 값을 인벤토리로 가져옴
                    foreach (FieldInfo field in allFields)
                        field.SetValue(newItemState.state, field.GetValue(state.state));
                }

                displayData.Add(newItem);
                isAddSucceed = true;
                return;
            }
        }

        // 빈공간이 없을 떄 처리
        isAddSucceed = false;
    }

    // 다른 스크립트에서 AddItem을 호출하기 위해 사용되는 메소드
    public static void CallAddItem(string name, EquipmentState equipmentState, EquidState state = null)
    {
        OnAddItem(name, equipmentState, state);
    }
}