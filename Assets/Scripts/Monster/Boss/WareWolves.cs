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
        boss.damage = 1000;
        boss.disapearTime = 0.8f;
        boss.attackCount = 3;
        anim = GetComponent<Animator>();
    }
    public override void Attack1()
    {
        boss.pos = new Vector3(1.5f,-0.6f,0);
        boss.range.transform.localScale = new Vector3(2,2,1);
        GameObject Atk = Instantiate(boss.range, transform.position + boss.pos*boss.dir, Quaternion.identity);
        BossAttack atkSc = Atk.GetComponent<BossAttack>();
        atkSc.setState(boss); 
        StartCoroutine(DelayedTrigger("attack1", 0.5f));
    }

    public override void Attack2()
    {
        //DashAttack
        boss.bossState.dashcoaf =2;
        Invoke("ResetSpeed", 0.5f);
        boss.pos = new Vector3(2,-0.5f,0);
        boss.range.transform.localScale = new Vector3(3,2.2f,1);
        GameObject Atk = Instantiate(boss.range, transform.position + boss.pos*boss.dir, Quaternion.identity);
        BossAttack atkSc = Atk.GetComponent<BossAttack>();
        atkSc.setState(boss);
        StartCoroutine(DelayedTrigger("attack2", 0.5f));
    }

    public override void SpecialAttack1()
    {
        //jump Attack
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        boss.pos = new Vector3(0,-1.5f,0);
        boss.range.transform.localScale = new Vector3(14,1,1);
        GameObject Atk = Instantiate(boss.range, transform.position + boss.pos*boss.dir, Quaternion.identity);
        BossAttack atkSc = Atk.GetComponent<BossAttack>();
        atkSc.setState(boss);
        rigid.velocity = Vector2.up * boss.bossState.jumpPower;
        StartCoroutine(DelayedTrigger("jump", 0.5f));
        Invoke("Jump", 0.6f);
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
    IEnumerator DelayedTrigger(string triggerName, float delay)
    {   
        yield return new WaitForSeconds(delay);
        anim.SetTrigger(triggerName);
    }
}
