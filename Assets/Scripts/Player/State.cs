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
}