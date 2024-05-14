using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    [SerializeField] private EquidStore equidStore;                             // 무기 상점 UI 스크립트
    [HideInInspector] public Dictionary<string, SelectableItem> weaphonInfoEx;  // 무기 추가 정보

    [SerializeField] private GameObject resultUI;                       // 결과 UI 오브젝트
    [SerializeField] private TextMeshProUGUI timeText;                  // 탐험 시간 출력 텍스트
    [SerializeField] private TextMeshProUGUI currentLevelText;          // 현재 스테이지 텍스트
    [SerializeField] private TextMeshProUGUI resultGoldText;            // 얻은 골드 텍스트
    [SerializeField] private TextMeshProUGUI resultEXPText;             // 얻은 경험치 텍스트
    [SerializeField] private TextMeshProUGUI currentPlayerLevelText;    // 플레이어 레벨 텍스트
    [SerializeField] private TextMeshProUGUI currentEXPText;            // 현재 경험치 텍스트
    [SerializeField] private TextMeshProUGUI nextEXPText;               // 다음 레벨에 필요한 텍스트
    [SerializeField] private TextMeshProUGUI currentEXPPersentText;     // 현재 경험치에 따른 다름 레벨 텍스트
    [SerializeField] private Slider currentEXPPersentSlider;            // 다음 레벨에 따른 현재 경험치 퍼센트
    [SerializeField] private GameObject itemFrame;                      // 획득 아이템 프레임
    [SerializeField] private Transform frameTarget;                     // 획득 아이템 프레임이 배치할 위치

    [SerializeField] private int startEXP;          // 2레벨로 가기위한 경험치
    [SerializeField] private int level1PerEXPUp;    // 1레벨 올라갈때 마다 필요 경험치 증가량

    private GameObject tempWeaphonState;            // 무기 상태 임시 객체

    private static List<string> addItemList;        // 활성화시 추가될 아이템 리스트
    private static List<string> getItemList;        // 탐험 통안 획득한 아이템
    private static ResultUI staticResultUI;         // Result UI static 버전
    private static float playTime = 0.0f;           // 탐험 시간
    private static float getGold = 0;               // 탐험시 얻은 골드
    private static float getEXP = 0;                // 탐험시 얻은 경험치


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
            tempWeaphonState.name = "Equid State 임시 객체";

            EquidState state = tempWeaphonState.AddComponent<EquidState>();
        }

        GetItem("단검");

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

    // 게임이 끝났을 경우 호출되는 메소드
    public void GameIsEnd()
    {
        // 결과 UI를 활성화함
        resultUI.SetActive(true);
        isPlayTimeReset = false;

        ShowItemList();

        StartCoroutine(ShowPlayTime()); // 플레이어 한 시간을 가져옴
        GetCurrentStage();              // 현재 씬과 스테이지를 가져옴

        // 플레이어 상태, 현재 경험치에 따른 다음 레벨의 퍼센트
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

    // 탐험 중 경험치를 얻었을 때 호출되는 메소드
    public static void GetEXP(int exp)
    {
        getEXP += exp;
    }

    // 탐험 중 골드를 얻었을 때 호출되는 메소드
    public static void GetGold(int coin)
    {
        getGold += coin;

        GameManager.info.playerState.money += coin;
    }

    public static void GetItem(string weaphonName)
    {
        Debug.Assert(getItemList != null, "얻은 아이템을 저장하는 리스트가 초기화되지 않았습니다.");

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

        Debug.Assert(false, "해당 이름의 장비는 존재하지 않음");
    }

    // 현재 씬을 가져오는 메소드
    private void GetCurrentStage()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        // 현재 어떤 씬인지 가져옴
        if (currentSceneName.StartsWith("Stage"))
        {
            int currentLevel = int.Parse(currentSceneName[currentSceneName.Length - 1].ToString());
            currentLevelText.text = currentLevel + "레벨 ";

            // 씬 중 어떤 레벨에 해당하는지 가져옴
            Transform stages = GameObject.Find(currentLevel + "Stages").transform;

            for (int i = 0; i < stages.childCount; i++)
            {
                if (stages.GetChild(i).gameObject.activeSelf)
                {
                    string stageName = stages.GetChild(i).name;

                    // 일반 스테이지인 경우
                    if (stageName.StartsWith("stage") || stageName.StartsWith("Stage"))
                        currentLevelText.text += stageName[stageName.Length - 1].ToString() + "스테이지";

                    // 보스 스테이지인 경우
                    else
                        currentLevelText.text += "보스스테이지";

                    break;
                }
            }
        }
    }

    // 탐험한 시간을 처리하는 메소드
    private IEnumerator ShowPlayTime()
    {
        float nowTime = Time.time;

        // 2초 동안 0 ~ 탐험시간까지 늘어남
        while (Time.time - nowTime <= 2.0f)
        {
            float currentTime = Mathf.Lerp(0.0f, playTime, (Time.time - nowTime) / 2);

            // 탐험한 시간을 시간, 분, 초로 나눔
            int hours = (int)(currentTime / 3600);
            int minutes = (int)((currentTime % 3600) / 60);
            int seconds = (int)(currentTime % 60);

            timeText.text = hours.ToString("#0") + "시간 " + minutes.ToString("00") + "분 " + seconds.ToString("00") + "초";

            yield return null;
        }

        // 결과 값을 정확하게 처리
        int hour = (int)(playTime / 3600);
        int minute = (int)((playTime % 3600) / 60);
        int second = (int)(playTime % 60);

        timeText.text = hour.ToString("#0") + "시간 " + minute.ToString("00") + "분 " + second.ToString("00") + "초";

        StartCoroutine(ResultEXP());
    }

    // 얻은 경험치를 출력하는 메소드
    private IEnumerator ResultEXP()
    {
        // 현재 시간, 탐험전 경험치, 탐험 후 얻은 경험치, 다음 레벨에 필요한 경험치를 가져옴
        float nowTime = Time.time;
        int initExp = int.Parse(currentEXPText.text.Replace(",", "")), addExp, nextExp = int.Parse(nextEXPText.text.Replace(",", ""));

        // 2초 동안 0 ~ 얻은 경험치를 반영함
        while (Time.time - nowTime <= 2.0f)
        {
            addExp = (int)Mathf.Lerp(0, getEXP, (Time.time - nowTime) / 2);
            int currentEXP = initExp + addExp;

            // 현재 경험치가 다음 레벨에 필요한 경험치를 넘긴 경우
            if (currentEXP >= nextExp)
            {
                // 레벨 업하고 다음 레벨에 필요한 값 만큼 초기 경험치를 뺌
                GameManager.info.playerState.level++;
                currentEXP -= nextExp;
                initExp -= nextExp;

                // 현재 경험치와 다음 경험치에 필요한 텍스트 초기화
                nextEXPText.text = ((level1PerEXPUp * (GameManager.info.playerState.level - 1)) + startEXP).ToString("#,##0");
                currentPlayerLevelText.text = GameManager.info.playerState.level.ToString("#,##0");
            }

            // 슬라이더에 출력할 퍼센트를 가져옴
            float currentEXPPersent = (float)currentEXP / ((level1PerEXPUp * (GameManager.info.playerState.level - 1)) + startEXP);

            currentEXPPersentSlider.value = currentEXPPersent;
            currentEXPPersentText.text = currentEXPPersent.ToString("#0.0%");
            
            GameManager.info.playerState.currentExp = currentEXP;
            currentEXPText.text = currentEXP.ToString("#,##0");

            GameManager.info.UpdatePlayerState();

            yield return null;
        }

        // 최종 결과 값을 출력함
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
            // 해당 이름 무기의 추가 정보를 가져옴
            SelectableItem newItemInfoEx = weaphonInfoEx[getItemList[i]];

            // 아이템 프레임을 새로 생성하고 추가 리스트 프레임과 기본 리스트 값을 저장하는 오브젝트를 가져옴
            GameObject newItem = Instantiate(itemFrame, frameTarget);
            Transform addList = newItem.transform.GetChild(3);
            Transform baseListValue = newItem.transform.GetChild(4).GetChild(1);

            Image image = newItem.transform.GetChild(2).GetComponent<Image>();
            image.sprite = newItemInfoEx.sprite;

            State newItemState = (State)Resources.Load("Scriptable/Equid/" + getItemList[i]);
            Debug.Assert(newItemState != null, "\"" + getItemList[i] + "\"라는 해당 무기는 존재하지 않습니다.");

            newItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = newItemState.name;

            int addListCount = 0;

            foreach (string stateName in State.datas.Keys)
            {
                if (stateName == "NickName")
                    continue;

                object returnValue = State.datas[stateName].GetValue(newItemState);

                // 해당 능력치가 어떤 타입인지 알아낸 후 추가할 값을 더함
                Type type = returnValue.GetType();
                returnValue = Convert.ChangeType(returnValue, Type.GetTypeCode(type));

                // 값이 0이면 추가해주지 않음
                if (type == typeof(int) && (int)returnValue == 0)
                    continue;

                else if (type == typeof(float) && ((float)returnValue > -0.001f && (float)returnValue < 0.001f))
                    continue;

                // 최대 값 또는 돈 기본 값인 경우
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
                    // addList의 처리할 위치를 가져옴
                    Transform currentAddText = addList.GetChild(addListCount);

                    // 해당 추가 텍스트를 킴
                    currentAddText.gameObject.SetActive(true);

                    // 해당 텍스트로 정보 출력
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
