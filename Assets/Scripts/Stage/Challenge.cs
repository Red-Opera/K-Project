using UnityEngine;
using UnityEngine.UI;

public class Challenge : MonoBehaviour
{
    public GameObject boss;            // 보스 객체 참조
    public GameObject currentStage;    // 현재 스테이지 오브젝트
    public GameObject nextStage;       // 다음 스테이지 오브젝트
    public GameObject player;          
    public Image fadeImage;            
    public float fadeDuration = 1.0f; 
    private bool bossDefeated = false;

    void Update()
    {
        // 보스의 생명력이 0 이하가 되면 처리
        if (!bossDefeated && boss.GetComponent<Boss>().IsDefeated())
        {
            bossDefeated = true;
            StartCoroutine(TransitionToNextStage());
        }
    }

    private IEnumerator TransitionToNextStage()
    {
        // 1. 페이드아웃
        float elapsedTime = 0;
        Color fadeColor = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeColor.a = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            fadeImage.color = fadeColor;
            yield return null;
        }

        // 2. 플레이어 위치 이동
        player.transform.position = new Vector3(20, -1, player.transform.position.z);

        // 3. 현재 스테이지 비활성화 및 다음 스테이지 활성화
        currentStage.SetActive(false);
        nextStage.SetActive(true);

        // 4. 페이드인
        elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeColor.a = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            fadeImage.color = fadeColor;
            yield return null;
        }
    }
}
