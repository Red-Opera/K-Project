using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WareWolves : BossMonster
{
    Animator anim;
    public override void InitSetting()
    {
        boss.range = Resources.Load<GameObject>("Prefab/Boss/AttackRange");
        boss.bossState = Resources.Load<MonsterState>("Scriptable/Boss/WareWolf");
        boss.pos = new Vector3(2, 0, 0);
        boss.damage = 10;
        boss.disapearTime = 0.3f;
        anim = GetComponent<Animator>();
    }
    public override void Attack1()
    {
        boss.ainmterm = 0;
        GameObject Atk = Instantiate(boss.range, transform.position + boss.pos*boss.dir, Quaternion.identity);
        BossAttack atkSc = Atk.GetComponent<BossAttack>();
        atkSc.setState(boss); 
        anim.SetTrigger("attack1");
    }

    public override void Attack2()
    {
        //DashAttack
        boss.ainmterm = 0;
        boss.bossState.dashcoaf =2;
        Invoke("ResetSpeed", 0.5f);
        GameObject Atk = Instantiate(boss.range, transform.position + boss.pos*boss.dir, Quaternion.identity);
        BossAttack atkSc = Atk.GetComponent<BossAttack>();
        atkSc.setState(boss);
        anim.SetTrigger("attack2");
    }

    public override void SpecialAttack1()
    {
        //jump Attack
        boss.ainmterm = 0.1f;
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.up * boss.bossState.jumpPower;
        anim.SetTrigger("jump");
        Invoke("Jump", boss.ainmterm);
    }

    public override void SpecialAttack2()
    {
        Debug.Log("it is not use");
    }
    void Jump(){
        anim.SetBool("isJump",true);
    }
    void ResetSpeed(){
        boss.bossState.dashcoaf =1;
    }
}
