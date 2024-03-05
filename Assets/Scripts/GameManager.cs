using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager info { get { return instance; } }

    private static GameManager instance;

    [SerializeField] public State playerState;  // 플레이어 기본 스탯 및 정보            참조법 (GameManager.info.playerState)
    public State addFoodState;                  // 음식 추가 능력치가 있을 경우 추가      참조법 (GameManager.info.addFoodState)
    public State addWaphonState;                // 무기 추가 능력치가 있을 경우 추가      참조법 (GameManager.info.addWaphonState)
    public State addStatState;                  // 스탯 추가 능력치가 있을 경우 추가      참조법 (GameManager.info.addStatState)
    public State allPlayerState;                // 총 플레이어 능력치                   참조법 (GameManager.info.allPlayerState)

    public State currentPlayerState { get { return allPlayerState; } }    // 총 플레이어 스탯을 반환하는 변수

    void Awake()
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

    // 모든 상태를 더하여 전체 상태를 업데이트하는 메소드
    private void UpdatePlayerState()
    {
        foreach (string state in State.datas.Keys)
        {
            if (state == "NickName")
                continue;

            // 해당 능력치의 타입
            Type type = State.datas[state].GetValue(addFoodState).GetType();

            // 모든 능력치를 가져옴
            double defualtState = Convert.ToDouble(State.datas[state].GetValue(playerState));
            double food = Convert.ToDouble(State.datas[state].GetValue(addFoodState));
            double waphon = Convert.ToDouble(State.datas[state].GetValue(addWaphonState));
            double stat = Convert.ToDouble(State.datas[state].GetValue(addStatState));

            // 모두 더함
            object returnValue = Convert.ChangeType(defualtState + food + waphon + stat, Type.GetTypeCode(type));

            // 합계를 저장함
            State.datas[state].SetValue(allPlayerState, returnValue);
        }
    }

    // 음식으로 인한 스탯 상승을 구하는 메소드
    public void AddFoodState(string stateName, double value)
    {
        // 그 상태가 존재하는지 찾아봄
        object fieldValue = null;
        try { fieldValue = State.datas[stateName].GetValue(addFoodState); }

        catch { return; }

        if (fieldValue == null)
        {
            Debug.Assert(false, "해당 이름의 상태 이름이 존재하지 않습니다.");
            return;
        }

        // 숫자로 바꿀 수 있는 경우 숫자로 바꾸고 값을 더함
        double resultValue = Convert.ToDouble(fieldValue) + value;
        
        // 해당 능력치가 어떤 타입인지 알아낸 후 추가할 값을 더함
        Type type = State.datas[stateName].GetValue(addFoodState).GetType();
        object returnValue = Convert.ChangeType(resultValue, Type.GetTypeCode(type));

        // 다시 해당 타입으로 변환되어 저장함
        State.datas[stateName].SetValue(addFoodState, returnValue);

        UpdatePlayerState();
    }

    // 무기로 인한 스탯 상승을 구하는 메소드
    public void AddWaphonState(string stateName, double value)
    {
        // 그 상태가 존재하는지 찾아봄
        object fieldValue = null;
        try { fieldValue = State.datas[stateName].GetValue(addWaphonState); }

        catch { return; }

        if (fieldValue == null)
        {
            Debug.Assert(false, "해당 이름의 상태 이름이 존재하지 않습니다.");
            return;
        }

        // 숫자로 바꿀 수 있는 경우 숫자로 바꾸고 값을 더함
        double resultValue = Convert.ToDouble(fieldValue) + value;

        // 해당 능력치가 어떤 타입인지 알아낸 후 추가할 값을 더함
        Type type = State.datas[stateName].GetValue(addWaphonState).GetType();
        object returnValue = Convert.ChangeType(resultValue, Type.GetTypeCode(type));

        // 다시 해당 타입으로 변환되어 저장함
        State.datas[stateName].SetValue(addWaphonState, returnValue);

        UpdatePlayerState();
    }

    // 능력치 업그레이드에 따른 스탯 상승을 구하는 메소드
    public void AddStatState(string stateName, double value)
    {
        // 그 상태가 존재하는지 찾아봄
        object fieldValue = null;
        try { fieldValue = State.datas[stateName].GetValue(addStatState); }

        catch { return; }

        if (fieldValue == null)
        {
            Debug.Assert(false, "해당 이름의 상태 이름이 존재하지 않습니다.");
            return;
        }

        // 숫자로 바꿀 수 있는 경우 숫자로 바꾸고 값을 더함
        double resultValue = Convert.ToDouble(fieldValue) + value;

        // 해당 능력치가 어떤 타입인지 알아낸 후 추가할 값을 더함
        Type type = State.datas[stateName].GetValue(addStatState).GetType();
        object returnValue = Convert.ChangeType(resultValue, Type.GetTypeCode(type));

        // 다시 해당 타입으로 변환되어 저장함
        State.datas[stateName].SetValue(addStatState, returnValue);

        UpdatePlayerState();
    }

    public void SetFoodState(string stateName, double value)
    {
        // 그 상태가 존재하는지 찾아봄
        object fieldValue = null;
        try { fieldValue = State.datas[stateName].GetValue(addFoodState); }

        catch { return; }

        if (fieldValue == null)
        {
            Debug.Assert(false, "해당 이름의 상태 이름이 존재하지 않습니다.");
            return;
        }

        // 해당 능력치가 어떤 타입인지 알아낸 후 추가할 값을 더함
        Type type = State.datas[stateName].GetValue(addFoodState).GetType();
        object returnValue = Convert.ChangeType(value, Type.GetTypeCode(type));

        // 다시 해당 타입으로 변환되어 저장함
        State.datas[stateName].SetValue(addFoodState, returnValue);

        UpdatePlayerState();
    }

    // 무기로 인한 스탯 값을 구하는 메소드
    public void SetWaphonState(string stateName, double value)
    {
        // 그 상태가 존재하는지 찾아봄
        object fieldValue = null;
        try { fieldValue = State.datas[stateName].GetValue(addWaphonState); }

        catch { return; }

        if (fieldValue == null)
        {
            Debug.Assert(false, "해당 이름의 상태 이름이 존재하지 않습니다.");
            return;
        }

        // 해당 능력치가 어떤 타입인지 알아낸 후 추가할 값을 더함
        Type type = State.datas[stateName].GetValue(addWaphonState).GetType();
        object returnValue = Convert.ChangeType(value, Type.GetTypeCode(type));

        // 다시 해당 타입으로 변환되어 저장함
        State.datas[stateName].SetValue(addWaphonState, returnValue);

        UpdatePlayerState();
    }

    // 능력치 업그레이드에 따른 스탯 상승을 구하는 메소드
    public void SetStatState(string stateName, double value)
    {
        // 그 상태가 존재하는지 찾아봄
        object fieldValue = null;
        try { fieldValue = State.datas[stateName].GetValue(addStatState); }

        catch { return; }

        if (fieldValue == null)
        {
            Debug.Assert(false, "해당 이름의 상태 이름이 존재하지 않습니다.");
            return;
        }

        // 해당 능력치가 어떤 타입인지 알아낸 후 추가할 값을 더함
        Type type = State.datas[stateName].GetValue(addStatState).GetType();
        object returnValue = Convert.ChangeType(value, Type.GetTypeCode(type));

        // 다시 해당 타입으로 변환되어 저장함
        State.datas[stateName].SetValue(addStatState, returnValue);

        UpdatePlayerState();
    }
}