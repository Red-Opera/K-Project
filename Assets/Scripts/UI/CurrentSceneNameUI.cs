using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CurrentSceneNameUI : MonoBehaviour
{
    [SerializeField] private Image nameFrame;               // �̸� ������
    [SerializeField] private TextMeshProUGUI nameText;      // �̸� �ؽ�Ʈ
    [SerializeField] private GameObject stages;             // �ش� ��������

    private static CurrentSceneNameUI instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Debug.Assert(nameFrame != null, "�̸� �������� �������� �ʽ��ϴ�.");
        Debug.Assert(nameText != null, "�̸� �ؽ�Ʈ�� �������� �ʽ��ϴ�.");

        // �ʱ� ���¸� �����ϰ� ����
        SetAlpha(0f);

        StartCoroutine(ShowCurrentSceneName());
    }

    // ���� ���� �ٲ��ִ� �޼ҵ�
    private void SetAlpha(float alpha)
    {
        // �̸� �����Ӱ� �ؽ�Ʈ�� ���İ� ����
        nameFrame.color = new Color(1, 1, 1, alpha);
        nameText.color = new Color(1, 1, 1, alpha);
    }

    // ���� �� �̸��� �����ִ� �޼ҵ�
    private IEnumerator ShowCurrentSceneName()
    {
        nameFrame.gameObject.SetActive(true);
        StageNameChange();

        // ���� ������������ �ִϸ��̼�
        for (float t = 0; t <= 1; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(0f, 1f, t);
            SetAlpha(alpha);
            yield return null;
        }

        // 2�� ���
        yield return new WaitForSeconds(2f);

        // ���� ���������� �ִϸ��̼�
        for (float t = 0; t <= 1; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(1f, 0f, t);
            SetAlpha(alpha);
            yield return null;
        }

        nameFrame.gameObject.SetActive(false);
    }

    // �������� �̸��� �ٲ��ִ� �޼ҵ�
    private void StageNameChange()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "Map")
            nameText.text = "Town";

        // ���� � ������ ������
        if (currentSceneName.StartsWith("Stage"))
        {
            int currentLevel = int.Parse(currentSceneName[currentSceneName.Length - 1].ToString());
            nameText.text = currentLevel + " Level ";

            // �� �� � ������ �ش��ϴ��� ������
            Transform stages = GameObject.Find(currentLevel + "Stages").transform;

            for (int i = 0; i < stages.childCount; i++)
            {
                if (stages.GetChild(i).gameObject.activeSelf)
                {
                    string stageName = stages.GetChild(i).name;

                    // �Ϲ� ���������� ���
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

                    // ���� ���������� ���
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