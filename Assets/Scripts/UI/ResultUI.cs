using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    [SerializeField] private GameObject resultUI;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI currentLevelText;
    [SerializeField] private TextMeshProUGUI resultGoldText;
    [SerializeField] private TextMeshProUGUI resultEXPText;
    [SerializeField] private TextMeshProUGUI currentLevel;
    [SerializeField] private TextMeshProUGUI currentEXPText;
    [SerializeField] private TextMeshProUGUI nextEXPText;
    [SerializeField] private TextMeshProUGUI currentEXPPersentText;
    [SerializeField] private Slider currentEXPPersentSlider;

    [SerializeField] private int startEXP;
    [SerializeField] private int level1PerEXPUp;

    private static float playTime = 0.0f;
    private static float getGold = 0;
    private static float getEXP = 0;

    private bool isPlayTimeReset = false;

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceceLoaded;
        OnSceceLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);

        Invoke("GameIsEnd", 5.0f);
    }

    public void Update()
    {
        if (isPlayTimeReset)
            playTime += Time.deltaTime;
    }

    // ������ ������ ��� ȣ��Ǵ� �޼ҵ�
    public void GameIsEnd()
    {
        // ��� UI�� Ȱ��ȭ��
        resultUI.SetActive(true);
        isPlayTimeReset = false;

        StartCoroutine(ShowPlayTime()); // �÷��̾� �� �ð��� ������
        GetCurrentStage();              // ���� ���� ���������� ������

        // �÷��̾� ����, ���� ����ġ�� ���� ���� ������ �ۼ�Ʈ
        State playerState = GameManager.info.allPlayerState;
        float currentEXPPersent = (float)playerState.currentExp / ((level1PerEXPUp * (playerState.level - 1)) + startEXP);

        resultGoldText.text = getGold.ToString("#,##0");
        resultEXPText.text = getEXP.ToString("#,##0");
        currentLevel.text = playerState.level.ToString("#,##0");
        currentEXPText.text = playerState.currentExp.ToString("#,##0");
        nextEXPText.text = ((level1PerEXPUp * (playerState.level - 1)) + startEXP).ToString("#,##0");
        currentEXPPersentSlider.value = currentEXPPersent;
        currentEXPPersentText.text = currentEXPPersent.ToString("#0.0%");
    }

    public static void GetEXP(int exp)
    {
        getEXP += exp;
    }

    public static void GetGold(int coin)
    {
        getGold += coin;

        GameManager.info.playerState.money += coin;
    }

    // ���� ���� �������� �޼ҵ�
    private void GetCurrentStage()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName.StartsWith("Stage"))
        {
            int currentLevel = int.Parse(currentSceneName[currentSceneName.Length - 1].ToString());
            currentLevelText.text = currentLevel + "���� ";

            Transform stages = GameObject.Find(currentLevel + "Stages").transform;

            for (int i = 0; i < stages.childCount; i++)
            {
                if (stages.GetChild(i).gameObject.activeSelf)
                {
                    string stageName = stages.GetChild(i).name;

                    if (stageName.StartsWith("stage"))
                        currentLevelText.text += stageName[stageName.Length - 1].ToString() + "��������";

                    else
                        currentLevelText.text += "������������";

                    break;
                }
            }
        }
    }

    private IEnumerator ShowPlayTime()
    {
        float nowTime = Time.time;

        while (Time.time - nowTime <= 2.0f)
        {
            float currentTime = Mathf.Lerp(0.0f, playTime, (Time.time - nowTime) / 2);

            int hours = (int)(currentTime / 3600);
            int minutes = (int)((currentTime % 3600) / 60);
            int seconds = (int)(currentTime % 60);

            timeText.text = hours.ToString("#0") + "�ð� " + minutes.ToString("00") + "�� " + seconds.ToString("00") + "��";

            yield return null;
        }

        int hour = (int)(playTime / 3600);
        int minute = (int)((playTime % 3600) / 60);
        int second = (int)(playTime % 60);

        timeText.text = hour.ToString("#0") + "�ð� " + minute.ToString("00") + "�� " + second.ToString("00") + "��";
    }

    private void OnSceceLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        if (scene.name == "Stage1" && !isPlayTimeReset)
        {
            isPlayTimeReset = true;
            playTime = 0.0f;
            getGold = 0;
            getEXP = 0;
        }
    }
}
