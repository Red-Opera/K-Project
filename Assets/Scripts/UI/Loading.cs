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
        Debug.Assert(loadingUI != null, "Error (Null Reference) : �ε� UI�� �������� �ʽ��ϴ�.");
        Debug.Assert(nextMapName != null, "Error (Null Reference) : �̵��ϴ� �� �̸� UI�� �������� �ʽ��ϴ�.");
        Debug.Assert(persentText != null, "Error (Null Reference) : ���� ���� �ۼ�Ʈ ǥ���ϴ� UI�� �������� �ʽ��ϴ�.");
        Debug.Assert(persentSlider != null, "Error (Null Reference) : ���� ���� ǥ���ϴ� �����̴��� �������� �ʽ��ϴ�.");

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
        operation.allowSceneActivation = false; // �� �ڵ� ��ȯ ��Ȱ��ȭ

        loadingUI.SetActive(true);
        postprocessVolume = GameObject.Find("Global Volume").GetComponent<Volume>();
        postprocessVolume.weight = 0;

        // �̵� �� �̸��� ������
        sceneName = sceneName.Replace("Stage", "");
        sceneName += " Level";

        nextMapName.text = sceneName;

        float targetProgress = 0f;

        while (!operation.isDone)
        {
            // ��ǥ ���� ���¸� ���
            targetProgress = Mathf.Clamp01(operation.progress / 0.9f);

            // �����̴��� ���� ���� ��ǥ ���� Lerp�� �ε巴�� ����
            persentSlider.value = Mathf.Lerp(persentSlider.value, targetProgress, Time.deltaTime * 5f);

            // �ؽ�Ʈ ������Ʈ
            persentText.text = persentSlider.value.ToString("##0.0%");

            // ���� ������� 0.9f(90%)�� �����ϸ� ���� ��ƾ�� �غ�
            if (operation.progress >= 0.9f)
            {
                // �����̴��� �ؽ�Ʈ�� 100%�� ����
                persentSlider.value = 1.0f;
                persentText.text = "100.0%";

                // ��� ��� �� ���� Ȱ��ȭ�� �� �ֽ��ϴ�.
                yield return new WaitForSeconds(0.05f);

                operation.allowSceneActivation = true; // �� ��ȯ Ȱ��ȭ
            }

            yield return null;
        }

        postprocessVolume.weight = 1;
    }
}