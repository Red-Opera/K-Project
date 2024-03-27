using UnityEngine;
using TMPro;

public class TextFadeInOut : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;  // 페이드 인/아웃할 TextMeshProUGUI 객체
    [SerializeField] private float fadeDuration = 1f;   // 페이드 인/아웃에 걸리는 총 시간

    private bool fadingOut = false;                             // 페이드 아웃 중인지 나타내는 플래그
    private Color originalColor;                                // 텍스트의 원래 색상
    private Color transparentColor = new Color(1f, 1f, 1f, 0f); // 완전히 투명한 색상

    private float startTime;

    private void Start()
    {
        originalColor = textMesh.color; // 텍스트의 원래 색상 저장
        startTime = Time.time;          // 시작 시간

        StartFadeOut(); // 시작할 때 페이드 아웃 시작
    }

    private void Update()
    {
        // 페이드 아웃 중일 때
        if (fadingOut)
        {
            // 투명해지는 정도 계산
            float alpha = Mathf.Clamp01(1f - (fadeDuration - (Time.time - startTime)) / fadeDuration);

            // 텍스트 색상을 원래 색상에서 완전 투명한 색상으로 변경
            textMesh.color = Color.Lerp(originalColor, transparentColor, alpha);

            if (Time.time - startTime >= fadeDuration) // 페이드 아웃이 완료되면
            {
                fadingOut = false;  // 페이드 아웃 종료
                StartFadeIn();      // 페이드 인 시작
            }
        }

        // 페이드 인 중일 때
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
