using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager info { get { return instance; } }

    private static GameManager instance;

    [SerializeField] public State playerState;      // 플레이어 기본 스탯 및 정보            참조법 (GameManager.info.playerState)
    [HideInInspector] public State addFoodState;    // 음식 추가 능력치가 있을 경우 추가      참조법 (GameManager.info.addFoodState)
    [HideInInspector] public State addWaphonState;  // 무기 추가 능력치가 있을 경우 추가      참조법 (GameManager.info.addWaphonState)
    [HideInInspector] public State addStatState;    // 스탯 추가 능력치가 있을 경우 추가      참조법 (GameManager.info.addStatState)
    [HideInInspector] public State addLevelState;   // 레벨 추가 능력치가 있을 경우 추가      참조법 (GameManager.info.addLevelState)
    [HideInInspector] public State allPlayerState;  // 총 플레이어 능력치                   참조법 (GameManager.info.allPlayerState)

    public AbilityTempState abilityState;                              // 현재 어빌리티 값 접근을 위해 사용
    public State currentPlayerState { get { return allPlayerState; } }  // 총 플레이어 스탯을 반환하는 변수

    private HpLevelManager hpLevelManager;      // 플레이어 체력바 컨포넌트

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        else
            Destroy(this);

        addFoodState = ScriptableObject.CreateInstance<State>();
        addWaphonState = ScriptableObject.CreateInstance<State>();
        addStatState = ScriptableObject.CreateInstance<State>();
        allPlayerState = ScriptableObject.CreateInstance<State>();

        UpdatePlayerState();
    }

    private void Update()
    {
        AudioListener.volume = Mathf.Min(AudioListener.volume, 0.35f);
    }

    // 음식으로 인한 스탯 상승을 구하는 메소드
    public void AddFoodState(string stateName, double value)
    {
        AddState(stateName, value, addFoodState);
    }

    // 무기로 인한 스탯 상승을 구하는 메소드
    public void AddWaphonState(string stateName, double value)
    {
        AddState(stateName, value, addWaphonState);
    }

    // 능력치 업그레이드에 따른 스탯 상승을 구하는 메소드
    public void AddStatState(string stateName, double value)
    {
        AddState(stateName, value, addStatState);
    }

    // 음식으로 인한 스탯 값을 구하는 메소드
    public void SetFoodState(string stateName, double value)
    {
        SetState(stateName, value, addFoodState);
    }

    // 무기로 인한 스탯 값을 구하는 메소드
    public void SetWaphonState(string stateName, double value)
    {
        SetState(stateName, value, addWaphonState);
    }

    // 능력치 업그레이드에 따른 스탯 상승을 구하는 메소드
    public void SetStatState(string stateName, double value)
    {
        SetState(stateName, value, addStatState);
    }

    private void AddState(string stateName, double value, State state)
    {
        // 그 상태가 존재하는지 찾아봄
        object fieldValue = null;
        try { fieldValue = State.datas[stateName].GetValue(state); }

        catch { return; }

        if (fieldValue == null)
        {
            Debug.Assert(false, "해당 이름의 상태 이름이 존재하지 않습니다.");
            return;
        }

        // 숫자로 바꿀 수 있는 경우 숫자로 바꾸고 값을 더함
        double resultValue = Convert.ToDouble(fieldValue) + value;

        // 해당 능력치가 어떤 타입인지 알아낸 후 추가할 값을 더함
        Type type = State.datas[stateName].GetValue(state).GetType();
        object returnValue = Convert.ChangeType(resultValue, Type.GetTypeCode(type));

        // 다시 해당 타입으로 변환되어 저장함
        State.datas[stateName].SetValue(state, returnValue);

        UpdatePlayerState();
    }

    private void SetState(string stateName, double value, State state)
    {
        // 그 상태가 존재하는지 찾아봄
        object fieldValue = null;
        try { fieldValue = State.datas[stateName].GetValue(state); }

        catch { return; }

        if (fieldValue == null)
        {
            Debug.Assert(false, "해당 이름의 상태 이름이 존재하지 않습니다.");
            return;
        }

        // 해당 능력치가 어떤 타입인지 알아낸 후 추가할 값을 더함
        Type type = State.datas[stateName].GetValue(state).GetType();
        object returnValue = Convert.ChangeType(value, Type.GetTypeCode(type));

        // 다시 해당 타입으로 변환되어 저장함
        State.datas[stateName].SetValue(state, returnValue);

        UpdatePlayerState();
    }

    public static void AddStates(State currentState, State otherState)
    {
        FieldInfo[] allFields = typeof(State).GetFields(BindingFlags.Public | BindingFlags.Instance);

        foreach (FieldInfo field in allFields)
        {
            if (field.Name == "nickName" || field.Name == "money")
                continue;

            object currentValue = field.GetValue(currentState);
            object otherValue = field.GetValue(otherState);

            if (currentValue is int)
                field.SetValue(currentState, Convert.ToInt32(currentValue) + Convert.ToInt32(otherValue));

            else if (currentValue is float)
                field.SetValue(currentState, Convert.ToSingle(currentValue) + Convert.ToSingle(otherValue));
        }
    }

    // 모든 상태를 더하여 전체 상태를 업데이트하는 메소드
    public void UpdatePlayerState()
    {
        foreach (string state in State.datas.Keys)
        {
            if (state == "NickName" || state == "CurrentHp")
                continue;

            // 해당 능력치의 타입
            Type type = State.datas[state].GetValue(addFoodState).GetType();

            // 모든 능력치를 가져옴
            double defualtState = Convert.ToDouble(State.datas[state].GetValue(playerState));
            double food = Convert.ToDouble(State.datas[state].GetValue(addFoodState));
            double waphon = Convert.ToDouble(State.datas[state].GetValue(addWaphonState));
            double stat = Convert.ToDouble(State.datas[state].GetValue(addStatState));
            double level = Convert.ToDouble(State.datas[state].GetValue(addLevelState)) * (playerState.level - 1);

            // 모두 더함
            object returnValue = Convert.ChangeType(defualtState + food + waphon + stat + level, Type.GetTypeCode(type));

            // 합계를 저장함
            State.datas[state].SetValue(allPlayerState, returnValue);
        }

        HpManager();
    }

    // allPlayerState의 체력을 관리하는 메소드
    private void HpManager()
    {
        TownCurrentHPMax();

        // 현재 체력이 최대 체력보다 높을 경우 최대 체력으로 설정
        if (allPlayerState.currentHp > allPlayerState.maxHP)
            allPlayerState.currentHp = allPlayerState.maxHP;

        if (hpLevelManager == null)
            hpLevelManager = UIOpen.ui.hpLevelBar.GetComponent<HpLevelManager>();

        hpLevelManager.UpdatePlayerHP();
    }

    // 마을에서의 체력을 최대로 만드는 메소드
    private void TownCurrentHPMax()
    {
        string mapName = SceneManager.GetActiveScene().name;

        if (mapName == "Map")
          allPlayerState.currentHp = allPlayerState.maxHP;
    }
}