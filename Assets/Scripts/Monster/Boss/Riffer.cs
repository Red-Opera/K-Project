using System.Collections;
using System.Collections.Generic;
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
        boss.disapearTime = .3f;
        boss.attackCount = 4;
        anim = GetComponent<Animator>();
    }
    public override void Attack1()
    {
        //공격
        GameObject Atk = Instantiate(boss.range, transform.position + boss.pos*boss.dir,Quaternion.identity );
        BossAttack atkSc = Atk.GetComponent<BossAttack>();
        atkSc.setState(boss);
        anim.SetTrigger("Attack");
    }

    public override void Attack2()
    {
        //마법 공격
        GameObject Atk = Instantiate(boss.range, transform.position + boss.pos*boss.dir, Quaternion.identity);
        BossAttack atkSc = Atk.GetComponent<BossAttack>();
        atkSc.setState(boss);
        anim.SetTrigger("mAttack");
    }

    public override void SpecialAttack1()
    {
        //텔포
        GameObject Atk = Instantiate(boss.range, transform.position + boss.pos * boss.dir, Quaternion.identity);
        BossAttack atkSc = Atk.GetComponent<BossAttack> ();
        atkSc.setState(boss);
        anim.SetTrigger ("telpo");
        Invoke("Telpo", 0.3f);
        anim.SetTrigger ("telpoE");
    }

    public override void SpecialAttack2()
    {
        //차지 공격
        GameObject Atk = Instantiate(boss.range, transform.position + boss.pos * boss.dir, Quaternion.identity);
        BossAttack atkSc = Atk.GetComponent <BossAttack> ();
        atkSc.setState (boss);
        anim.SetBool("isCast",true);
        Invoke("CastE",1);
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

}
