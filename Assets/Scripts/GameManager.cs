using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager info { get { return instance; } }

    private static GameManager instance;

    [SerializeField] public State playerState;  // �÷��̾� �⺻ ���� �� ����            ������ (GameManager.info.playerState)
    public State addFoodState;                  // ���� �߰� �ɷ�ġ�� ���� ��� �߰�      ������ (GameManager.info.addFoodState)
    public State addWaphonState;                // ���� �߰� �ɷ�ġ�� ���� ��� �߰�      ������ (GameManager.info.addWaphonState)
    public State addStatState;                  // ���� �߰� �ɷ�ġ�� ���� ��� �߰�      ������ (GameManager.info.addStatState)
    public State allPlayerState;                // �� �÷��̾� �ɷ�ġ                   ������ (GameManager.info.allPlayerState)

    public State currentPlayerState { get { return allPlayerState; } }    // �� �÷��̾� ������ ��ȯ�ϴ� ����

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

    // ��� ���¸� ���Ͽ� ��ü ���¸� ������Ʈ�ϴ� �޼ҵ�
    private void UpdatePlayerState()
    {
        foreach (string state in State.datas.Keys)
        {
            if (state == "NickName")
                continue;

            // �ش� �ɷ�ġ�� Ÿ��
            Type type = State.datas[state].GetValue(addFoodState).GetType();

            // ��� �ɷ�ġ�� ������
            double defualtState = Convert.ToDouble(State.datas[state].GetValue(playerState));
            double food = Convert.ToDouble(State.datas[state].GetValue(addFoodState));
            double waphon = Convert.ToDouble(State.datas[state].GetValue(addWaphonState));
            double stat = Convert.ToDouble(State.datas[state].GetValue(addStatState));

            // ��� ����
            object returnValue = Convert.ChangeType(defualtState + food + waphon + stat, Type.GetTypeCode(type));

            // �հ踦 ������
            State.datas[state].SetValue(allPlayerState, returnValue);
        }
    }

    // �������� ���� ���� ����� ���ϴ� �޼ҵ�
    public void AddFoodState(string stateName, double value)
    {
        // �� ���°� �����ϴ��� ã�ƺ�
        object fieldValue = null;
        try { fieldValue = State.datas[stateName].GetValue(addFoodState); }

        catch { return; }

        if (fieldValue == null)
        {
            Debug.Assert(false, "�ش� �̸��� ���� �̸��� �������� �ʽ��ϴ�.");
            return;
        }

        // ���ڷ� �ٲ� �� �ִ� ��� ���ڷ� �ٲٰ� ���� ����
        double resultValue = Convert.ToDouble(fieldValue) + value;
        
        // �ش� �ɷ�ġ�� � Ÿ������ �˾Ƴ� �� �߰��� ���� ����
        Type type = State.datas[stateName].GetValue(addFoodState).GetType();
        object returnValue = Convert.ChangeType(resultValue, Type.GetTypeCode(type));

        // �ٽ� �ش� Ÿ������ ��ȯ�Ǿ� ������
        State.datas[stateName].SetValue(addFoodState, returnValue);

        UpdatePlayerState();
    }

    // ����� ���� ���� ����� ���ϴ� �޼ҵ�
    public void AddWaphonState(string stateName, double value)
    {
        // �� ���°� �����ϴ��� ã�ƺ�
        object fieldValue = null;
        try { fieldValue = State.datas[stateName].GetValue(addWaphonState); }

        catch { return; }

        if (fieldValue == null)
        {
            Debug.Assert(false, "�ش� �̸��� ���� �̸��� �������� �ʽ��ϴ�.");
            return;
        }

        // ���ڷ� �ٲ� �� �ִ� ��� ���ڷ� �ٲٰ� ���� ����
        double resultValue = Convert.ToDouble(fieldValue) + value;

        // �ش� �ɷ�ġ�� � Ÿ������ �˾Ƴ� �� �߰��� ���� ����
        Type type = State.datas[stateName].GetValue(addWaphonState).GetType();
        object returnValue = Convert.ChangeType(resultValue, Type.GetTypeCode(type));

        // �ٽ� �ش� Ÿ������ ��ȯ�Ǿ� ������
        State.datas[stateName].SetValue(addWaphonState, returnValue);

        UpdatePlayerState();
    }

    // �ɷ�ġ ���׷��̵忡 ���� ���� ����� ���ϴ� �޼ҵ�
    public void AddStatState(string stateName, double value)
    {
        // �� ���°� �����ϴ��� ã�ƺ�
        object fieldValue = null;
        try { fieldValue = State.datas[stateName].GetValue(addStatState); }

        catch { return; }

        if (fieldValue == null)
        {
            Debug.Assert(false, "�ش� �̸��� ���� �̸��� �������� �ʽ��ϴ�.");
            return;
        }

        // ���ڷ� �ٲ� �� �ִ� ��� ���ڷ� �ٲٰ� ���� ����
        double resultValue = Convert.ToDouble(fieldValue) + value;

        // �ش� �ɷ�ġ�� � Ÿ������ �˾Ƴ� �� �߰��� ���� ����
        Type type = State.datas[stateName].GetValue(addStatState).GetType();
        object returnValue = Convert.ChangeType(resultValue, Type.GetTypeCode(type));

        // �ٽ� �ش� Ÿ������ ��ȯ�Ǿ� ������
        State.datas[stateName].SetValue(addStatState, returnValue);

        UpdatePlayerState();
    }

    public void SetFoodState(string stateName, double value)
    {
        // �� ���°� �����ϴ��� ã�ƺ�
        object fieldValue = null;
        try { fieldValue = State.datas[stateName].GetValue(addFoodState); }

        catch { return; }

        if (fieldValue == null)
        {
            Debug.Assert(false, "�ش� �̸��� ���� �̸��� �������� �ʽ��ϴ�.");
            return;
        }

        // �ش� �ɷ�ġ�� � Ÿ������ �˾Ƴ� �� �߰��� ���� ����
        Type type = State.datas[stateName].GetValue(addFoodState).GetType();
        object returnValue = Convert.ChangeType(value, Type.GetTypeCode(type));

        // �ٽ� �ش� Ÿ������ ��ȯ�Ǿ� ������
        State.datas[stateName].SetValue(addFoodState, returnValue);

        UpdatePlayerState();
    }

    // ����� ���� ���� ���� ���ϴ� �޼ҵ�
    public void SetWaphonState(string stateName, double value)
    {
        // �� ���°� �����ϴ��� ã�ƺ�
        object fieldValue = null;
        try { fieldValue = State.datas[stateName].GetValue(addWaphonState); }

        catch { return; }

        if (fieldValue == null)
        {
            Debug.Assert(false, "�ش� �̸��� ���� �̸��� �������� �ʽ��ϴ�.");
            return;
        }

        // �ش� �ɷ�ġ�� � Ÿ������ �˾Ƴ� �� �߰��� ���� ����
        Type type = State.datas[stateName].GetValue(addWaphonState).GetType();
        object returnValue = Convert.ChangeType(value, Type.GetTypeCode(type));

        // �ٽ� �ش� Ÿ������ ��ȯ�Ǿ� ������
        State.datas[stateName].SetValue(addWaphonState, returnValue);

        UpdatePlayerState();
    }

    // �ɷ�ġ ���׷��̵忡 ���� ���� ����� ���ϴ� �޼ҵ�
    public void SetStatState(string stateName, double value)
    {
        // �� ���°� �����ϴ��� ã�ƺ�
        object fieldValue = null;
        try { fieldValue = State.datas[stateName].GetValue(addStatState); }

        catch { return; }

        if (fieldValue == null)
        {
            Debug.Assert(false, "�ش� �̸��� ���� �̸��� �������� �ʽ��ϴ�.");
            return;
        }

        // �ش� �ɷ�ġ�� � Ÿ������ �˾Ƴ� �� �߰��� ���� ����
        Type type = State.datas[stateName].GetValue(addStatState).GetType();
        object returnValue = Convert.ChangeType(value, Type.GetTypeCode(type));

        // �ٽ� �ش� Ÿ������ ��ȯ�Ǿ� ������
        State.datas[stateName].SetValue(addStatState, returnValue);

        UpdatePlayerState();
    }
}