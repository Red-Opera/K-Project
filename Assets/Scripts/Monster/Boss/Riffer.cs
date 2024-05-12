using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Riffer : BossMonster
{
    Animator anim;
    public override void InitSetting()
    {
        boss.range = Resources.Load<GameObject>("Prefab/Boss/AttackRange");
        boss.bossState = Resources.Load<MonsterState>("Scriptable/Boss/Riffer");
        boss.pos = new Vector3(2,0,0);
        boss.damage = 10;
        boss.disapearTime = .8f;
        boss.attackCount = 4;
        anim = GetComponent<Animator>();
    }
    public override void Attack1()
    {
        //공격
        boss.pos = new Vector3(1.7f,-0.8f,0);
        boss.range.transform.localScale = new Vector3(6.4f,6,1);
        GameObject Atk = Instantiate(boss.range, transform.position + boss.pos*boss.dir,Quaternion.identity );
        BossAttack atkSc = Atk.GetComponent<BossAttack>();
        atkSc.setState(boss);
        StartCoroutine(DelayedTrigger("Attack", 0.5f));
    }

    public override void Attack2()
    {
        //마법 공격
        boss.pos = new Vector3(0,0,0);
        boss.range.transform.localScale = new Vector3(1.5f,2,1);
        Rigidbody2D rigid = GetComponent<Rigidbody2D> ();
        var findP = Physics2D.OverlapArea(rigid.position + new Vector2(10,5), rigid.position + new Vector2(-10,-5), LayerMask.GetMask("Player"));
        GameObject Atk = Instantiate(boss.range, findP.transform.position + boss.pos * boss.dir, Quaternion.identity);
        BossAttack atkSc = Atk.GetComponent<BossAttack>();
        atkSc.setState(boss);
        StartCoroutine(DelayedTrigger("mAttack", 0.5f));
    }

    public override void SpecialAttack1()
    {
        //텔포
        boss.pos = new Vector3(0,0,0);
        boss.range.transform.localScale = new Vector3(4.5f,4.8f,1);
        Rigidbody2D rigid = GetComponent<Rigidbody2D> ();
        var findP = Physics2D.OverlapArea(rigid.position + new Vector2(10,5), rigid.position + new Vector2(-10,-5), LayerMask.GetMask("Player"));
        GameObject Atk = Instantiate(boss.range, findP.transform.position + boss.pos * boss.dir, Quaternion.identity);
        BossAttack atkSc = Atk.GetComponent<BossAttack> ();
        atkSc.setState(boss);
        StartCoroutine(DelayedTrigger("telpo", 0.5f));
        StartCoroutine(DelayedTrigger("telpoE", 0.5f));
        Invoke("Telpo", 0.8f);
    }

    public override void SpecialAttack2()
    {
        //차지 공격
        boss.pos = new Vector3(0,0,0);
        boss.range.transform.localScale = new Vector3(4.5f,4.8f,1);
        Rigidbody2D rigid = GetComponent<Rigidbody2D> ();
        var findP = Physics2D.OverlapArea(rigid.position + new Vector2(10,5), rigid.position + new Vector2(-10,-5), LayerMask.GetMask("Player"));
        GameObject Atk = Instantiate(boss.range, findP.transform.position + boss.pos * boss.dir, Quaternion.identity);
        BossAttack atkSc = Atk.GetComponent <BossAttack> ();
        atkSc.setState (boss);
        StartCoroutine(DelayedBool("isCast", 0.5f));
        Invoke("CastE",1.5f);
    }
    void Telpo(){
        Rigidbody2D rigid = GetComponent<Rigidbody2D> ();
        var findP = Physics2D.OverlapArea(rigid.position + new Vector2(10,5), rigid.position + new Vector2(-10,-5), LayerMask.GetMask("Player"));
        rigid.transform.position = new Vector2(findP.transform.position.x - boss.dir * 3,findP.transform.position.y);
    }

    void CastE(){
        anim.SetBool("isCast",false);
    }
    IEnumerator DelayedTrigger(string triggerName, float delay)
    {   
        yield return new WaitForSeconds(delay);
        anim.SetTrigger(triggerName);
    }
    IEnumerator DelayedBool(string triggerName, float delay)
    {   
        yield return new WaitForSeconds(delay);
        anim.SetBool(triggerName, true);
    }

}
