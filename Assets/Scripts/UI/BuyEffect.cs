using UnityEngine;
using UnityEngine.UI;

public class BuyEffect : MonoBehaviour
{
    Image image;        // 구매 효과 오브젝트의 이미지

    public void Start()
    {
        image = GetComponent<Image>();
        Debug.Assert(image, "해당 오브젝트에 이미지가 없습니다.");
    }

    public void Update()
    {
        // 흐릿하게 보일 경우 없앰
        if (image.color.a < 0.05f)
            Destroy(gameObject);
    }
}
