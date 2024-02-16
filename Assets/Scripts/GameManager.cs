using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager info { get { return instance; } }

    private static GameManager instance;

    [SerializeField] public State playerState;  // 플레이어 기본 스탯 및 정보
    public State addState;                      // 추가 능력치가 있을 경우 추가

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        else
            Destroy(this);
    }

    void Update()
    {
        
    }
}
