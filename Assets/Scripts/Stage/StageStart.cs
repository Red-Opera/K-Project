using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageStart : MonoBehaviour
{
    public GameObject portal;
    public float interactDistance = 3f; // 작용 가능한 거리
    public float fadeDuration = 1.0f; // 화면 페이드 인/아웃 지속 시간

    private bool isTransitioning = false; // 전환 중인지 여부를 나타내는 플래그

    void Update()
    {
        if (isTransitioning)
            return;

        // "Player"테그 찾고 오브젝트 사이의 거리를 계산
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
            return;

        float distanceToPortal = Vector3.Distance(player.transform.position, portal.transform.position);

        // f키 누르면 씬전환
        if (distanceToPortal < interactDistance && Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(TransitionToScene());
        }
    }

    IEnumerator TransitionToScene()
    {
        isTransitioning = true;

        // 화면 어두워지기
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            // 화면을 어둡게 만듭니다. 여기서는 화면 전체를 가리는 판넬이나 이미지를 사용할 수 있습니다.
            // 판넬의 알파 값을 조절하여 어두운 효과를 만듭니다.
            // 판넬이나 이미지의 알파 값 조절 방법은 적절히 수정해야 합니다.
            yield return null;
        }

        // 씬 전환
        SceneManager.LoadScene("Stage1");

        // 화면 밝아지기
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            // 화면을 밝게 만듭니다. 화면이 어두워졌던 것을 되돌립니다.
            // 판넬이나 이미지의 알파 값을 조절하여 밝은 효과를 만듭니다.
            // 판넬이나 이미지의 알파 값 조절 방법은 적절히 수정해야 합니다.
            yield return null;
        }

        isTransitioning = false;
    }
}
