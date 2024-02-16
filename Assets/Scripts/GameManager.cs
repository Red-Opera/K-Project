using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager info { get { return instance; } }

    private static GameManager instance;

    [SerializeField] public State playerState;  // �÷��̾� �⺻ ���� �� ����
    public State addState;                      // �߰� �ɷ�ġ�� ���� ��� �߰�

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
