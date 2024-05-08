using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Reflection;

public class InventroyPosition : MonoBehaviour
{
    public static bool isChange = true;                 // 자리를 교환했는지 여부
    public static bool isAddSucceed = false;            // 구매 성공 여부

    [SerializeField] private GameObject slots;          // 전시할 수 있는 오브젝트를 담는 슬롯
    [SerializeField] private GameObject itemDisplay;    // 아이템을 전시하기 위한 오브젝트

    private List<GameObject> displayData;               // 현재 전시중인 아이템
    private Transform[] displayPos;                     // 아이템을 전시할 수 있는 오브젝트

    public static event System.Action<string, EquipmentState, EquidState> OnAddItem;        // 이 스크립트를 가지고 있는 모든 오브젝트가 실행할 이벤트
    public List<Sprite> spriteData = new List<Sprite>();                        // 추가할 아이템 이미지
    private Dictionary<string, Sprite> sprites;                                 // 추가할 아이템 이름과 이미지 배열

    public void Awake()
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

        OnAddItem += AddItem;
    }

    // 아이템의 위치를 조정하는 메소드
    public void ChangePos(int displayIndex, int dragIndex)
    {
        if (displayPos[displayIndex].childCount == 0)
            return;

        // 이동할 장비, 붙여 놓을 위치
        EquipmentState displayEquipment = displayPos[displayIndex].GetChild(0).GetComponent<InventableEquipment>().inventableEquipment;
        EquipmentState dragEquipment = displayPos[dragIndex].GetComponent<InventableEquipment>().inventableEquipment;

        // 같은 장소로 이동하거나 이미 바꿨거나 해당 타입의 장비를 해당 위치에 둘 수 없을 경우
        if (displayIndex == dragIndex || isChange || ((displayEquipment & dragEquipment) == 0))
            return;

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

        for (int i = 8; i < displayPos.Length; i++)
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