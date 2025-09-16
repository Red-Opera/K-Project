using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StageStart : MonoBehaviour
{
    public GameObject portal;
    public float interactDistance = 3f;
    public float fadeDuration = 0.5f;
    public Image fadeImage;

    public GameObject modeSelectionUI; // ModeSelectionPanel
    public Button normalModeButton;
    public Button challengeModeButton;

    private bool isTransitioning = false;
    private bool modeSelected = false; // UI에서 선택했는지 확인

    void Start()
    {
        // 버튼 클릭 이벤트 연결
        normalModeButton.onClick.AddListener(() => SelectMode(false));
        challengeModeButton.onClick.AddListener(() => SelectMode(true));

        modeSelectionUI.SetActive(false);
    }

    void Update()
    {
        if (isTransitioning || modeSelected)
            return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
            return;

        float distanceToPortal = Vector3.Distance(player.transform.position, portal.transform.position);

        if (distanceToPortal < interactDistance && Input.GetKeyDown(KeyCode.F))
        {
            // UI 활성화
            modeSelectionUI.SetActive(true);
        }
    }

    void SelectMode(bool challenge)
    {
        // 선택한 모드 저장
        if (GameManager.instance != null)
            GameManager.instance.challenge = challenge;

        modeSelected = true;
        modeSelectionUI.SetActive(false);

        // 씬 전환 시작
        StartCoroutine(TransitionToScene());
    }

    IEnumerator TransitionToScene()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        isTransitioning = true;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadeImage.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }

        player.transform.position = Vector3.zero;

        // 씬 전환
        Loading.LoadScene("Stage1");

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
