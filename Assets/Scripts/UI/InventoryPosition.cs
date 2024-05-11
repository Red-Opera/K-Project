using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Reflection;

public class InventroyPosition : MonoBehaviour
{
    public static bool isChange = true;                 // �ڸ��� ��ȯ�ߴ��� ����
    public static bool isAddSucceed = false;            // ���� ���� ����

    [SerializeField] private GameObject slots;          // ������ �� �ִ� ������Ʈ�� ��� ����
    [SerializeField] private GameObject itemDisplay;    // �������� �����ϱ� ���� ������Ʈ

    private List<GameObject> displayData;               // ���� �������� ������
    private Transform[] displayPos;                     // �������� ������ �� �ִ� ������Ʈ

    public static event System.Action<string, EquipmentState, EquidState> OnAddItem;        // �� ��ũ��Ʈ�� ������ �ִ� ��� ������Ʈ�� ������ �̺�Ʈ
    public List<Sprite> spriteData = new List<Sprite>();                        // �߰��� ������ �̹���
    private Dictionary<string, Sprite> sprites;                                 // �߰��� ������ �̸��� �̹��� �迭

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

            // �������� ��ġ�� ���� ������Ʈ���� �˷���
            item.GetComponent<MoveInventory>().displayIndex = i;

            // ������Ʈ�� ���ʴ�� �κ��丮�� ����
            item.transform.localPosition = Vector3.zero;
        }

        sprites = new Dictionary<string, Sprite>();

        foreach (Sprite sprite in spriteData)
            sprites.Add(sprite.name, sprite);

        OnAddItem += AddItem;
    }

    // �������� ��ġ�� �����ϴ� �޼ҵ�
    public void ChangePos(int displayIndex, int dragIndex)
    {
        if (displayPos[displayIndex].childCount == 0)
            return;

        // �̵��� ���, �ٿ� ���� ��ġ
        EquipmentState displayEquipment = displayPos[displayIndex].GetChild(0).GetComponent<InventableEquipment>().inventableEquipment;
        EquipmentState dragEquipment = displayPos[dragIndex].GetComponent<InventableEquipment>().inventableEquipment;

        // ���� ��ҷ� �̵��ϰų� �̹� �ٲ�ų� �ش� Ÿ���� ��� �ش� ��ġ�� �� �� ���� ���
        if (displayIndex == dragIndex || isChange || ((displayEquipment & dragEquipment) == 0))
            return;

        // �κ��丮�� ������ �ѱ� ���
        if (displayData == null || displayIndex < 0 || dragIndex < 0 || displayIndex >= displayPos.Length || dragIndex >= displayPos.Length)
        {
            Debug.Assert(false, "Error (Out of Range) : �߸��� �ε����� ���޵Ǿ����ϴ�.");
            return;
        }

        // ������ ��ġ�� ��������� �ű�� ���
        if (displayPos[dragIndex].childCount == 0)
        {
            Transform moveInventory = displayPos[displayIndex].GetChild(0);

            moveInventory.GetComponent<MoveInventory>().displayIndex = dragIndex;
            moveInventory.SetParent(displayPos[dragIndex]);
        }

        // displayIndex�� dragIndex�� ������Ʈ�� ��ȯ
        else
        {
            if (displayPos[displayIndex].childCount <= 0 || displayPos[dragIndex].childCount <= 0)
                return;

            // ������ ��� ���� �� �ִ� ����� ������
            EquipmentState displaySlotEquidState = displayPos[displayIndex].GetComponent<InventableEquipment>().inventableEquipment;
            EquipmentState dragSlotEquidState = displayPos[dragIndex].GetComponent<InventableEquipment>().inventableEquipment;

            // ��� ������ ������
            EquipmentState displayState = displayPos[displayIndex].GetChild(0).GetComponent<InventableEquipment>().inventableEquipment;
            EquipmentState dragState = displayPos[dragIndex].GetChild(0).GetComponent<InventableEquipment>().inventableEquipment;

            // �Ű��� �� ��� ��ġ�� �ٲ� �� ���� ��� ����
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

        // �̵���ų ������Ʈ�� �з��� ������ ��ġ�� õõ�� �߾����� �ű�
        UpdateDisplayPositions(dragIndex);
        UpdateDisplayPositions(displayIndex);
    }

    // ������ ��ġ�� �������� ��ġ�� õõ�� �̵���Ű�� �޼ҵ�
    private void UpdateDisplayPositions(int moveIndex)
    {
        if (displayPos[moveIndex].childCount <= 0)
            return;

        displayPos[moveIndex].GetChild(0).localPosition = Vector3.zero;

        isChange = true;
    }

    // �������� �߰��ϰų� �����ϴ� �Լ�
    public void AddItem(string name, EquipmentState equipmentState, EquidState state = null)
    {
        Debug.Assert(sprites.ContainsKey(name), "�ش� �̸��� �������� �������� �ʽ��ϴ�");

        for (int i = 8; i < displayPos.Length; i++)
        {
            // ������ ã��
            Transform slot = displayPos[i];

            // �ش� ���Կ� �������� ���� �� �ִ��� Ȯ��
            if (slot.childCount <= 0)
            {
                // �ش� �������� ���� �� �κ��丮�� �°� ����
                GameObject newItem = Instantiate(itemDisplay);
                newItem.GetComponent<InventableEquipment>().inventableEquipment = equipmentState;
                newItem.GetComponent<MoveInventory>().displayIndex = i;
                newItem.transform.GetChild(0).GetComponent<Image>().sprite = sprites[name];

                // �ش� �������� ����ġ ��Ŵ
                newItem.transform.SetParent(slot);
                newItem.transform.localPosition = Vector3.zero;

                if (state != null)
                {
                    // ��� �ʵ带 �����ͼ� ������.
                    FieldInfo[] allFields = typeof(State).GetFields(BindingFlags.Public | BindingFlags.Instance);
                    EquidState newItemState = newItem.GetComponent<EquidState>();
                    newItemState.state = ScriptableObject.CreateInstance<State>();

                    // ��� �ʵ� ���� ������ �ִ� ���� �κ��丮�� ������
                    foreach (FieldInfo field in allFields)
                        field.SetValue(newItemState.state, field.GetValue(state.state));
                }

                displayData.Add(newItem);
                isAddSucceed = true;
                return;
            }
        }

        // ������� ���� �� ó��
        isAddSucceed = false;
    }

    // �ٸ� ��ũ��Ʈ���� AddItem�� ȣ���ϱ� ���� ���Ǵ� �޼ҵ�
    public static void CallAddItem(string name, EquipmentState equipmentState, EquidState state = null)
    {
        OnAddItem(name, equipmentState, state);
    }
}