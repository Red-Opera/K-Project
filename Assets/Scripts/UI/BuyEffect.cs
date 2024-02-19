using UnityEngine;
using UnityEngine.UI;

public class BuyEffect : MonoBehaviour
{
    Image image; 

    public void Start()
    {
        image = GetComponent<Image>();
        Debug.Assert(image, "�ش� ������Ʈ�� �̹����� �����ϴ�.");
    }

    public void Update()
    {
        if (image.color.a < 0.05f)
            Destroy(gameObject);
    }
}
