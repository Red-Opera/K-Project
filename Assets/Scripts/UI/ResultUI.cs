using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    [SerializeField] private EquidStore equidStore;                             // ���� ���� UI ��ũ��Ʈ
    [HideInInspector] public Dictionary<string, SelectableItem> weaphonInfoEx;  // ���� �߰� ����

    [SerializeField] private GameObject resultUI;                       // ��� UI ������Ʈ
    [SerializeField] private TextMeshProUGUI timeText;                  // Ž�� �ð� ��� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI currentLevelText;          // ���� �������� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI resultGoldText;            // ���� ��� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI resultEXPText;             // ���� ����ġ �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI currentPlayerLevelText;    // �÷��̾� ���� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI currentEXPText;            // ���� ����ġ �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI nextEXPText;               // ���� ������ �ʿ��� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI currentEXPPersentText;     // ���� ����ġ�� ���� �ٸ� ���� �ؽ�Ʈ
    [SerializeField] private Slider currentEXPPersentSlider;            // ���� ������ ���� ���� ����ġ �ۼ�Ʈ
    [SerializeField] private GameObject itemFrame;                      // ȹ�� ������ ������
    [SerializeField] private Transform frameTarget;                     // ȹ�� ������ �������� ��ġ�� ��ġ

    [SerializeField] private int startEXP;          // 2������ �������� ����ġ
    [SerializeField] private int level1PerEXPUp;    // 1���� �ö󰥶� ���� �ʿ� ����ġ ������

    private GameObject tempWeaphonState;            // ���� ���� �ӽ� ��ü

    private static List<string> addItemList;        // Ȱ��ȭ�� �߰��� ������ ����Ʈ
    private static List<string> getItemList;        // Ž�� ��� ȹ���� ������
    private static ResultUI staticResultUI;         // Result UI static ����
    private static float playTime = 0.0f;           // Ž�� �ð�
    private static float getGold = 0;               // Ž��� ���� ���
    private static float getEXP = 0;                // Ž��� ���� ����ġ


    private bool isPlayTimeReset = false;

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceceLoaded;
        OnSceceLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);

        if (getItemList == null)
            getItemList = new List<string>();

        weaphonInfoEx = new Dictionary<string, SelectableItem>();

        if (addItemList == null)
            addItemList = new List<string>();

        for (int i = 0; i < equidStore.selectables.Count; i++)
            weaphonInfoEx.Add(equidStore.selectables[i].state.name, equidStore.selectables[i]);

        if (staticResultUI == null)
            staticResultUI = this;

        if (tempWeaphonState == null)
        {
            tempWeaphonState = new GameObject();
            tempWeaphonState.name = "Equid State �ӽ� ��ü";

            EquidState state = tempWeaphonState.AddComponent<EquidState>();
        }

        GetItem("�ܰ�");

        //Invoke("GameIsEnd", 2.0f);
    }

    public void Update()
    {
        if (isPlayTimeReset)
            playTime += Time.deltaTime;

        if (InventroyPosition.isAddItemable && addItemList.Count != 0)
        {
            for (int i = 0; i < getItemList.Count; i++)
                AddItem(getItemList[i]);

            getItemList.Clear();
        }
    }

    // ������ ������ ��� ȣ��Ǵ� �޼ҵ�
    public void GameIsEnd()
    {
        // ��� UI�� Ȱ��ȭ��
        resultUI.SetActive(true);
        isPlayTimeReset = false;

        ShowItemList();

        StartCoroutine(ShowPlayTime()); // �÷��̾� �� �ð��� ������
        GetCurrentStage();              // ���� ���� ���������� ������

        // �÷��̾� ����, ���� ����ġ�� ���� ���� ������ �ۼ�Ʈ
        State playerState = GameManager.info.allPlayerState;
        float currentEXPPersent = (float)playerState.currentExp / ((level1PerEXPUp * (playerState.level - 1)) + startEXP);

        resultGoldText.text = getGold.ToString("#,##0");
        resultEXPText.text = getEXP.ToString("#,##0");
        currentPlayerLevelText.text = playerState.level.ToString("#,##0");
        currentEXPText.text = playerState.currentExp.ToString("#,##0");
        nextEXPText.text = ((level1PerEXPUp * (playerState.level - 1)) + startEXP).ToString("#,##0");
        currentEXPPersentSlider.value = currentEXPPersent;
        currentEXPPersentText.text = currentEXPPersent.ToString("#0.0%");
    }

    // Ž�� �� ����ġ�� ����� �� ȣ��Ǵ� �޼ҵ�
    public static void GetEXP(int exp)
    {
        getEXP += exp;
    }

    // Ž�� �� ��带 ����� �� ȣ��Ǵ� �޼ҵ�
    public static void GetGold(int coin)
    {
        getGold += coin;

        GameManager.info.playerState.money += coin;
    }

    public static void GetItem(string weaphonName)
    {
        Debug.Assert(getItemList != null, "���� �������� �����ϴ� ����Ʈ�� �ʱ�ȭ���� �ʾҽ��ϴ�.");

        getItemList.Add(weaphonName.Replace(" ", ""));

        if (!InventroyPosition.isAddItemable)
            addItemList.Add(weaphonName);

        else
            staticResultUI.AddItem(weaphonName);
    }

    private void AddItem(string name)
    {
        for (int i = 0; i < equidStore.selectables.Count; i++)
        {
            if (equidStore.selectables[i].state.name == name)
            {
                EquidState equidState = tempWeaphonState.GetComponent<EquidState>();
                equidState.state = equidStore.selectables[i].state;

                InventroyPosition.CallAddItem(name, equidStore.selectables[i].equipmentType, equidState);
                return;
            }
        }

        Debug.Assert(false, "�ش� �̸��� ���� �������� ����");
    }

    // ���� ���� �������� �޼ҵ�
    private void GetCurrentStage()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        // ���� � ������ ������
        if (currentSceneName.StartsWith("Stage"))
        {
            int currentLevel = int.Parse(currentSceneName[currentSceneName.Length - 1].ToString());
            currentLevelText.text = currentLevel + "���� ";

            // �� �� � ������ �ش��ϴ��� ������
            Transform stages = GameObject.Find(currentLevel + "Stages").transform;

            for (int i = 0; i < stages.childCount; i++)
            {
                if (stages.GetChild(i).gameObject.activeSelf)
                {
                    string stageName = stages.GetChild(i).name;

                    // �Ϲ� ���������� ���
                    if (stageName.StartsWith("stage") || stageName.StartsWith("Stage"))
                        currentLevelText.text += stageName[stageName.Length - 1].ToString() + "��������";

                    // ���� ���������� ���
                    else
                        currentLevelText.text += "������������";

                    break;
                }
            }
        }
    }

    // Ž���� �ð��� ó���ϴ� �޼ҵ�
    private IEnumerator ShowPlayTime()
    {
        float nowTime = Time.time;

        // 2�� ���� 0 ~ Ž��ð����� �þ
        while (Time.time - nowTime <= 2.0f)
        {
            float currentTime = Mathf.Lerp(0.0f, playTime, (Time.time - nowTime) / 2);

            // Ž���� �ð��� �ð�, ��, �ʷ� ����
            int hours = (int)(currentTime / 3600);
            int minutes = (int)((currentTime % 3600) / 60);
            int seconds = (int)(currentTime % 60);

            timeText.text = hours.ToString("#0") + "�ð� " + minutes.ToString("00") + "�� " + seconds.ToString("00") + "��";

            yield return null;
        }

        // ��� ���� ��Ȯ�ϰ� ó��
        int hour = (int)(playTime / 3600);
        int minute = (int)((playTime % 3600) / 60);
        int second = (int)(playTime % 60);

        timeText.text = hour.ToString("#0") + "�ð� " + minute.ToString("00") + "�� " + second.ToString("00") + "��";

        StartCoroutine(ResultEXP());
    }

    // ���� ����ġ�� ����ϴ� �޼ҵ�
    private IEnumerator ResultEXP()
    {
        // ���� �ð�, Ž���� ����ġ, Ž�� �� ���� ����ġ, ���� ������ �ʿ��� ����ġ�� ������
        float nowTime = Time.time;
        int initExp = int.Parse(currentEXPText.text.Replace(",", "")), addExp, nextExp = int.Parse(nextEXPText.text.Replace(",", ""));

        // 2�� ���� 0 ~ ���� ����ġ�� �ݿ���
        while (Time.time - nowTime <= 2.0f)
        {
            addExp = (int)Mathf.Lerp(0, getEXP, (Time.time - nowTime) / 2);
            int currentEXP = initExp + addExp;

            // ���� ����ġ�� ���� ������ �ʿ��� ����ġ�� �ѱ� ���
            if (currentEXP >= nextExp)
            {
                // ���� ���ϰ� ���� ������ �ʿ��� �� ��ŭ �ʱ� ����ġ�� ��
                GameManager.info.playerState.level++;
                currentEXP -= nextExp;
                initExp -= nextExp;

                // ���� ����ġ�� ���� ����ġ�� �ʿ��� �ؽ�Ʈ �ʱ�ȭ
                nextEXPText.text = ((level1PerEXPUp * (GameManager.info.playerState.level - 1)) + startEXP).ToString("#,##0");
                currentPlayerLevelText.text = GameManager.info.playerState.level.ToString("#,##0");
            }

            // �����̴��� ����� �ۼ�Ʈ�� ������
            float currentEXPPersent = (float)currentEXP / ((level1PerEXPUp * (GameManager.info.playerState.level - 1)) + startEXP);

            currentEXPPersentSlider.value = currentEXPPersent;
            currentEXPPersentText.text = currentEXPPersent.ToString("#0.0%");
            
            GameManager.info.playerState.currentExp = currentEXP;
            currentEXPText.text = currentEXP.ToString("#,##0");

            GameManager.info.UpdatePlayerState();

            yield return null;
        }

        // ���� ��� ���� �����
        float finalEXPPersent = (float)(initExp + getEXP) / ((level1PerEXPUp * (GameManager.info.playerState.level - 1)) + startEXP); 
        currentEXPPersentSlider.value = finalEXPPersent;
        currentEXPPersentText.text = finalEXPPersent.ToString("#0.0%");

        currentEXPText.text = (initExp + getEXP).ToString("#,##0");
        GameManager.info.playerState.currentExp = int.Parse(currentEXPText.text.Replace(",", ""));

        getEXP = 0;
    }

    private void ShowItemList()
    {
        for (int i = 0; i < getItemList.Count; i++)
        {
            // �ش� �̸� ������ �߰� ������ ������
            SelectableItem newItemInfoEx = weaphonInfoEx[getItemList[i]];

            // ������ �������� ���� �����ϰ� �߰� ����Ʈ �����Ӱ� �⺻ ����Ʈ ���� �����ϴ� ������Ʈ�� ������
            GameObject newItem = Instantiate(itemFrame, frameTarget);
            Transform addList = newItem.transform.GetChild(3);
            Transform baseListValue = newItem.transform.GetChild(4).GetChild(1);

            Image image = newItem.transform.GetChild(2).GetComponent<Image>();
            image.sprite = newItemInfoEx.sprite;

            State newItemState = (State)Resources.Load("Scriptable/Equid/" + getItemList[i]);
            Debug.Assert(newItemState != null, "\"" + getItemList[i] + "\"��� �ش� ����� �������� �ʽ��ϴ�.");

            newItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = newItemState.name;

            int addListCount = 0;

            foreach (string stateName in State.datas.Keys)
            {
                if (stateName == "NickName")
                    continue;

                object returnValue = State.datas[stateName].GetValue(newItemState);

                // �ش� �ɷ�ġ�� � Ÿ������ �˾Ƴ� �� �߰��� ���� ����
                Type type = returnValue.GetType();
                returnValue = Convert.ChangeType(returnValue, Type.GetTypeCode(type));

                // ���� 0�̸� �߰������� ����
                if (type == typeof(int) && (int)returnValue == 0)
                    continue;

                else if (type == typeof(float) && ((float)returnValue > -0.001f && (float)returnValue < 0.001f))
                    continue;

                // �ִ� �� �Ǵ� �� �⺻ ���� ���
                if (stateName == "MaxHP" || stateName == "Money")
                {
                    if (stateName == "MaxHP")
                    {
                        newItem.transform.GetChild(4).GetChild(0).GetChild(0).gameObject.SetActive(true);
                        baseListValue.GetChild(0).gameObject.SetActive(true);
                        baseListValue.GetChild(0).GetComponent<TextMeshProUGUI>().text = ((int)returnValue).ToString("#,##0");
                    }

                    else if (stateName == "Money")
                        baseListValue.GetChild(1).GetComponent<TextMeshProUGUI>().text = ((int)returnValue * 0.8).ToString("#,##0");

                    continue;
                }

                else
                {
                    // addList�� ó���� ��ġ�� ������
                    Transform currentAddText = addList.GetChild(addListCount);

                    // �ش� �߰� �ؽ�Ʈ�� Ŵ
                    currentAddText.gameObject.SetActive(true);

                    // �ش� �ؽ�Ʈ�� ���� ���
                    currentAddText.GetChild(1).GetComponent<TextMeshProUGUI>().text = stateName;
                    currentAddText.GetChild(2).GetComponent<TextMeshProUGUI>().text = (Convert.ToDouble(returnValue)).ToString("#,##0.###");
                    addListCount++;
                }
            }
        }
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
