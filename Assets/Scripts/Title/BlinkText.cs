using UnityEngine;
using TMPro;

public class TextFadeInOut : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;  // ���̵� ��/�ƿ��� TextMeshProUGUI ��ü
    [SerializeField] private float fadeDuration = 1f;   // ���̵� ��/�ƿ��� �ɸ��� �� �ð�

    private bool fadingOut = false;                             // ���̵� �ƿ� ������ ��Ÿ���� �÷���
    private Color originalColor;                                // �ؽ�Ʈ�� ���� ����
    private Color transparentColor = new Color(1f, 1f, 1f, 0f); // ������ ������ ����

    private float startTime;

    private void Start()
    {
        originalColor = textMesh.color; // �ؽ�Ʈ�� ���� ���� ����
        startTime = Time.time;          // ���� �ð�

        StartFadeOut(); // ������ �� ���̵� �ƿ� ����
    }

    private void Update()
    {
        // ���̵� �ƿ� ���� ��
        if (fadingOut)
        {
            // ���������� ���� ���
            float alpha = Mathf.Clamp01(1f - (fadeDuration - (Time.time - startTime)) / fadeDuration);

            // �ؽ�Ʈ ������ ���� ���󿡼� ���� ������ �������� ����
            textMesh.color = Color.Lerp(originalColor, transparentColor, alpha);

            if (Time.time - startTime >= fadeDuration) // ���̵� �ƿ��� �Ϸ�Ǹ�
            {
                fadingOut = false;  // ���̵� �ƿ� ����
                StartFadeIn();      // ���̵� �� ����
            }
        }

        // ���̵� �� ���� ��
        if (!fadingOut)
        {
            float alpha = Mathf.Clamp01(((Time.time - startTime) - fadeDuration) / fadeDuration);

            textMesh.color = Color.Lerp(transparentColor, originalColor, alpha);

            if (Time.time - startTime >= fadeDuration * 2)
            {
                startTime = Time.time;

                fadingOut = true;
                StartFadeOut();
            }
        }
    }

    private void StartFadeOut()
    {
        fadingOut = true;
    }

    private void StartFadeIn()
    {
        fadingOut = false;
    }
}
