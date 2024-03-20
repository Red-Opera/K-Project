using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomMouseIndex : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public static int selectedIndex = 0;                // ���õ� ���� ������ �ε���
    public float duration = 1f;                         // ���ý� ���� ȿ�� ���� �ð�

    [SerializeField] private Image pointImageColor;     // ���콺 �����Ͱ� �ö� �� ������ �ٲ�� �̹���
    [SerializeField] private GameObject selectedImage;  // ���õ� �̹���
    [SerializeField] private GameObject contents;       // ������ �����ϴ� ������Ʈ

    private Image selectImageCompo;     // ������ �̹��� ������Ʈ
    private Color defualtColor;         // �⺻ ������ ����

    private int thisIndex = 0;          // �� �������� �ε���
    private bool isChange = false;      // ���� ���õ� �������� ���� ���� ����

    public void Start()
    {
        // �ش� ��ü�� ������Ʈ�� ������
        Transform parent = transform.parent;

        // �� ������ �����ϴ� �����Ӹ��� �ڽ��� �ε����� ������ 
        for (int i = 0; i < parent.childCount; i++)
            if (parent.GetChild(i).gameObject == gameObject)
                thisIndex = i;

        selectImageCompo = selectedImage.GetComponent<Image>();
        defualtColor = pointImageColor.color;

        // ���� ���γ����� ��� ������Ʈ�� �������� �ʴ� ��� ã��
        if (contents == null)
            contents = GameObject.Find("Contents");
    }

    public void Update()
    {
        // ���� ������ �������� �� �������̰� ���� �������� ���� �������� ��� �� �������� Ȱ��ȭ
        if (selectedIndex == thisIndex && selectImageCompo.color.a <= 0.1 && !isChange) 
            StartCoroutine(TurnOnOff(true));

        // ���� ������ �������� �� �������� �ƴϰ� �̹� ���õƴ� �̹����� ��� �� �� �������� ��Ȱ��ȭ
        else if (selectedIndex != thisIndex && selectImageCompo.material.color.a >= 0.9 && !isChange)
            StartCoroutine(TurnOnOff(false));
    }

    // �ش� �������� ������ ���
    public void OnPointerClick(PointerEventData eventData)
    {
        // �����ߴ� ���� ������ ���γ����� ��
        contents.transform.GetChild(selectedIndex).gameObject.SetActive(false);

        selectedIndex = thisIndex;

        // �� �������� ���� ���� ������ ��
        contents.transform.GetChild(thisIndex).gameObject.SetActive(true);
    }

    // �ش� �̹����� ���콺 �����ͷ� ����Ű�� �ִ� ���
    public void OnPointerEnter(PointerEventData eventData)
    {
        pointImageColor.color = defualtColor + new Color(0.3f, 0.3f, 0.3f);
    }

    // �ش� �̹������� ���콺�� ���� ���
    public void OnPointerExit(PointerEventData eventData)
    {
        pointImageColor.color = defualtColor;
    }

    // �ش� �������� ���� �Ǵ� �̼������� �ٲ��ִ� �޼ҵ�
    private IEnumerator TurnOnOff(bool isOn)
    {
        isChange = true;

        // ���� ������ �̹����� Ȱ��ȭ �Ǿ� �ִ� ��� ��Ȱ���ϰ� ��Ȱ��ȭ�� ��� Ȱ��ȭ ��Ŵ
        Color targetColor = selectImageCompo.color;
        float targetAlpha = isOn ? 1f : 0f;
        float elapsedTime = 0f;

        // ���� Ȱ��ȭ/��Ȱ��ȭ�ǵ��� ����
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
