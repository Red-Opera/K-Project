using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GStage2 : MonoBehaviour
{
    public GameObject portal;
    public float interactDistance = 3f; // 작용 가능한 거리
    public float fadeDuration = 0.5f; // 페이드 인/아웃 지속 시간
    public Image fadeImage; // 페이드 인/아웃에 사용할 이미지

    private bool isTransitioning = false; // 전환 중인지 여부를 나타내는 플래그

    void Update()
    {
        if (isTransitioning)
            return;

        // "Player" 태그 찾고 오브젝트 사이의 거리를 계산
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
            return;

        float distanceToPortal = Vector3.Distance(player.transform.position, portal.transform.position);

        // F 키를 누르면 씬 전환
        if (distanceToPortal < interactDistance && Input.GetKeyDown(KeyCode.F))
        {
            player.transform.position = new Vector3(0, 0, 0);
            // 코루틴을 사용하여 페이드 인/아웃 효과를 적용하여 씬 전환
            StartCoroutine(TransitionToScene());
        }
    }

    IEnumerator TransitionToScene()
    {
        isTransitioning = true;

        // 페이드 아웃 효과
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadeImage.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }

        // 씬 전환
        SceneManager.LoadScene("Stage2");

        // 페이드 인 효과
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            fadeImage.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }

        isTransitioning = false;
    }
}
