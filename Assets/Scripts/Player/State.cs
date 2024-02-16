using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Scriptable/Player", order = int.MaxValue)]
public class State : ScriptableObject
{
    public string nickName;             // 플레이어 닉네임
    public int level = 0;               // 레벨
    public int maxHP = 0;               // 최대 체력
    public int currentHp = 0;           // 현재 체력

    public int damage = 0;              // 데미지
    public int defense = 0;             // 방어력
    public float critical = 0;          // 크리티컬 확률
    public float moveSpeed = 0;         // 이동 속도
    public float jumpPower = 0;         // 점프력
    public int jumpCount = 0;           // 점프 횟수
    public int maxJump = 0;             // 최대 점프 회수
    public float dashDamage = 0;        // 대쉬 데미지
    public int strong = 0;              // 강인함
    public float criticalDamage = 0;    // 크리티컬 데미지
    public float attackSpeed = 0;       // 공격 속도
    public int fixedDamage = 0;         // 추가 고정 데미지
    public float defensePersent = 0;    // 방어 확률
    public float avoidPersent = 0;      // 회피 확률
    public float reloadTime = 0;        // 재장전 시간
    public static float satiety = 0;

    public int dashBarCount = 0;        // 대쉬 바 개수
    public int food = 0;                // 허기 상태
    public int money = 0;               // 플레이어 돈

    public static Dictionary<string, FieldInfo> datas = new Dictionary<string, FieldInfo>();

    public void Awake()
    {
        // 모든 멤버 변수를 가져옴
        FieldInfo[] allField = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

        foreach (FieldInfo field in allField)
        {
            // 필드 이름을 가져와 대문자로 바꿈
            string fieldName = field.Name;
            fieldName = char.ToUpper(fieldName[0]) + fieldName.Substring(1);

            // 이미 데이터가 존재하는지 확인
            if (datas.ContainsKey(fieldName))
                continue;

            datas.Add(fieldName, field);
        }
    }
}