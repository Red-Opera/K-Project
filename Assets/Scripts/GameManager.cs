using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager info { get { return instance; } }

    private static GameManager instance;

    [SerializeField] public State playerState;  // 플레이어 기본 스탯 및 정보
    public State addFoodState;                  // 음식 추가 능력치가 있을 경우 추가
    public State addWaphonState;                // 무기 추가 능력치가 있을 경우 추가
    public State addStatState;                  // 스탯 추가 능력치가 있을 경우 추가

    public State currentPlayerState { get { return playerState; } }    // 총 플레이어 스탯을 반환하는 변수

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
        // 그 상태가 존재하는지 찾아봄
        object fieldValue = null;
        try { fieldValue = State.datas[stateName].GetValue(addFoodState); }

        catch { return; }

        if (fieldValue == null)
        {
            Debug.Assert(false, "해당 이름의 상태 이름이 존재하지 않습니다.");
            return;
        }

        // 값을 더함
        try 
        {
            // 숫자로 바꿀 수 있는 경우 숫자로 바꾸고 값을 더함
            double resultValue = (double)fieldValue + value;

            // 다시 해당 타입으로 변환되어 저장함
            State.datas[stateName].SetValue(addFoodState, resultValue);
        }

        catch 
        {
            Debug.Assert(false, "해당 값은 더할 수 없습니다.");
            return;
        }
    }
}
