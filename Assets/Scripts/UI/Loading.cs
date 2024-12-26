using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public static Loading instance;

    [SerializeField] private GameObject loadingUI;
    [SerializeField] private TextMeshProUGUI nextMapName;
    [SerializeField] private TextMeshProUGUI persentText;
    [SerializeField] private Slider persentSlider;

    private Volume postprocessVolume;

    private void Start()
    {
        Debug.Assert(loadingUI != null, "Error (Null Reference) : 로딩 UI가 존재하지 않습니다.");
        Debug.Assert(nextMapName != null, "Error (Null Reference) : 이동하는 맵 이름 UI가 존재하지 않습니다.");
        Debug.Assert(persentText != null, "Error (Null Reference) : 현재 진행 퍼센트 표시하는 UI가 존재하지 않습니다.");
        Debug.Assert(persentSlider != null, "Error (Null Reference) : 현재 진행 표시하는 슬라이더가 존재하지 않습니다.");

        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += CloseUI;
    }

    private void CloseUI(Scene scene, LoadSceneMode mode)
    {
        loadingUI.SetActive(false);
    }

    public static void LoadScene(string sceneName)
    {
        instance.StartCoroutine(instance.StartLoadScene(sceneName));
    }

    private IEnumerator StartLoadScene(string sceneName)
    {
        persentSlider.value = 0.0f;
        persentText.text = "0.0%";

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false; // 씬 자동 전환 비활성화

        loadingUI.SetActive(true);
        postprocessVolume = GameObject.Find("Global Volume").GetComponent<Volume>();
        postprocessVolume.weight = 0;

        // 이동 씬 이름을 재조정
        sceneName = sceneName.Replace("Stage", "");
        sceneName += " Level";

        nextMapName.text = sceneName;

        float targetProgress = 0f;

        while (!operation.isDone)
        {
            // 목표 진행 상태를 계산
            targetProgress = Mathf.Clamp01(operation.progress / 0.9f);

            // 슬라이더의 현재 값과 목표 값을 Lerp로 부드럽게 변경
            persentSlider.value = Mathf.Lerp(persentSlider.value, targetProgress, Time.deltaTime * 5f);

            // 텍스트 업데이트
            persentText.text = persentSlider.value.ToString("##0.0%");

            // 만약 진행률이 0.9f(90%)에 도달하면 종료 루틴을 준비
            if (operation.progress >= 0.9f)
            {
                // 슬라이더와 텍스트를 100%로 설정
                persentSlider.value = 1.0f;
                persentText.text = "100.0%";

                // 잠시 대기 후 씬을 활성화할 수 있습니다.
                yield return new WaitForSeconds(0.05f);

                operation.allowSceneActivation = true; // 씬 전환 활성화
            }

            yield return null;
        }

        postprocessVolume.weight = 1;
    }
}