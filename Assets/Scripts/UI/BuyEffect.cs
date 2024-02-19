using UnityEngine;
using UnityEngine.UI;

public class BuyEffect : MonoBehaviour
{
    Image image; 

    public void Start()
    {
        image = GetComponent<Image>();
        Debug.Assert(image, "해당 오브젝트에 이미지가 없습니다.");
    }

    public void Update()
    {
        if (image.color.a < 0.05f)
            Destroy(gameObject);
    }
}
