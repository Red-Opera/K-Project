using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Scriptable/Player", order = int.MaxValue)]
public class State : ScriptableObject
{
    public string nickName;
    public int level;
    public int maxHP;
    public int currentHp;

    public int damage;
    public int defense;
    public float critical;
    public float moveSpeed;
    public float jumpPower;
    public int jumpCount;
    public int maxJump;
    public float dashDamage;
    public int strong;
    public float criticalDamage;
    public float attackSpeed;
    public int fixedDamage;
    public float defensePersent;
    public float avoidPersent;
    public float reloadTime;
    public static float satiety;

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