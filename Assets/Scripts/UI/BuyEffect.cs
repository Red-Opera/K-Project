using UnityEngine;
using UnityEngine.UI;

public class BuyEffect : MonoBehaviour
{
    Image image;        // ���� ȿ�� ������Ʈ�� �̹���

    public void Start()
    {
        image = GetComponent<Image>();
        Debug.Assert(image, "�ش� ������Ʈ�� �̹����� �����ϴ�.");
    }

    public void Update()
    {
        // �帴�ϰ� ���� ��� ����
        if (image.color.a < 0.05f)
            Destroy(gameObject);
    }
}
