using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetItemUI : MonoBehaviour
{
    static public bool isShowUI = false;                    // 현재 UI 창이 켜져 있는지 여부

    [SerializeField] private float waitTime = 3.0f;         // UI 창을 보여주는 시간

    [SerializeField] private Image showItemImageTarget;     // 획득 UI 이미지를 보여주는 위치
    [SerializeField] private TextMeshProUGUI itemNameText;  // 획득한 아이템의 이름을 알려줄 Text

    public IEnumerator ShowItemUI(Sprite sprite, string itemName, EquipmentState equipmentState, Color color)
    {
        // 현재 아이템 획득 UI가 열려 있는지 알려줌
        isShowUI = true;

        // 입력한 값에 따라 UI에 표시함
        showItemImageTarget.sprite = sprite;
        itemNameText.text = itemName;
        itemNameText.color = color;

        InventroyPosition.CallAddItem(itemName, equipmentState);

        yield return new WaitForSeconds(waitTime);

        isShowUI = false;
        gameObject.SetActive(false);
    }
}
