using UnityEngine;
using UnityEngine.EventSystems;

public class EquidStoreItem : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector] public int thisIndex;         // �� ������Ʈ�� �ε���

    private static EquidStore equidStore;           // ��� ������ �����ϴ� ��ũ��Ʈ
    private static int currentSelectedIndex = 0;    // ���� ������ ������ �ε���

    public void Start()
    {
        if (equidStore == null)
        {
            equidStore = GameObject.Find("EquidStore").GetComponent<EquidStore>();
            Debug.Assert(equidStore != null, "���� ���� �����ϴ�.");
        }
    }

    public static void BuyItem()
    {
        equidStore.BuyEffect(currentSelectedIndex);
    }

    // �ش� �������� ���ý�ó���Ǵ� �޼ҵ�
    public void DisplayItem()
    {
        equidStore.OnSelectedItem(thisIndex, ref currentSelectedIndex);
    }

    // �ش� �������� Ŭ���� ����Ǵ� �޼ҵ�
    public void OnPointerClick(PointerEventData eventData)
    {
        DisplayItem();
    }
}
