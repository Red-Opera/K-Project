using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SaleItem : MonoBehaviour, IPointerClickHandler
{
    private static GameObject equidStore;           // ��� ���� ������Ʈ
    private static EquidStore equidStoreCompo;      // ��� ���� ���� ������Ʈ
    private static GameObject saleAlert;            // �ǸŽ� ǥ�õǴ� ������Ʈ
    private static GameObject infoText;             // ���� ����ϴ� �ؽ�Ʈ�� ��� ������Ʈ
    private static TextMeshProUGUI equidName;       // ��� �̸�
    private static Image saleItemIamge;             // �Ǹ��� �������� �̹���
    private static SaleItemFrame saleItemFrame;     // �Ǹ� UI ��ũ��Ʈ
    private static TextMeshProUGUI saleCostText;    // �Ǹ� �ݾ� �ؽ�Ʈ

    private EquidState state;                       // ��� �ɷ�ġ
    private MoveInventory moveInventory;            // ��� �̵����� ��ũ��Ʈ

    // �ش� �������� ������ ���콺�� Ŭ���� ���
    public void OnPointerClick(PointerEventData eventData)
    {
        // �Ǹ� UI�� ���� �ְų� ��� ���� UI�� ���� ���� ��� ó�� ����, �̹� �������� ���� �Ǹ� ����
        if (saleAlert.activeSelf || !equidStore.activeSelf || moveInventory.displayIndex < 8)
            return;

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            saleAlert.SetActive(true);

            // �Ǹ��� �̹����� ǥ���ϰ� �Ǹ��� ��� �ε����� �ѱ�
            saleItemIamge.sprite = transform.GetChild(0).GetComponent<Image>().sprite;
            saleItemFrame.saleItemIndex = moveInventory.displayIndex;

            // �Ǹ� �ݾ��� �����
            for (int i = 0; i < equidStoreCompo.selectables.Count; i++)
            {
                if (saleItemIamge.sprite == equidStoreCompo.selectables[i].sprite)
                    saleCostText.text = (equidStoreCompo.selectables[i].cost * 0.8f).ToString("#,##0");
            }

            // ��� ���� ���� ����
            for (int i = 0; i < 3; i++)
                infoText.transform.GetChild(i).gameObject.SetActive(false);

            int infoCount = 0;  // ���� ����

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
                    Debug.Assert(false, "�߸��� ���� Ÿ��");

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
            Debug.Assert(equidStore != null, "��� ���� UI�� �����ϴ�.");

            equidStore.SetActive(true);

            equidStoreCompo = equidStore.GetComponent<EquidStore>();
            Debug.Assert(equidStoreCompo != null, "��� ���� ������Ʈ�� �����ϴ�.");

            saleAlert = equidStore.transform.Find("Alert").gameObject;
            Debug.Assert(saleAlert != null, "��� �Ǹ� UI�� �����ϴ�.");

            Transform infoFrame = saleAlert.transform.Find("InfoFrame");

            infoText = infoFrame.Find("Ability").gameObject;
            Debug.Assert(infoText != null, "��� ������ ��� ������Ʈ�� �����ϴ�.");

            equidName = infoFrame.Find("EquidNameLabel").GetChild(0).GetComponent<TextMeshProUGUI>();
            Debug.Assert(equidName != null, "��� �̸��� �ִ� �ؽ�Ʈ�� �����ϴ�.");

            saleItemIamge = infoFrame.Find("ItemFrame").GetChild(0).GetComponent<Image>();
            Debug.Assert(saleItemIamge != null, "�Ǹ��� �̹��� �������� �����ϴ�.");

            saleItemFrame = saleAlert.GetComponent<SaleItemFrame>();
            Debug.Assert(saleItemFrame != null, "��� UI ��ũ��Ʈ�� �����ϴ�.");

            saleCostText = infoFrame.Find("CostLabel").GetChild(0).GetComponent<TextMeshProUGUI>();
            Debug.Assert(saleCostText != null, "�Ǹ� �ݾ� �ؽ�Ʈ�� �����ϴ�.");

            equidStore.SetActive(false);
        }

        if (state == null)
        {
            state = GetComponent<EquidState>();             // ���� ���¸� ������
            moveInventory = GetComponent<MoveInventory>();  // ��� �̵����� ������Ʈ�� ������
        }
    }
}
