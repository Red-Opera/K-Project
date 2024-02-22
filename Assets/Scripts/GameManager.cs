using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager info { get { return instance; } }

    private static GameManager instance;

    [SerializeField] public State playerState;  // �÷��̾� �⺻ ���� �� ����
    public State addFoodState;                  // ���� �߰� �ɷ�ġ�� ���� ��� �߰�
    public State addWaphonState;                // ���� �߰� �ɷ�ġ�� ���� ��� �߰�
    public State addStatState;                  // ���� �߰� �ɷ�ġ�� ���� ��� �߰�

    public State currentPlayerState { get { return playerState; } }    // �� �÷��̾� ������ ��ȯ�ϴ� ����

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
    }

    void Update()
    {
        
    }

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

        // ���� ����
        try 
        {
            // ���ڷ� �ٲ� �� �ִ� ��� ���ڷ� �ٲٰ� ���� ����
            double resultValue = (double)fieldValue + value;

            // �ٽ� �ش� Ÿ������ ��ȯ�Ǿ� ������
            State.datas[stateName].SetValue(addFoodState, resultValue);
        }

        catch 
        {
            Debug.Assert(false, "�ش� ���� ���� �� �����ϴ�.");
            return;
        }
    }
}
