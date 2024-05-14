using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CurrentSceneNameUI : MonoBehaviour
{
    [SerializeField] private Image nameFrame;               // 이름 프레임
    [SerializeField] private TextMeshProUGUI nameText;      // 이름 텍스트
    [SerializeField] private GameObject stages;             // 해당 스테이지

    private static CurrentSceneNameUI instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Debug.Assert(nameFrame != null, "이름 프레임이 존재하지 않습니다.");
        Debug.Assert(nameText != null, "이름 텍스트가 존재하지 않습니다.");

        // 초기 상태를 투명하게 설정
        SetAlpha(0f);

        StartCoroutine(ShowCurrentSceneName());
    }

    // 알파 값을 바꿔주는 메소드
    private void SetAlpha(float alpha)
    {
        // 이름 프레임과 텍스트의 알파값 설정
        nameFrame.color = new Color(1, 1, 1, alpha);
        nameText.color = new Color(1, 1, 1, alpha);
    }

    // 현재 씬 이름을 보여주는 메소드
    private IEnumerator ShowCurrentSceneName()
    {
        nameFrame.gameObject.SetActive(true);
        StageNameChange();

        // 점점 불투명해지는 애니메이션
        for (float t = 0; t <= 1; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(0f, 1f, t);
            SetAlpha(alpha);
            yield return null;
        }

        // 2초 대기
        yield return new WaitForSeconds(2f);

        // 점점 투명해지는 애니메이션
        for (float t = 0; t <= 1; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(1f, 0f, t);
            SetAlpha(alpha);
            yield return null;
        }

        nameFrame.gameObject.SetActive(false);
    }

    // 스테이지 이름을 바꿔주는 메소드
    private void StageNameChange()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "Map")
            nameText.text = "Town";

        // 현재 어떤 씬인지 가져옴
        if (currentSceneName.StartsWith("Stage"))
        {
            int currentLevel = int.Parse(currentSceneName[currentSceneName.Length - 1].ToString());
            nameText.text = currentLevel + " Level ";

            // 씬 중 어떤 레벨에 해당하는지 가져옴
            Transform stages = GameObject.Find(currentLevel + "Stages").transform;

            for (int i = 0; i < stages.childCount; i++)
            {
                if (stages.GetChild(i).gameObject.activeSelf)
                {
                    string stageName = stages.GetChild(i).name;

                    // 일반 스테이지인 경우
                    if (stageName.StartsWith("stage") || stageName.StartsWith("Stage"))
                    {
                        nameText.text += stageName[stageName.Length - 1].ToString() + " Stage";
                        BackgroundSound.NoBossClip();
                    }

                    else if (stageName.StartsWith("EventStage"))
                    {
                        char num = stageName[stageName.Length - 1];

                        if (num == '1')
                            nameText.text += "INN Store";

                        else if (num == '2')
                            nameText.text += "Equid Store";
                    }    

                    // 보스 스테이지인 경우
                    else
                    {
                        nameText.text += " BossStage";
                        BackgroundSound.StartBossClip();
                    }

                    break;
                }
            }
        }
    }

    public static void StartSceneNameAnimation()
    {
        instance.StartCoroutine(instance.ShowCurrentSceneName());
    }
}