using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomMouseIndex : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public static int selectedIndex = 0;                // 선택된 직업 프레임 인덱스
    public float duration = 1f;                         // 선택시 선택 효과 지속 시간

    [SerializeField] private Image pointImageColor;     // 마우스 포인터가 올라갈 시 색깔이 바뀌는 이미지
    [SerializeField] private GameObject selectedImage;  // 선택된 이미지
    [SerializeField] private GameObject contents;       // 내용을 저장하는 오브젝트

    private Image selectImageCompo;     // 선택한 이미지 컴포넌트
    private Color defualtColor;         // 기본 프레임 색깔

    private int thisIndex = 0;          // 이 프레임의 인덱스
    private bool isChange = false;      // 현재 선택된 프레임이 변경 중인 여부

    public void Start()
    {
        // 해당 객체의 오브젝트를 가져옴
        Transform parent = transform.parent;

        // 각 직업을 전시하는 프레임마다 자신의 인덱스를 지정함 
        for (int i = 0; i < parent.childCount; i++)
            if (parent.GetChild(i).gameObject == gameObject)
                thisIndex = i;

        selectImageCompo = selectedImage.GetComponent<Image>();
        defualtColor = pointImageColor.color;

        // 직업 세부내용을 담는 오브젝트가 존재하지 않는 경우 찾음
        if (contents == null)
            contents = GameObject.Find("Contents");
    }

    public void Update()
    {
        // 현재 선택한 프레임이 이 프레임이고 아직 선택하지 않은 프레임일 경우 이 프레임을 활성화
        if (selectedIndex == thisIndex && selectImageCompo.color.a <= 0.1 && !isChange) 
            StartCoroutine(TurnOnOff(true));

        // 현재 선택한 프레임이 이 프레임이 아니고 이미 선택됐던 이미지일 경우 이 이 프레임을 비활성화
        else if (selectedIndex != thisIndex && selectImageCompo.material.color.a >= 0.9 && !isChange)
            StartCoroutine(TurnOnOff(false));
    }

    // 해당 프레임을 선택한 경우
    public void OnPointerClick(PointerEventData eventData)
    {
        // 선택했던 기존 직업의 세부내용을 끔
        contents.transform.GetChild(selectedIndex).gameObject.SetActive(false);

        selectedIndex = thisIndex;

        // 이 프레임의 직업 세부 내용을 낌
        contents.transform.GetChild(thisIndex).gameObject.SetActive(true);
    }

    // 해당 이미지가 마우스 포인터로 가리키고 있는 경우
    public void OnPointerEnter(PointerEventData eventData)
    {
        pointImageColor.color = defualtColor + new Color(0.3f, 0.3f, 0.3f);
    }

    // 해당 이미지에서 마우스가 나간 경우
    public void OnPointerExit(PointerEventData eventData)
    {
        pointImageColor.color = defualtColor;
    }

    // 해당 프레임을 선택 또는 미선택으로 바꿔주는 메소드
    private IEnumerator TurnOnOff(bool isOn)
    {
        isChange = true;

        // 현재 선택한 이미지가 활성화 되어 있는 경우 비활성하고 비활성화인 경우 활성화 시킴
        Color targetColor = selectImageCompo.color;
        float targetAlpha = isOn ? 1f : 0f;
        float elapsedTime = 0f;

        // 점점 활성화/비활성화되도록 설정
        while (elapsedTime < duration)
        {
            selectImageCompo.color = new Color(targetColor.r, targetColor.g, targetColor.b, Mathf.Lerp(targetColor.a, targetAlpha, elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        selectImageCompo.color = new Color(targetColor.r, targetColor.g, targetColor.b, targetAlpha);
        
        isChange = false;
    }
}
