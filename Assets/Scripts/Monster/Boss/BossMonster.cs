using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BossSetting{
    public GameObject range;
    public Vector3 pos;
    public MonsterState bossState;
    public int damage;
    public float disapearTime;
    public int dir;
    public float ainmterm;
}
public abstract class BossMonster : MonoBehaviour
{
    public BossSetting boss;
    // Start is called before the first frame update
    public abstract void InitSetting();
    public abstract void Attack1();
    public abstract void Attack2();
    public abstract void SpecialAttack1();
    public abstract void SpecialAttack2();
    
}
