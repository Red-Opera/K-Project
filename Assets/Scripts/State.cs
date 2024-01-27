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
    public float satiety;

    public Dictionary<string, FieldInfo> datas;

    public void Awake()
    {
        FieldInfo[] allField = GetType().GetFields(BindingFlags.Public);

        foreach (FieldInfo field in allField)
            datas.Add(nameof(field.Name), field);
    }
}