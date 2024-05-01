using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Monster", menuName ="Scriptable/Monster",  order =int.MaxValue)]
public class MonsterState : ScriptableObject
{
    public int level;
    public int maxHP;
    public int currentHp;
    public int Stage;

    public int damage;
    public int defense;
    public int moveSpeed;
    public float dashcoaf;
    public float jumpPower;
    public int flyingSpeed;
    public int Money;
}
