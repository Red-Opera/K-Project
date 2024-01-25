using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomMouseIndex : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public static int selectedIndex = 0;
    public float duration = 1f;

    [SerializeField] private GameObject pointImage;
    [SerializeField] private GameObject selectedImage;
    [SerializeField] private GameObject contents;

    private Image selectImageCompo;

    private int thisIndex = 0;
    private bool isChange = false;

    public void Start()
    {
        Transform parent = transform.parent;

        for (int i = 0; i < parent.childCount; i++)
            if (parent.GetChild(i).gameObject == gameObject)
                thisIndex = i;

        selectImageCompo = selectedImage.GetComponent<Image>();

        if (contents == null)
            contents = GameObject.Find("Contents");
    }

    public void Update()
    {
        if (selectedIndex == thisIndex && selectImageCompo.color.a <= 0.1 && !isChange) 
            StartCoroutine(TurnOnOff(true));

        else if (selectedIndex != thisIndex && selectImageCompo.material.color.a >= 0.9 && !isChange)
            StartCoroutine(TurnOnOff(false));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        contents.transform.GetChild(selectedIndex).gameObject.SetActive(false);

        selectedIndex = thisIndex;

        contents.transform.GetChild(thisIndex).gameObject.SetActive(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointImage.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointImage.SetActive(false);
    }

    private IEnumerator TurnOnOff(bool isOn)
    {
        isChange = true;

        Color targetColor = selectImageCompo.color;
        float targetAlpha = isOn ? 1f : 0f;
        float elapsedTime = 0f;

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
