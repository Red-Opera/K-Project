using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetItemUI : MonoBehaviour
{
    static public bool isShowUI = false;                    // ���� UI â�� ���� �ִ��� ����

    [SerializeField] private float waitTime = 3.0f;         // UI â�� �����ִ� �ð�

    [SerializeField] private Image showItemImageTarget;     // ȹ�� UI �̹����� �����ִ� ��ġ
    [SerializeField] private TextMeshProUGUI itemNameText;  // ȹ���� �������� �̸��� �˷��� Text

    public IEnumerator ShowItemUI(Sprite sprite, string itemName, EquipmentState equipmentState, Color color)
    {
        // ���� ������ ȹ�� UI�� ���� �ִ��� �˷���
        isShowUI = true;

        // �Է��� ���� ���� UI�� ǥ����
        showItemImageTarget.sprite = sprite;
        itemNameText.text = itemName;
        itemNameText.color = color;

        InventroyPosition.CallAddItem(itemName, equipmentState);

        yield return new WaitForSeconds(waitTime);

        isShowUI = false;
        gameObject.SetActive(false);
    }
}
